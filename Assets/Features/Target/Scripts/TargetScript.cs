using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetScript : MonoBehaviour
{
    private GameManagerScript GameManager;
    private bool Goal;
    // Start is called before the first frame update
    void Start()
    {
        Goal = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject obj = collision.gameObject;

        // 星だった時
        if (obj.CompareTag("Star"))
        {
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
            int rare = obj.GetComponent<StarScript>().Rarity;
            GameManager.AddScore(rare);

            Goal = true;
            // 自分を非表示にする
            gameObject.SetActive(false);
        }
    }

    public void Set(GameManagerScript gameManager)
    {
        GameManager = gameManager;
    }

    public bool IsGoal()
    {
        return Goal;
    }
}
