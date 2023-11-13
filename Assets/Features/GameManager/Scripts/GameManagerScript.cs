using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UIElements;
// ゲーム全体を制御する
public class GameManagerScript : MonoBehaviour
{
    [Header("ステージセッティング")]
    public StageSetting[] stageSettings;
    [Header("星座データのファイル名")]
    public string SavedFileName;

    private ProceduralGenerator ProceduralGenerator;
    private SaveConstellationData[] ConstellationDatas;
    private MissionScript Mission;
    private uint Score;
    private int DischargeNumber;
    private Rigidbody2D FinalDischargedStar;
    private bool IsFinished;
    private int StageNumber;
    private int MissinNumber;
    private Mission1[] Missions1;
    private Mission2[] Missions2;

    // Start is called before the first frame update
    void Start()
    {
        Score = 0;
        DischargeNumber = 20;
        FinalDischargedStar = null;
        IsFinished = false;
        StageNumber = 0;
        MissinNumber = 0;

        ProceduralGenerator = GetComponent<ProceduralGenerator>();
        ConstellationDatas = GetComponent<ConstellationLoadManager>().LoadData(SavedFileName);
        Mission = GetComponent<MissionScript>();
        // 星を配置
        ProceduralGenerator.GenerateStars(stageSettings[StageNumber].Range, stageSettings[StageNumber].Threshold);


        //ミッション
        // 星座を配置
        SaveConstellationData determination = null;
        determination = Mission.SetMission(stageSettings[StageNumber], ConstellationDatas);
        if (determination != null)
            ProceduralGenerator.GenerateTargets(determination.constellations);
    }

    // Update is called once per frame
    void Update()
    {
        if (FinalDischargedStar != null && !IsFinished)
        {
            //星が停止した
            if (Vector2.Dot(FinalDischargedStar.velocity, FinalDischargedStar.velocity) <= 0.1f)
            {
                IsFinished = true;
                //ゲームオーバー処理
                GameOver();
            }
        }

    }

    //はめ込む型に星がはまるとスコア加算
    public void AddScore(int starRarity)
    {
        Score += 10;

        bool isComp = true;
        foreach (TargetScript i in ProceduralGenerator.GetTargets())
        {
            if (!i.IsGoal())
            {
                //星がはまっていないものがあったら失敗
                isComp = false;
                break;
            }
        }

        if (isComp)
        {
            //全部はまっていた
            IsFinished = true;
            //ミッションクリア処理

        }
    }

    //星を飛ばすときの処理
    //rb : 飛ばした星
    public void Discharge(Rigidbody2D rb)
    {
        DischargeNumber--;

        //発射可能回数が0になった
        if (DischargeNumber <= 0)
        {
            FinalDischargedStar = rb;
        }
    }

    public uint GetScore()
    {
        return Score;
    }

    //ゲーム終了したかどうか
    public bool GetIsFinished()
    {
        return IsFinished;
    }

    //ゲームオーバー処理
    private void GameOver()
    {
        Debug.Log("GameOver");
    }
    //ミッションクリア処理
    private void MissionComplete()
    {

    }
    //ゲームクリア処理
    private void Complete()
    {
        Debug.Log("Clear");
    }
}
