using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class ContorolStar : MonoBehaviour
{
    //public PlayerStock PlayerStockScript;
    //public GameManagerScript UseGameManagerScript;
    private bool MoveTG;

    public Rigidbody2D rigid2d { get; protected set; }
    // Vector2 startPos;
    // Vector2 startDirection;

    // //public Slider shotGauge;
    // float speed = 4.5f;
    //// float gaugeLength = 0.0f;
    // bool shotGaugeSet = false;

    private StarScript _StarScript;

    Vector3 direction;
    Vector3 normal;

    void Start()
    {
        this.rigid2d = GetComponent<Rigidbody2D>();
        MoveTG = false;
        _StarScript = GetComponent<StarScript>();
    }

    void Update()
    {
        //UseGameManagerScript.Discharge(rigid2d);
        FixedUpdate();
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
        //UseGameManagerScript.Discharge(rigid2d);
        if (other.gameObject.CompareTag("Target"))
        {
            //UseGameManagerScript.Discharge(rigid2d); 
            //PlayerStockScript.Sporn();
            //Destroy(this.gameObject);
        }
        //// directionの更新
        //direction = rb.velocity;
    }

    public void ChangeMoveTG()
    {
        if (MoveTG == false)
        {
            MoveTG = true;
            this.gameObject.tag = "StarStop";
        }
        else if (MoveTG == true)
        {
            MoveTG = false;
            this.gameObject.tag = "Star";
        }
    }

    public void AddForce(Vector2 UpdateForce)
    {
        this.rigid2d.AddForce(UpdateForce);
        _StarScript.PlayParticle();
    }

    public void StopMove(Vector2 UpdatePos)
    {
        transform.position = UpdatePos;

        this.rigid2d.velocity *= 0.0f;
        this.gameObject.tag = "StarStop";
    }
}


