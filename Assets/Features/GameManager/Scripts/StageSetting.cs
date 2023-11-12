using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public struct Mission1
{
    [Header("�v���C���[�����𔭎˂ł����")]
    public int DischargeNumber;
    [Header("�����̖��O\n�z��ŕ����w�肵���ꍇ�̓����_���łǂꂩ��I�΂��\n�����w�肵�Ȃ������ꍇ�͑S�Ă̐����f�[�^���烉���_���ň�I�΂��j")]
    public string[] Name;
}
[System.Serializable]
public struct Mission2
{
    [Header("�v���C���[�����𔭎˂ł����")]
    public int DischargeNumber;
    [Header("�����͂ߍ��ތ^�̐��̍ŏ��l�ƍő�l�̊ԂŃ����_���őI��\n�����������l�Ȃ炻�̐��l�̐��̐����������_���őI��")]
    public int minNumber;
    public int maxNumber;
}


[System.Serializable]
public struct StageSetting
{
    [Header("�~�b�V�����Z�b�e�B���O")]
    [Header("�~�b�V�����Ŋ��������鐯���𖼑O�Ŏw�肷��ꍇ")]
    public Mission1[] missions1;

    [Header("�~�b�V�����Ŋ��������鐯���𐯂��͂ߍ��ތ^�̐��Ŏw�肷��ꍇ")]
    public Mission1[] missions2;

    [Header("����z�u����G���A�̍L��")]
    public Vector2 Range;
    [Header("���̖��x(0�`1)")]
    public float Threshold;
}
