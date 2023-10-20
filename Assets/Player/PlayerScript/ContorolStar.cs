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

        // �}�E�X���������n�_�̍��W���L�^
        if (Input.GetMouseButtonDown(0))
        {
            this.startPos = Input.mousePosition;
            shotGaugeSet = true;
        }

        // �}�E�X�𗣂����n�_�̍��W����A���˕������v�Z
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

        ////�ړ�
        //this.rigid2d.velocity = (startDirection * speed);
        ////����
        FixedUpdate();

        // �}�E�X��������Ă���� �V���b�g�Q�[�W���Ă�
        if (shotGaugeSet)
        {
            shotGaugeValue();
        }

        // �e�X�g�p�F�X�y�[�X�L�[�����Œ�~
        if (Input.GetKeyDown(KeyCode.Space))
        {
            this.rigid2d.velocity *= 0;
        }

    }

    void FixedUpdate()
    {
        this.rigid2d.velocity *= 0.995f;

        // �|�C���g
        // Update�̒��Œl����Ɏ擾���邱�ƁB
        direction = rigid2d.velocity;
    }

    // �V���b�g�Q�[�W�֐�
    void shotGaugeValue()
    {
        Debug.Log("�Ăяo���m�F");
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
        //// direction�̍X�V
        //direction = rb.velocity;
    }

}


