using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[System.Serializable]
public class SaveConstellationData
{
    // 制作した年月日
    public int year;
    public int month;
    public int day;
    // 名前
    public string name;
    // 星をはめ込む型の位置
    public Vector2[] tagerPos;
}