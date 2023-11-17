using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UIElements;
using static MissionScript;
// �Q�[���S�̂𐧌䂷��
public class GameManagerScript : MonoBehaviour
{
    [System.Serializable]
    public struct ST_StarRarity
    {
        public StarRarity rarity;
        public int score;
    }


    [Header("�X�e�[�W�Z�b�e�B���O")]
    public StageSetting Setting;
    [Header("�����f�[�^�̃t�@�C����")]
    public string SavedFileName;
    [Header("�͂ߍ��ތ^�ɂ͂܂������Ɏ擾����X�R�A")]
    public ST_StarRarity[] TaregtScore;
    [Header("���ʃ|�C���g�ɂ͂܂������Ɏ擾����X�R�A")]
    public ST_StarRarity[] SpecialTaregtScore;
    

    private ProceduralGenerator ProceduralGenerator;
    private SaveConstellationData[] ConstellationDatas;
    private MissionScript[] Missions;
    private int Score;
    private int DischargeNumber;       //�v���C���[�����𔭎˂ł����
    private Rigidbody2D FinalDischargedStar;
    private bool IsFinished;
    private bool IsStageComplete;

    // Start is called before the first frame update
    void Start()
    {
        Score = 0;
        DischargeNumber = Setting.DischargeNumber;
        FinalDischargedStar = null;
        IsFinished = false;
        IsStageComplete = false;

        ProceduralGenerator = GetComponent<ProceduralGenerator>();
        ConstellationDatas = GetComponent<ConstellationLoadManager>().LoadData(SavedFileName);
        
        // ����z�u
        ProceduralGenerator.GenerateStars(Setting.Range, Setting.Threshold);

        SaveConstellationData temp = null;
        foreach (SaveConstellationData i in ConstellationDatas)
        {
            if (i.name == Setting.ConstellationName)
            {
                temp = i;
            }
        }

        if (temp != null)
        ProceduralGenerator.GenerateTargets(temp.constellations);

        //�~�b�V����
        int len = Setting.MissionTypes.Length;
        Missions = new MissionScript[len];
        int index = 0;
        foreach (MissionType i in Setting.MissionTypes)
        {
            Missions[index] = new MissionScript(i);
            index++;
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
                StartCoroutine(GameOver());
            }
        }

    }

    //�͂ߍ��ތ^�ɐ����͂܂�ƃX�R�A���Z
    //starRarity : �͂܂������̃��A���e�B
    //isSpecialPoint : �͂ߍ��ތ^�����ʃ|�C���g���ǂ���
    public void AddScore(StarRarity starRarity, bool isSpecialPoint)
    {
        switch (starRarity)
        {
            case StarRarity.Normal:
                if (isSpecialPoint)
                    Score += SpecialTaregtScore[0].score;
                else
                    Score += TaregtScore[0].score;
                break;
            case StarRarity.Rare:
                if (isSpecialPoint)
                    Score += SpecialTaregtScore[1].score;
                else
                    Score += TaregtScore[1].score;
                break;
            case StarRarity.Unique:
                if (isSpecialPoint)
                    Score += SpecialTaregtScore[2].score;
                else
                    Score += TaregtScore[2].score;
                break;
            case StarRarity.Legendary:
                if (isSpecialPoint)
                    Score += SpecialTaregtScore[3].score;
                else
                    Score += TaregtScore[3].score;
                break;
            default:
                break;
        }
        //�S�Ă̂͂ߍ��ތ^�ɐ����͂܂��Ă��邩
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
            StageComplete();
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

    public int GetScore()
    {
        return Score;
    }

    //�Q�[���I���������ǂ���
    public bool GetIsFinished()
    {
        return IsFinished;
    }

    //�X�e�[�W�I���������ǂ���
    public bool GetIsStageComplete()
    {
        return IsStageComplete;
    }

    //���ˉ\��
    public int GetDischargeNumber()
    {
        return DischargeNumber;
    }

    //�X�e�[�W�N���A����
    private void StageComplete()
    {
        Debug.Log("�X�e�[�W�N���A");
        Debug.Log(Score);
        IsStageComplete = true;
    }

    //�Q�[���I�[�o�[����
    IEnumerator GameOver()
    {
        yield return new WaitForSeconds(1);
        Debug.Log("GameOver");
    }
}