using System;
using System.Collections;
using System.Collections.Generic;
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
    private uint Score;
    private int DischargeNumber;
    private Rigidbody2D FinalDischargedStar;
    private bool IsFinished;

    // Start is called before the first frame update
    void Start()
    {
        Score = 0;
        DischargeNumber = 20;
        FinalDischargedStar = null;
        IsFinished = false;

        ProceduralGenerator = GetComponent<ProceduralGenerator>();

        ConstellationDatas = GetComponent<ConstellationLoadManager>().LoadData(SavedFileName);
        if (ConstellationDatas != null && ConstellationDatas.Length > 0)
        {
            int index = UnityEngine.Random.Range(0, ConstellationDatas.Length);
            // �I�u�W�F�N�g��z�u
            ProceduralGenerator.GenerateTargets(ConstellationDatas[index].constellations);
            ProceduralGenerator.GenerateStars(stageSettings[0].Range, stageSettings[0].Threshold);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (FinalDischargedStar != null && !IsFinished)
        {
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
            //�N���A����

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

    }
    //�Q�[���N���A����
    private void Complete()
    {

    }
}
