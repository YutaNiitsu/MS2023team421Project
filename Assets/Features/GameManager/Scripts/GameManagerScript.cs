using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UIElements;
// ゲーム全体を制御する
public class GameManagerScript : MonoBehaviour
{
    [Header("星を配置するエリアの広さ")]
    public Vector2 Range;
    [Header("星の密度(0～1)")]
    public float Threshold;

    private ProceduralGenerator ProceduralGenerator;
    private SaveConstellationData[] ConstellationDatas;
    private uint Score;
    private int DischargeNumber;
    private Rigidbody2D FinalDischargedStar;
    private bool IsFinished;

    // Start is called before the first frame update
    void Start()
    {
        Score = 0;
        DischargeNumber = 20;
        FinalDischargedStar = null;
        IsFinished = false;

        ProceduralGenerator = GetComponent<ProceduralGenerator>();

        ConstellationDatas = GetComponent<ConstellationLoadManager>().LoadData();
        if (ConstellationDatas.Length > 0)
        {
            int index = UnityEngine.Random.Range(0, ConstellationDatas.Length);
            // オブジェクトを配置
            ProceduralGenerator.GenerateTargets(ConstellationDatas[index].constellations);
            ProceduralGenerator.GenerateStars(Range, Threshold);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (FinalDischargedStar != null && !IsFinished)
        {
            if (Vector2.Dot(FinalDischargedStar.velocity, FinalDischargedStar.velocity) <= 0.1f)
            {
                IsFinished = true;
                //ゲームオーバー処理

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
            IsFinished = true;
            //クリア処理

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

}
