using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetScript : MonoBehaviour
{
    //星がすでにはまっているかどうか
    public bool Goaled { get; protected set; }
    //はまっている星を参照する
    public StarScript StarGoaled { get; protected set; }
    //特別ポイントに指定されているかどうか
    public bool IsSpecialPoint { get; protected set; }
    // Start is called before the first frame update
    void Start()
    {
        Goaled = false;
        StarGoaled = null;
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
                //衝突しても動かないようにする
                rb.bodyType = RigidbodyType2D.Static;
            }

            StarGoaled = obj.GetComponent<StarScript>();

            //スコア加算
            StarRarity rare = StarGoaled.Rarity;

            GameManagerScript.instance.AddScore(rare, IsSpecialPoint);
            
            // 自分を非表示にする
            gameObject.SetActive(false);
        }
    }

    //生成時の設定
    //isSpecialPoint : 特別ポイントにするかどうか
    public void Set(bool isSpecialPoint)
    {
        IsSpecialPoint = isSpecialPoint;
    }

    //特別ポイントに指定されている時、ユニーク以上のレアリティの星がはまっているかどうか
    public bool IsRareStarGoaledOnSpecialTarget()
    {
        if (IsSpecialPoint && Goaled && (int)StarGoaled.Rarity >= 2)
            return true;

        return false;
    }
}
