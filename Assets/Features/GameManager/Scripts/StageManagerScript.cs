using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UIElements;
using static MissionScript;
// �Q�[���S�̂𐧌䂷��
public class StageManagerScript : MonoBehaviour
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
    [Header("BGM�̖��O")]
    public string BGM_Name;

    public ProceduralGenerator ProceduralGenerator { get; protected set; }
    public SaveConstellationData[] ConstellationDatas { get; protected set; }
    public SaveConstellationData GenerateConstellation { get; protected set; }  //�������ꂽ����
    protected MissionScript[] Missions;
    protected DrawConstellationLine DrawLine;
    public int Score { get; protected set; }
    public int DischargeNumber { get; protected set; }       //�v���C���[�����𔭎˂ł����
    private Rigidbody2D FinalDischargedStar;
    public bool IsFinished { get; protected set; }
    public bool IsStageComplete { get; protected set; }
    public int ObstacleCollisionNumber { get; protected set; } //��Q���Փˉ�
    public int ObstacleDestroyNumber { get; protected set; }     //��Q���j���
    private MovableObstacleManagerScript MovableObstacleMgr;
    public UIManagerScript UIManager { get; protected set; }
    protected GameObject MainCamera;

    private void Awake()
    {
        GameManagerScript.instance.Set(this);

    }
    
    // Start is called before the first frame update
    void Start()
    {
        StageManagerStart();
       
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

        //�e�X�g�p
        if (Input.GetKeyDown(KeyCode.P))
        {
            UIManager.PauseGame();
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            SoundManager.instance.StopBGM(BGM_Name);
        }
        
    }

    public virtual void StageManagerStart()
    {
        Initialize();
        CreateObjects();
    }

    public virtual void Initialize()
    {
        GameManagerScript.instance.Set(this);
        Score = 0;
        DischargeNumber = Setting.DischargeNumber;
        FinalDischargedStar = null;
        IsFinished = false;
        IsStageComplete = false;
        ObstacleCollisionNumber = 0;
        ObstacleDestroyNumber = 0;

        ProceduralGenerator = GetComponent<ProceduralGenerator>();
        ConstellationDatas = ConstellationLoadManager.instance.LoadData(SavedFileName);
        DrawLine = GetComponent<DrawConstellationLine>();
        MovableObstacleMgr = GetComponent<MovableObstacleManagerScript>();
        UIManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManagerScript>();
        MainCamera = GameObject.FindGameObjectWithTag("MainCamera");

        SoundManager.instance.PlayBGM(BGM_Name);
    }

    public virtual void CreateObjects()
    {
        //�����f�[�^���������琶�����Ȃ�
        if (ConstellationDatas == null)
            return;

        //���Ə�Q����z�u
        ProceduralGenerator.CreateStarObstacle(Setting.StageSize, Setting.Threshold);

        SaveConstellationData temp = null;
        foreach (SaveConstellationData i in ConstellationDatas)
        {
            if (i.name == Setting.ConstellationName)
            {
                temp = i;
            }
        }

        //�͂ߍ��ތ^��z�u
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


        //�~�b�V����������
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
    }
    //hs�ڍ��ތ^�ɐ����͂܂�����
    public virtual void PutOnTareget()
    {

    }

    //�͂ߍ��ތ^�ɐ����͂܂�ƃX�R�A���Z
    //starRarity : �͂܂������̃��A���e�B
    //isSpecialPoint : �͂ߍ��ތ^�����ʃ|�C���g���ǂ���
    public virtual void AddScore(StarRarity starRarity, bool isSpecialPoint)
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
            StageComplete();
        }

       
    }

    //���N���b�N������
    public virtual void ClickStar()
    {
        
    }

    //�����΂��Ƃ��̏���
    //rb : ��΂�����
    public virtual void Discharge(Rigidbody2D rb)
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
            int direction = UnityEngine.Random.Range(0, 7);
            float rand = UnityEngine.Random.Range(0.01f, 1.0f);
            if (rand <= Setting.ProbabilityMovableObstacle)
            {
                MovableObstacleMgr.Create(direction);
            }

        }
    }


    //�X�e�[�W�N���A����
    public virtual void StageComplete()
    {
        StartCoroutine(StageCompleteCoroutine());
    }
    IEnumerator StageCompleteCoroutine()
    {
        SoundManager.instance.StopBGM(BGM_Name);
        SoundManager.instance.PlaySE("Complete");
        Debug.Log("�X�e�[�W�N���A");
        Debug.Log(Score);

        //�J�����̈ʒu�������ʒu�ɂ���
        MainCamera.transform.position = new Vector3(0.0f, 0.0f, -11.0f);
        IsStageComplete = true;
        DrawLine.DrawLine();

        foreach (MissionScript i in Missions)
        {
            //���U���g�Ƀ~�b�V�������ڒǉ�
            ResultScript result = UIManager.Result.GetComponent<ResultScript>();
            result.AddMissionResult(i.IsMissionComplete(), i.Type);
        }

        //���̕`�抮����҂�
        while (!DrawLine.FinishDraw())
        {
            yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForSeconds(0);

        UIManager.DisplayResult();
        Debug.Log("�X�e�[�W�N���A�����I��");
    }

    //�Q�[���I�[�o�[����
    void GameOver()
    {
        SoundManager.instance.StopBGM("BGM1");
        SoundManager.instance.PlaySE("GameOver");
        Time.timeScale = 0;
        UIManager.DisplayGameOver();
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

}
