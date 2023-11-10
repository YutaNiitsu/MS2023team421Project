using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public struct Mission
{
    [Header("発射ターン数")]
    public int DischargeNumber;
    [Header("はめ込む型の数")]
    public int TargetNum;
}
[System.Serializable]
public struct StageSetting
{
    [Header("ミッションセッティング")]
    public Mission[] missions;
    [Header("星を配置するエリアの広さ")]
    public Vector2 Range;
    [Header("星の密度(0～1)")]
    public float Threshold;
}
