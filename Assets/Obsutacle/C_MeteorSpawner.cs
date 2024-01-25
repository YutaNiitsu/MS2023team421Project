using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_MeteorSpawner : MonoBehaviour
{
    [SerializeField]
    GameObject meteorPrefab = null;
    [SerializeField, Min(5.0f)]
    float defaultMinWaitTime = 5;
    [SerializeField, Min(5.0f)]
    float defaultMaxWaitTime = 5;
    [SerializeField]
    Vector2 defaultMinSize = Vector2.one;
    [SerializeField]
    Vector2 defaultMaxSize = Vector2.one;
    bool isSpawning = false;
    float minWaitTime;
    float maxWaitTime;
    Vector2 minSize;
    Vector2 maxSize;
    Coroutine timer;
    //外部から値を代入するためのプロパティ
    public float MinWaitTime
    {
        set
        {
            //あまりにも小さい値になるとものすごい数の障害物が生成されてしまうので、0.1未満にならないようにする
            minWaitTime = Mathf.Max(value, 5.0f);
        }
        get
        {
            return minWaitTime;
        }
    }
    public float MaxWaitTime
    {
        set
        {
            maxWaitTime = Mathf.Max(value, 5.0f);
        }
        get
        {
            return maxWaitTime;
        }
    }
    public bool IsActive { get; set; } = true;
    void Start()
    {
        InitSpawner();
    }
    void Update()
    {
        if (!IsActive)
        {
            //生成中なら中断する
            if (timer != null)
            {
                StopCoroutine(timer);
                isSpawning = false;
            }
            return;
        }
        //生成中じゃないなら生成開始
        if (!isSpawning)
        {
            timer = StartCoroutine(nameof(SpawnTimer));
        }
    }
    //初期化用メソッド
    public void InitSpawner()
    {
        minWaitTime = defaultMinWaitTime;
        maxWaitTime = defaultMaxWaitTime;
        minSize = defaultMinSize;
        maxSize = defaultMaxSize;
    }
    //生成処理を行うコルーチン
    IEnumerator SpawnTimer()
    {
        isSpawning = true;

        GameObject meteorObj = Instantiate(meteorPrefab, transform.position, Quaternion.identity);
        C_Meteor meteor = meteorObj.GetComponent<C_Meteor>();
        float sizeX = Random.Range(minSize.x, maxSize.x);
        float sizeY = Random.Range(minSize.y, maxSize.y);
        meteor.SetWall(new Vector2(sizeX, sizeY));
        float waitTime = Random.Range(minWaitTime, maxWaitTime);
        yield return new WaitForSeconds(waitTime);
        isSpawning = false;
    }
}
