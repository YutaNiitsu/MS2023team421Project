using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public struct Mission
{
    [Header("���˃^�[����")]
    public int DischargeNumber;
    [Header("�͂ߍ��ތ^�̐�")]
    public int TargetNum;
}
[System.Serializable]
public struct StageSetting
{
    [Header("�~�b�V�����Z�b�e�B���O")]
    public Mission[] missions;
    [Header("����z�u����G���A�̍L��")]
    public Vector2 Range;
    [Header("���̖��x(0�`1)")]
    public float Threshold;
}
