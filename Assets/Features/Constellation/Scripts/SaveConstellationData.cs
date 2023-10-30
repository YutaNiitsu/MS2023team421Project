using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[System.Serializable]
public class SaveConstellationData
{
    // ���삵���N����
    public int year;
    public int month;
    public int day;
    // ���O
    public string name;
    // �����͂ߍ��ތ^�̈ʒu
    public Vector2[] tagerPos;
}