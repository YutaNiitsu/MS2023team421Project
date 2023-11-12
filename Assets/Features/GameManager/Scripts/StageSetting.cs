using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    [Header("ミッションセッティング")]
    [Header("ミッションで完成させる星座を名前で指定する場合")]
    public Mission1[] missions1;

    [Header("ミッションで完成させる星座を星をはめ込む型の数で指定する場合")]
    public Mission1[] missions2;

    [Header("星を配置するエリアの広さ")]
    public Vector2 Range;
    [Header("星の密度(0～1)")]
    public float Threshold;
}
