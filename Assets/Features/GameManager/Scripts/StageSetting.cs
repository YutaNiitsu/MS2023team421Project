using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MissionScript;
using UnityEngine.UI;

[System.Serializable]
public struct StageSetting
{
    [Header("�ݒu���鐯���̖��O")]
    public string ConstellationName;
    [Header("�ݒu���鐯���̉摜")]
    public Sprite ConstellationImage;
    [Header("�v���C���[�����𔭎˂ł����")]
    public int DischargeNumber;
    [Header("����z�u����G���A�̍L��")]
    public Vector2 StageSize;
    [Header("臒l�A���̒l���Ⴂ�قǐ�������鐯�������Ȃ�")]
    [SerializeField, Range(0f, 1f)]
    public float Threshold;
    [Header("���̃��A���e�B�ݒ肷�邽�߂̒l")]
    [SerializeField, Range(0f, 1f)]
    public float NormalPoint;
    [SerializeField, Range(0f, 1f)]
    public float RarePoint;
    [SerializeField, Range(0f, 1f)]
    public float UniquePoint;
    [SerializeField, Range(0f, 1f)]
    public float LegendaryPoint;

    [Header("���ʃ|�C���g�̐�")]
    public int SpecialPointNumber;
    [Header("�V�[���h�̐�")]
    public int ShieldNumber;
    [Header("�V�[���h��HealthPoint")]
    [SerializeField, Range(1, 4)]
    public int ShieldHealthPoint;
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
