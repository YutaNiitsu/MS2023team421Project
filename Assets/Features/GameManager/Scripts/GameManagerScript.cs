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
    public StageSetting[] StageSettings;
    [Header("�����f�[�^�̃t�@�C����")]
    public string SavedFileName;

    private ProceduralGenerator ProceduralGenerator;
    private SaveConstellationData[] ConstellationDatas;
    private MissionScript Mission;
    private uint Score;
    private int DischargeNumber;       //�v���C���[�����𔭎˂ł����
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
        DischargeNumber = 0;
        FinalDischargedStar = null;
        IsFinished = false;
        StageNumber = 0;
        MissinNumber = 0;

        ProceduralGenerator = GetComponent<ProceduralGenerator>();
        ConstellationDatas = GetComponent<ConstellationLoadManager>().LoadData(SavedFileName);
        Mission = GetComponent<MissionScript>();
        // ����z�u
        ProceduralGenerator.GenerateStars(StageSettings[StageNumber].Range, StageSettings[StageNumber].Threshold);


        //�~�b�V����
        // ������z�u
        Mission.SetMission(StageSettings[StageNumber], ConstellationDatas);
        DischargeNumber = Mission.GetDischargeNumber();
        //�~�b�V�������s
        if (!Mission.ExecuteMission())
        {
            Debug.Log("�~�b�V�������ݒ肳��Ă��Ȃ�");
        }
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

        //�S�Ă̂͂ߍ��ތ^�ɐ����͂܂��Ă��邩
        if (Mission.IsMissionComplete())
        {

            //�~�b�V�����S�ăN���A�������ǂ���
            if (Mission.IsAllMissionsComplete())
            {
                StageComplete();
                return;
            }
            else
            {
                MissionComplete();
            }

            if (!Mission.ExecuteMission())
            {
                Debug.Log("�~�b�V�������ݒ肳��Ă��Ȃ�");
            }
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
        Debug.Log("�~�b�V��������");
        //���ˉ\�񐔍X�V
        DischargeNumber = Mission.GetDischargeNumber();
    }
    //�X�e�[�W�N���A����
    private void StageComplete()
    {
        Debug.Log("�X�e�[�W�N���A");
        //StageNumber++;
        //if (StageSettings.Length > StageNumber)
        //{
        //    //���̃X�e�[�W��
        //}
        //else
        //{
        //    //
        //}
    }
}
