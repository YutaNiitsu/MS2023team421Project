using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
// 線の構造体
[System.Serializable]
public struct Line
{
    // 線の始点
    public Vector3 start;
    // 線の終点
    public Vector3 end;
    // 始点にある星をはめ込む型の要素番号
    public int startTargetKey;
    // 終点にある星をはめ込む型の要素番号
    public int endTargetKey;

}

// セーブデータのクラス
[System.Serializable]
public class SaveConstellationData
{
    // 制作した年月日
    public int year { get; protected set; }
    public int month { get; protected set; }
    public int day { get; protected set; }
    // ID
    public uint id { get; protected set; }
    // 名前
    public string name { get; protected set; }
    // 星をはめ込む型の位置
    public ST_Constellation[] targets { get; protected set; }
    // 星同士をつなぐ線
    public Line[] lines { get; protected set; }

    public SaveConstellationData_Conv DataConv { get; protected set; }

    // saveデータ作成
    public void Create(int _year, int _month, int _day,
        uint _id, string _name, ST_Constellation[] _constellations, Line[] _lines)
    {
        year = _year;
        month = _month;
        day = _day;
        id = _id;
        name = _name;
        targets = _constellations;
        lines = _lines;

        Conversion();
    }
    // IDを設定
    public void SetID(uint _id)
    {
        id = _id;
        Conversion();
    }

    // JSONに変換できるように変換
    private void Conversion()
    {
        DataConv = new SaveConstellationData_Conv();
        DataConv.year = year;
        DataConv.month = month;
        DataConv.day = day;
        DataConv.id = id;
        DataConv.name = name;
        DataConv.targets = new string[targets.Length];
        int index = 0;
        foreach (ST_Constellation i in targets)
        {
            DataConv.targets[index] = JsonUtility.ToJson(i);
            index++;
        }
        DataConv.lines = new string[lines.Length];
        index = 0;
        foreach (Line i in lines)
        {
            DataConv.lines[index] = JsonUtility.ToJson(i);
            index++;
        }
    }
}

// JSONに変換できるように変換した星座のデータ
[System.Serializable]
public class SaveConstellationData_Conv
{
    // 制作した年月日
    public int year;
    public int month;
    public int day;
    // ID
    public uint id;
    // 名前
    public string name;
    // 星をはめ込む型の位置
    public string[] targets;
    // 星同士をつなぐ線
    public string[] lines;

    // SaveConstellationData型に変換してゲーム内で使用できるようにする
    public SaveConstellationData Normalize()
    {
        SaveConstellationData data = new SaveConstellationData();

        ST_Constellation[] _constellations = new ST_Constellation[targets.Length];
        int index = 0;
        foreach (string i in targets)
        {
            _constellations[index] = JsonUtility.FromJson<ST_Constellation>(i);
            index++;
        }
        Line[] _lines = new Line[lines.Length];
        index = 0;
        foreach (string i in lines)
        {
            _lines[index] = JsonUtility.FromJson<Line>(i);
            index++;
        }
        data.Create(year, month, day, id, name, _constellations, _lines);
        return data;
    }
}

// 変換した星座のデータの配列をメンバにして最終的にJSONに変換するためのクラス
[System.Serializable]
public class SaveConstellationData_ConvArray
{
    public SaveConstellationData_Conv[] saveConstellationDatas;

}