using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UIElements;
using static MissionScript;
// ゲーム全体を制御する
public class GameManagerScript : MonoBehaviour
{
    [System.Serializable]
    public struct ST_StarRarity
    {
        public StarRarity rarity;
        public int score;
    }


    [Header("ステージセッティング")]
    public StageSetting StageSettings;
    [Header("星座データのファイル名")]
    public string SavedFileName;
    [Header("はめ込む型にはまった時に取得するスコア")]
    public ST_StarRarity[] TaregtScore;
    [Header("特別ポイントにはまった時に取得するスコア")]
    public ST_StarRarity[] SpecialTaregtScore;
    

    private ProceduralGenerator ProceduralGenerator;
    private SaveConstellationData[] ConstellationDatas;
    private MissionScript[] Missions;
    private int Score;
    private int DischargeNumber;       //プレイヤーが星を発射できる回数
    private Rigidbody2D FinalDischargedStar;
    private bool IsFinished;
    private int MissinNumber;
    private Mission1[] Missions1;
    private Mission2[] Missions2;

    // Start is called before the first frame update
    void Start()
    {
        Score = 0;
        DischargeNumber = StageSettings.DischargeNumber;
        FinalDischargedStar = null;
        IsFinished = false;
        MissinNumber = 0;

        ProceduralGenerator = GetComponent<ProceduralGenerator>();
        ConstellationDatas = GetComponent<ConstellationLoadManager>().LoadData(SavedFileName);
        
        // 星を配置
        ProceduralGenerator.GenerateStars(StageSettings.Range, StageSettings.Threshold);

        SaveConstellationData temp = null;
        foreach (SaveConstellationData i in ConstellationDatas)
        {
            if (i.name == StageSettings.ConstellationName)
            {
                temp = i;
            }
        }

        if (temp != null)
        ProceduralGenerator.GenerateTargets(temp.constellations);

        //ミッション
        foreach (MissionType i in StageSettings.MissionTypes)
        {

        }
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
                StartCoroutine(GameOver());
            }
        }

    }

    //はめ込む型に星がはまるとスコア加算
    //starRarity : はまった星のレアリティ
    //isSpecialPoint : はめ込む型が特別ポイントかどうか
    public void AddScore(StarRarity starRarity, bool isSpecialPoint)
    {
        switch (starRarity)
        {
            case StarRarity.Normal:
                if (isSpecialPoint)
                    Score += SpecialTaregtScore[0].score;
                else
                    Score += TaregtScore[0].score;
                break;
            case StarRarity.Rare:
                if (isSpecialPoint)
                    Score += SpecialTaregtScore[1].score;
                else
                    Score += TaregtScore[1].score;
                break;
            case StarRarity.Unique:
                if (isSpecialPoint)
                    Score += SpecialTaregtScore[2].score;
                else
                    Score += TaregtScore[2].score;
                break;
            case StarRarity.Legendary:
                if (isSpecialPoint)
                    Score += SpecialTaregtScore[3].score;
                else
                    Score += TaregtScore[3].score;
                break;
            default:
                break;
        }
        //全てのはめ込む型に星がはまっているか
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
            StageComplete();
        }

        //if (Mission.IsMissionComplete())
        //{

        //    //ミッション全てクリアしたかどうか
        //    if (Mission.IsAllMissionsComplete())
        //    {
        //        StageComplete();
        //        return;
        //    }
        //    else
        //    {
        //        MissionComplete();
        //    }

        //    if (!Mission.ExecuteMission())
        //    {
        //        Debug.Log("ミッションが設定されていない");
        //    }
        //}
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

    public int GetScore()
    {
        return Score;
    }

    //ゲーム終了したかどうか
    //public bool GetIsFinished()
    //{
    //    return IsFinished;
    //}

    
    //ミッションクリア処理
    //private void MissionComplete()
    //{
    //    Debug.Log("ミッション成功");
    //    //発射可能回数更新
    //    DischargeNumber = Mission.GetDischargeNumber();
    //}
    //ステージクリア処理
    private void StageComplete()
    {
        Debug.Log("ステージクリア");
        Debug.Log(Score);

    }

    //ゲームオーバー処理
    IEnumerator GameOver()
    {
        yield return new WaitForSeconds(1);
        Debug.Log("GameOver");
    }
}
