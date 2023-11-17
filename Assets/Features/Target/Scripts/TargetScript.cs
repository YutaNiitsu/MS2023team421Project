using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetScript : MonoBehaviour
{
    private GameManagerScript GameManager;
    private bool Goal;
    //特別ポイントに指定されているかどうか
    private bool IsSpecialPoint;
    // Start is called before the first frame update
    void Start()
    {
        Goal = false;
    }

    private void Update()
    {
        //テスト用
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Goal = true;
            GameManager.AddScore(StarRarity.Normal, false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject obj = collision.gameObject;

        // 星だった時
        if (obj.CompareTag("Star"))
        {
            obj.tag = "Untagged";
            Goal = true;
            // 位置をターゲットに合わせる
            Vector3 pos = gameObject.transform.position;
            obj.transform.position = pos;

            Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                // 速度を０にする
                rb.velocity = Vector3.zero;
            }

            //スコア加算
            StarRarity rare = obj.GetComponent<StarScript>().Rarity;
            GameManager.AddScore(rare, IsSpecialPoint);

            
            // 自分を非表示にする
            gameObject.SetActive(false);
        }
    }

    //生成時の設定
    //gameManager : ゲームマネージャーを参照する
    //isSpecialPoint : 特別ポイントにするかどうか
    public void Set(GameManagerScript gameManager, bool isSpecialPoint)
    {
        GameManager = gameManager;
        IsSpecialPoint = isSpecialPoint;
    }

    public bool IsGoal()
    {
        return Goal;
    }
    public bool GetIsSpecialPoint()
    {
        return IsSpecialPoint;
    }
}
