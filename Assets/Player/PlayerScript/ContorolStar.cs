using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class ContorolStar : MonoBehaviour
{
    public PlayerStock PlayerStockScript;

    Rigidbody2D rigid2d;
    Vector2 startPos;
    Vector2 startDirection;

    //public Slider shotGauge;
    float speed = 4.5f;
   // float gaugeLength = 0.0f;
    bool shotGaugeSet = false;

    Vector3 direction;
    Vector3 normal;

    void Start()
    {
        this.rigid2d = GetComponent<Rigidbody2D>();

    }

    void Update()
    {

        // マウスを押した地点の座標を記録
        if (Input.GetMouseButtonDown(0))
        {
            this.startPos = Input.mousePosition;
            shotGaugeSet = true;
        }

        // マウスを離した地点の座標から、発射方向を計算
        if (Input.GetMouseButtonUp(0))
        {
            Vector2 endPos = Input.mousePosition;
            //Vector2 aaaa = new Vector2(100, 100);
            //startDirection = -1 * (endPos - startPos).normalized;
            startDirection = -1 * (endPos - startPos);
            this.rigid2d.AddForce(startDirection * speed);
            //this.rigid2d.AddForce(aaaa);
            //this.rigid2d.velocity = (startDirection * speed);
            shotGaugeSet = false;
            Debug.Log(startDirection);
        }

        ////移動
        //this.rigid2d.velocity = (startDirection * speed);
        ////減速
        FixedUpdate();

        // マウスが押されている間 ショットゲージを呼ぶ
        if (shotGaugeSet)
        {
            shotGaugeValue();
        }

        // テスト用：スペースキー押下で停止
        if (Input.GetKeyDown(KeyCode.Space))
        {
            this.rigid2d.velocity *= 0;
        }

    }

    void FixedUpdate()
    {
        this.rigid2d.velocity *= 0.995f;

        // ポイント
        // Updateの中で値を常に取得すること。
        direction = rigid2d.velocity;
    }

    // ショットゲージ関数
    void shotGaugeValue()
    {
        Debug.Log("呼び出し確認");
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        //Vector2 ColPos;
        //ColPos = other.gameObject.transform.position;
        //ColDirection(ColPos);
        //this.rigid2d.velocity *= -1.0f;
        ///
        normal = other.contacts[0].normal;

        Vector3 result = Vector3.Reflect(direction, normal);

        rigid2d.velocity = result;

        if (other.gameObject.CompareTag("Point"))
        {
            PlayerStockScript.Sporn();
            Destroy(this.gameObject);
        }
        //// directionの更新
        //direction = rb.velocity;
    }

}


