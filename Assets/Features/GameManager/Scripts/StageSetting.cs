using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MissionScript;

[System.Serializable]
public struct Mission1
{
    [Header("プレイヤーが星を発射できる回数")]
    public int DischargeNumber;
    [Header("星座の名前\n配列で複数指定した場合はランダムでどれか一つ選ばれる\n何も指定しなかった場合は全ての星座データからランダムで一つ選ばれる）")]
    public string[] Name;
}
[System.Serializable]
public struct Mission2
{
    [Header("プレイヤーが星を発射できる回数")]
    public int DischargeNumber;
    [Header("星をはめ込む型の数の最小値と最大値の間でランダムで選ぶ\n両方同じ数値ならその数値の数の星座をランダムで選ぶ")]
    public int minNumber;
    public int maxNumber;
}


[System.Serializable]
public struct StageSetting
{
    [Header("設置する星座の名前")]
    public string ConstellationName;
    [Header("プレイヤーが星を発射できる回数")]
    public int DischargeNumber;
    [Header("星を配置するエリアの広さ")]
    public Vector2 Range;
    [Header("星の密度(0～1)")]
    public float Threshold;
    [Header("ミッションセッティング")]
    [Header("ミッションの種類選択")]
    public MissionType[] MissionTypes;
    [Header("ミッションの種類ごとの設定")]
    [Header("障害物衝突回数ｎ回以下")]
    public float ObstacleCollisionNumber;
    [Header("ｎターン以内にクリア")]
    public int DischargeNumberWithinClear;
    [Header("障害物ｎ回以上壊す")]
    public int ObstacleDestroyNumber;
}
