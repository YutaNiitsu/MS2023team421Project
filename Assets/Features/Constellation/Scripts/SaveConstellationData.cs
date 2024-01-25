using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
// ���̍\����
[System.Serializable]
public struct Line
{
    // ���̎n�_
    public Vector3 start;
    // ���̏I�_
    public Vector3 end;
    // �n�_�ɂ��鐯���͂ߍ��ތ^�̗v�f�ԍ�
    public int startTargetKey;
    // �I�_�ɂ��鐯���͂ߍ��ތ^�̗v�f�ԍ�
    public int endTargetKey;

}

// �Z�[�u�f�[�^�̃N���X
[System.Serializable]
public class SaveConstellationData
{
    // ���삵���N����
    public int year { get; protected set; }
    public int month { get; protected set; }
    public int day { get; protected set; }
    // ID
    public uint id { get; protected set; }
    // ���O
    public string name { get; protected set; }
    // �����͂ߍ��ތ^�̈ʒu
    public ST_Constellation[] targets { get; protected set; }
    // �����m���Ȃ���
    public Line[] lines { get; protected set; }

    public SaveConstellationData_Conv DataConv { get; protected set; }

    // save�f�[�^�쐬
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
    // ID��ݒ�
    public void SetID(uint _id)
    {
        id = _id;
        Conversion();
    }

    // JSON�ɕϊ��ł���悤�ɕϊ�
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

// JSON�ɕϊ��ł���悤�ɕϊ����������̃f�[�^
[System.Serializable]
public class SaveConstellationData_Conv
{
    // ���삵���N����
    public int year;
    public int month;
    public int day;
    // ID
    public uint id;
    // ���O
    public string name;
    // �����͂ߍ��ތ^�̈ʒu
    public string[] targets;
    // �����m���Ȃ���
    public string[] lines;

    // SaveConstellationData�^�ɕϊ����ăQ�[�����Ŏg�p�ł���悤�ɂ���
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

// �ϊ����������̃f�[�^�̔z��������o�ɂ��čŏI�I��JSON�ɕϊ����邽�߂̃N���X
[System.Serializable]
public class SaveConstellationData_ConvArray
{
    public SaveConstellationData_Conv[] saveConstellationDatas;

}