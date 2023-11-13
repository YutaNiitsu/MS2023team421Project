using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UIElements;
// �Q�[���S�̂𐧌䂷��
public class GameManagerScript : MonoBehaviour
{
    [Header("�X�e�[�W�Z�b�e�B���O")]
    public StageSetting[] stageSettings;
    [Header("�����f�[�^�̃t�@�C����")]
    public string SavedFileName;

    private ProceduralGenerator ProceduralGenerator;
    private SaveConstellationData[] ConstellationDatas;
    private MissionScript Mission;
    private uint Score;
    private int DischargeNumber;
    private Rigidbody2D FinalDischargedStar;
    private bool IsFinished;
    private int StageNumber;
    private int MissinNumber;
    private Mission1[] Missions1;
    private Mission2[] Missions2;

    // Start is called before the first frame update
    void Start()
    {
        Score = 0;
        DischargeNumber = 20;
        FinalDischargedStar = null;
        IsFinished = false;
        StageNumber = 0;
        MissinNumber = 0;

        ProceduralGenerator = GetComponent<ProceduralGenerator>();
        ConstellationDatas = GetComponent<ConstellationLoadManager>().LoadData(SavedFileName);
        Mission = GetComponent<MissionScript>();
        // ����z�u
        ProceduralGenerator.GenerateStars(stageSettings[StageNumber].Range, stageSettings[StageNumber].Threshold);


        //�~�b�V����
        // ������z�u
        SaveConstellationData determination = null;
        determination = Mission.SetMission(stageSettings[StageNumber], ConstellationDatas);
        if (determination != null)
            ProceduralGenerator.GenerateTargets(determination.constellations);
    }

    // Update is called once per frame
    void Update()
    {
        if (FinalDischargedStar != null && !IsFinished)
        {
            //������~����
            if (Vector2.Dot(FinalDischargedStar.velocity, FinalDischargedStar.velocity) <= 0.1f)
            {
                IsFinished = true;
                //�Q�[���I�[�o�[����
                GameOver();
            }
        }

    }

    //�͂ߍ��ތ^�ɐ����͂܂�ƃX�R�A���Z
    public void AddScore(int starRarity)
    {
        Score += 10;

        bool isComp = true;
        foreach (TargetScript i in ProceduralGenerator.GetTargets())
        {
            if (!i.IsGoal())
            {
                //�����͂܂��Ă��Ȃ����̂��������玸�s
                isComp = false;
                break;
            }
        }

        if (isComp)
        {
            //�S���͂܂��Ă���
            IsFinished = true;
            //�~�b�V�����N���A����

        }
    }

    //�����΂��Ƃ��̏���
    //rb : ��΂�����
    public void Discharge(Rigidbody2D rb)
    {
        DischargeNumber--;

        //���ˉ\�񐔂�0�ɂȂ���
        if (DischargeNumber <= 0)
        {
            FinalDischargedStar = rb;
        }
    }

    public uint GetScore()
    {
        return Score;
    }

    //�Q�[���I���������ǂ���
    public bool GetIsFinished()
    {
        return IsFinished;
    }

    //�Q�[���I�[�o�[����
    private void GameOver()
    {
        Debug.Log("GameOver");
    }
    //�~�b�V�����N���A����
    private void MissionComplete()
    {

    }
    //�Q�[���N���A����
    private void Complete()
    {
        Debug.Log("Clear");
    }
}
