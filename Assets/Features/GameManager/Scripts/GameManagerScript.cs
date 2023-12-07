using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    [Header("����Scene�̖��O")]
    public string NextSceneName;

    public ProceduralGenerator ProceduralGenerator { get; protected set; }
    public SaveConstellationData[] ConstellationDatas { get; protected set; }
    public SaveConstellationData GenerateConstellation { get; protected set; }  //�������ꂽ����
    private MissionScript[] Missions;
    private DrawConstellationLine DrawLine;
    public int Score { get; protected set; }
    public int DischargeNumber { get; protected set; }       //�v���C���[�����𔭎˂ł����
    private Rigidbody2D FinalDischargedStar;
    public bool IsFinished { get; protected set; }
    public bool IsStageComplete { get; protected set; }
    public int ObstacleCollisionNumber { get; protected set; } //��Q���Փˉ�
    public int ObstacleDestroyNumber { get; protected set; }     //��Q���j���
    private MovableObstacleManagerScript MovableObstacleMgr;


    //�V���O���g��
    public static GameManagerScript instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }




    // Start is called before the first frame update
    void Start()
    {
        Score = 0;
        DischargeNumber = Setting.DischargeNumber;
        FinalDischargedStar = null;
        IsFinished = false;
        IsStageComplete = false;
        ObstacleCollisionNumber = 0;
        ObstacleDestroyNumber = 0;

        ProceduralGenerator = GetComponent<ProceduralGenerator>();
        ConstellationDatas = GetComponent<ConstellationLoadManager>().LoadData(SavedFileName);
        DrawLine = GetComponent<DrawConstellationLine>();
        MovableObstacleMgr = GetComponent<MovableObstacleManagerScript>();

        // ����z�u
        ProceduralGenerator.CreateStar(Setting.StageSize, Setting.Threshold);

        SaveConstellationData temp = null;
        foreach (SaveConstellationData i in ConstellationDatas)
        {
            if (i.name == Setting.ConstellationName)
            {
                temp = i;
            }
        }

        //���O����v���鐯���Ȃ������烉���_��
        {
            if (temp == null)
            {
                int index = UnityEngine.Random.Range(0, ConstellationDatas.Length - 1);
                temp = ConstellationDatas[index];
            }
        }
        

        if (temp != null)
        {
            ProceduralGenerator.CreateTargets(temp);
            GenerateConstellation = temp;
        }


        //�~�b�V����
        {
            int len = Setting.MissionTypes.Length;
            Missions = new MissionScript[len];
            int index = 0;
            foreach (MissionType i in Setting.MissionTypes)
            {
                Missions[index] = new MissionScript(i);
                index++;
            }
        }
        //SceneManager.LoadScene();
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
        if (ProceduralGenerator.IsAllGoaled())
        {
            //�S���͂܂��Ă���
            IsFinished = true;
            StartCoroutine(StageComplete());
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
        else
        {
            //������Q������
            int direction = (int)UnityEngine.Random.Range(0.0f, 7.0f);
            //MovableObstacleMgr.Generate(direction);
        }
    }


    //�X�e�[�W�N���A����
    IEnumerator StageComplete()
    {
        Debug.Log("�X�e�[�W�N���A");
        Debug.Log(Score);
        IsStageComplete = true;

        DrawLine.DrawLine();

        foreach (MissionScript i in Missions)
        {
            i.IsMissionComplete();
        }

        yield return new WaitForSeconds(1);
        Debug.Log("�X�e�[�W�N���A�����I��");
    }

    //�Q�[���I�[�o�[����
    IEnumerator GameOver()
    {
        yield return new WaitForSeconds(1);
        Debug.Log("GameOver");
    }


    //��Q���ƏՓ˂�����
    public void CollisionObstacle()
    {
        ObstacleCollisionNumber++;
        Debug.Log("�Փ�");
    }

    //��Q����j�󂵂���
    public void DestroyObstacle()
    {
        ObstacleDestroyNumber++;
        Debug.Log("�j��");
    }

    //���̃V�[���ֈڍs
    public void NextScene()
    {
        SceneManager.LoadScene(NextSceneName);
    }
}
