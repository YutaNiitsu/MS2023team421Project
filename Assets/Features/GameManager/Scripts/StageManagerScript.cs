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
// ゲーム全体を制御する
public class StageManagerScript : MonoBehaviour
{
    [System.Serializable]
    public struct ST_StarRarity
    {
        public StarRarity rarity;
        public int score;
    }


    [Header("ステージセッティング")]
    public StageSetting Setting;
    [Header("星座データのファイル名")]
    public string SavedFileName;
    [Header("はめ込む型にはまった時に取得するスコア")]
    public ST_StarRarity[] TaregtScore;
    [Header("特別ポイントにはまった時に取得するスコア")]
    public ST_StarRarity[] SpecialTaregtScore;
    [Header("次のSceneの名前")]
    public string NextSceneName;
    [Header("BGMの名前")]
    public string BGM_Name;

    public ProceduralGenerator ProceduralGenerator { get; protected set; }
    public SaveConstellationData[] ConstellationDatas { get; protected set; }
    public SaveConstellationData GenerateConstellation { get; protected set; }  //生成された星座
    protected MissionScript[] Missions;
    protected DrawConstellationLine DrawLine;
    public int Score { get; protected set; }
    public int DischargeNumber { get; protected set; }       //プレイヤーが星を発射できる回数
    private Rigidbody2D FinalDischargedStar;
    public bool IsFinished { get; protected set; }
    public bool IsStageComplete { get; protected set; }
    public int ObstacleCollisionNumber { get; protected set; } //障害物衝突回数
    public int ObstacleDestroyNumber { get; protected set; }     //障害物破壊回数
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
            //星が停止した
            if (Vector2.Dot(FinalDischargedStar.velocity, FinalDischargedStar.velocity) <= 0.1f)
            {
                IsFinished = true;
                //ゲームオーバー処理
                GameOver();
            }
        }

        //テスト用
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
        //星座データ無かったら生成しない
        if (ConstellationDatas == null)
            return;

        //星と障害物を配置
        ProceduralGenerator.CreateStarObstacle(Setting.StageSize, Setting.Threshold);

        SaveConstellationData temp = null;
        foreach (SaveConstellationData i in ConstellationDatas)
        {
            if (i.name == Setting.ConstellationName)
            {
                temp = i;
            }
        }

        //はめ込む型を配置
        //名前が一致する星座なかったらランダム
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


        //ミッションを決定
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
    //hs目込む型に星がはまった時
    public virtual void PutOnTareget()
    {

    }

    //はめ込む型に星がはまるとスコア加算
    //starRarity : はまった星のレアリティ
    //isSpecialPoint : はめ込む型が特別ポイントかどうか
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

        //全てのはめ込む型に星がはまっているか
        if (ProceduralGenerator.IsAllGoaled())
        {
            //全部はまっていた
            IsFinished = true;
            StageComplete();
        }

       
    }

    //星クリックした時
    public virtual void ClickStar()
    {
        
    }

    //星を飛ばすときの処理
    //rb : 飛ばした星
    public virtual void Discharge(Rigidbody2D rb)
    {
        DischargeNumber--;

        //発射可能回数が0になった
        if (DischargeNumber <= 0)
        {
            FinalDischargedStar = rb;
        }
        else
        {
            //動く障害物生成
            int direction = UnityEngine.Random.Range(0, 7);
            float rand = UnityEngine.Random.Range(0.01f, 1.0f);
            if (rand <= Setting.ProbabilityMovableObstacle)
            {
                MovableObstacleMgr.Create(direction);
            }

        }
    }


    //ステージクリア処理
    public virtual void StageComplete()
    {
        StartCoroutine(StageCompleteCoroutine());
    }
    IEnumerator StageCompleteCoroutine()
    {
        SoundManager.instance.StopBGM(BGM_Name);
        SoundManager.instance.PlaySE("Complete");
        Debug.Log("ステージクリア");
        Debug.Log(Score);

        //カメラの位置を初期位置にする
        MainCamera.transform.position = new Vector3(0.0f, 0.0f, -11.0f);
        IsStageComplete = true;
        DrawLine.DrawLine();

        foreach (MissionScript i in Missions)
        {
            //リザルトにミッション項目追加
            ResultScript result = UIManager.Result.GetComponent<ResultScript>();
            result.AddMissionResult(i.IsMissionComplete(), i.Type);
        }

        //線の描画完了を待つ
        while (!DrawLine.FinishDraw())
        {
            yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForSeconds(0);

        UIManager.DisplayResult();
        Debug.Log("ステージクリア処理終了");
    }

    //ゲームオーバー処理
    void GameOver()
    {
        SoundManager.instance.StopBGM("BGM1");
        SoundManager.instance.PlaySE("GameOver");
        Time.timeScale = 0;
        UIManager.DisplayGameOver();
        Debug.Log("GameOver");
    }


    //障害物と衝突した時
    public void CollisionObstacle()
    {
        ObstacleCollisionNumber++;
        Debug.Log("衝突");
    }

    //障害物を破壊した時
    public void DestroyObstacle()
    {
        ObstacleDestroyNumber++;
        Debug.Log("破壊");
    }

}
