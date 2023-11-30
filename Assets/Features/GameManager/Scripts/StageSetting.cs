using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MissionScript;


[System.Serializable]
public struct StageSetting
{
    [Header("�ݒu���鐯���̖��O")]
    public string ConstellationName;
    [Header("�v���C���[�����𔭎˂ł����")]
    public int DischargeNumber;
    [Header("����z�u����G���A�̍L��")]
    public Vector2 StageSize;
    [Header("臒l�A���̒l���Ⴂ�قǐ�������鐯�������Ȃ�")]
    [SerializeField, Range(0f, 1f)]
    public float Threshold;
    [Header("�~�b�V�����Z�b�e�B���O")]
    [Header("�~�b�V�����̎�ޑI��")]
    public MissionType[] MissionTypes;
    [Header("�~�b�V�����̎�ނ��Ƃ̐ݒ�")]
    [Header("��Q���Փˉ񐔂���ȉ�")]
    public int ObstacleCollisionNumber;
    [Header("���^�[���ȓ��ɃN���A")]
    public int DischargeNumberWithinClear;
    [Header("��Q������ȏ��")]
    public int ObstacleDestroyNumber;
}
