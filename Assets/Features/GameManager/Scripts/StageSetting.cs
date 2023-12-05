using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MissionScript;


[System.Serializable]
public struct StageSetting
{
    [Header("設置する星座の名前")]
    public string ConstellationName;
    [Header("プレイヤーが星を発射できる回数")]
    public int DischargeNumber;
    [Header("星を配置するエリアの広さ")]
    public Vector2 StageSize;
    [Header("閾値、この値が低いほど生成される星が多くなる")]
    [SerializeField, Range(0f, 1f)]
    public float Threshold;
    [Header("星のレアリティ設定するための閾値")]
    [SerializeField, Range(0f, 1f)]
    public float NormalThreshold;
    [SerializeField, Range(0f, 1f)]
    public float RareThreshold;
    [SerializeField, Range(0f, 1f)]
    public float UniqueThreshold;
    [SerializeField, Range(0f, 1f)]
    public float LegendaryThreshold;

    [Header("特別ポイントの数")]
    public int SpecialPointNumber;
    [Header("シールドの数")]
    public int ShieldNumber;
    [Header("ミッションセッティング")]
    [Header("ミッションの種類選択")]
    public MissionType[] MissionTypes;
    [Header("ミッションの種類ごとの設定")]
    [Header("障害物衝突回数ｎ回以下")]
    public int ObstacleCollisionNumber;
    [Header("ｎターン以内にクリア")]
    public int DischargeNumberWithinClear;
    [Header("障害物ｎ回以上壊す")]
    public int ObstacleDestroyNumber;
}
