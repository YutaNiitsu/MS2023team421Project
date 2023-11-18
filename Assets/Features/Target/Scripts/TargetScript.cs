using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetScript : MonoBehaviour
{
    //星がすでにはまっているかどうか
    public bool Goaled { get; protected set; }
    //はまっている星がユニーク以上のレアリティかどうか
    public bool RareStarGoaled { get; protected set; }
    //特別ポイントに指定されているかどうか
    public bool IsSpecialPoint { get; protected set; }
    // Start is called before the first frame update
    void Start()
    {
        Goaled = false;
        RareStarGoaled = false;
    }

    private void Update()
    {
        //テスト用
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Goaled = true;
            GameManagerScript.instance.AddScore(StarRarity.Normal, false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject obj = collision.gameObject;

        // 星だった時
        if (obj.CompareTag("Star"))
        {
            obj.tag = "Untagged";
            Goaled = true;
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

            GameManagerScript.instance.AddScore(rare, IsSpecialPoint);
            
            if ((int)rare >= 2)
            {
                RareStarGoaled = true;
            }

            // 自分を非表示にする
            gameObject.SetActive(false);
        }
    }

    //生成時の設定
    //gameManager : ゲームマネージャーを参照する
    //isSpecialPoint : 特別ポイントにするかどうか
    public void Set(bool isSpecialPoint)
    {
        IsSpecialPoint = isSpecialPoint;
    }
}
