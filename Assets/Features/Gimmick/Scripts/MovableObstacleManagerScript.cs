using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngineInternal;

public class MovableObstacleManagerScript : MonoBehaviour
{
    [Header("動く障害物のプレハブ")]
    public GameObject MovableObstacle;
    [Header("動く障害物のスピード")]
    public float Speed;
    [Header("動く障害物の数")]
    public int Number;
    [Header("動く障害物の群体の大きさ")]
    public float Size;
    [Header("警告表示する時間")]
    public float WarningTime;
    [Header("警告表示点滅間隔")]
    public float Interval;

    private GameObject MainCamera;
    private Vector3[] SpawnPos;   //出現位置の候補
    private WarningMarkScript WarningMark;
    private Coroutine _Coroutine;
    // Start is called before the first frame update
    void Start()
    {
        MainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        SpawnPos = new Vector3[8];
        SpawnPos[0] = new Vector3(1.0f, 0.0f, 0.0f);
        SpawnPos[1] = new Vector3(1.0f, 1.0f, 0.0f);
        SpawnPos[2] = new Vector3(0.0f, 1.0f, 0.0f);
        SpawnPos[3] = new Vector3(-1.0f, 1.0f, 0.0f);
        SpawnPos[4] = new Vector3(-1.0f, 0.0f, 0.0f);
        SpawnPos[5] = new Vector3(-1.0f, -1.0f, 0.0f);
        SpawnPos[6] = new Vector3(0.0f, -1.0f, 0.0f);
        SpawnPos[7] = new Vector3(1.0f, -1.0f, 0.0f);
        WarningMark = GameObject.Find("WarningImage").GetComponent<WarningMarkScript>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //障害物生成
    //8方向から選択
    public void Create(int dir)
    {
        if (dir < 0 || dir > 7)
            return;

        if (_Coroutine != null) StopCoroutine(_Coroutine);
        _Coroutine = StartCoroutine(CreateCoroutine(dir));
       
    }

    IEnumerator CreateCoroutine(int dir)
    {
        Vector3 screenSize = new Vector3(Screen.currentResolution.width, Screen.currentResolution.height, 0.0f);

        //生成位置
        Vector3 popPos = Vector3.Scale(SpawnPos[dir], screenSize) * 0.45f;
       
        //警告表示
        WarningMark.StartWarning(popPos, Interval);
        yield return new WaitForSeconds(WarningTime);
        WarningMark.StopWarning();

        Vector3 cameraPos = MainCamera.transform.position;
        cameraPos.z = 0.0f;
        ////スクリーンのサイズをワールド内のサイズに変換
        //Vector3 worldScreen = Camera.main.ScreenToWorldPoint(screenSize);
        //生成
        popPos = Vector3.Scale(SpawnPos[dir], screenSize * 0.01f) + cameraPos;
        //移動方向
        Vector3 vel = -SpawnPos[dir] * Speed;

        for (int i = 0; i < Number; i++)
        {
            float x = UnityEngine.Random.Range(-1.0f, 1.0f);
            float y = UnityEngine.Random.Range(-1.0f, 1.0f);
            popPos += new Vector3(x, y, 0.0f) * Size;
            GameObject obj = Instantiate(MovableObstacle, popPos, new Quaternion());
            obj.GetComponent<Rigidbody2D>().velocity = vel;
        }
    }
}
