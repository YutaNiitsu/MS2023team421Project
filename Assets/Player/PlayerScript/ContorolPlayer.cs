using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContorolPlayer : MonoBehaviour
{
    List<GameObject> clickedGameObjectjList = new List<GameObject>();

    GameObject clickedGameObject;
    public ContorolStar ContorolStarScript;
    // Start is called before the first frame update
    Rigidbody2D rigid2d;
    Vector2 startPos;
    Vector2 startDirection;

    //public Slider shotGauge;
    float speed = 6.5f;
    // float gaugeLength = 0.0f;
    float maxDistance = 0.0f;

    bool shotGaugeSet = false;

    Vector3 direction;
    Vector3 normal;
    Vector3 mousePos;

 
    void Start()
    {
        //maxDistance = GetComponent<GameObject>().transform.lossyScale.x / 2.0f;
        maxDistance = this.transform.localScale.x;
        
    }

    // Update is called once per frame
    void Update()
    {
        PlayerContorol();
      

        CursorContorol();
    }

    // �V���b�g�Q�[�W�֐�
    void shotGaugeValue()
    {
        //Debug.Log("�Ăяo���m�F");
    }

    void PlayerContorol()
    {
        // �}�E�X���������n�_�̍��W���L�^
        if (Input.GetMouseButtonDown(0))
        {
            this.startPos = Input.mousePosition;
            shotGaugeSet = true;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit2d
                = Physics2D.CircleCast((Vector2)ray.origin, maxDistance, (Vector2)ray.direction);

            if (hit2d)
            {
                clickedGameObject = hit2d.transform.gameObject;
                if (clickedGameObject.gameObject.CompareTag("Player")
                    || clickedGameObject.gameObject.CompareTag("Star"))
                    ContorolStarScript = clickedGameObject.GetComponent<ContorolStar>();
            }

            //Debug.Log(clickedGameObject);
        }

        // �}�E�X�𗣂����n�_�̍��W����A���˕������v�Z
        if (Input.GetMouseButtonUp(0))
        {
            Vector2 endPos = Input.mousePosition;
            //Vector2 aaaa = new Vector2(100, 100);
            //startDirection = -1 * (endPos - startPos).normalized;
            startDirection = -1 * (endPos - startPos);
            //this.rigid2d.AddForce(startDirection * speed);
            //ContorolStarScript.AddForce(startDirection * speed);
            //this.rigid2d.AddForce(aaaa);
            //this.rigid2d.velocity = (startDirection * speed);
            shotGaugeSet = false;
            Debug.Log(startDirection);

            if (ContorolStarScript != null)
            {
                ContorolStarScript.AddForce(startDirection * speed);
                GameManagerScript.instance.Discharge(ContorolStarScript.rigid2d);
                ContorolStarScript = null;
                
            }
        }

        ////�ړ�
        //this.rigid2d.velocity = (startDirection * speed);
        ////����
        //FixedUpdate();

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

    //void FixedUpdate()
    //{
    //    this.rigid2d.velocity *= 0.995f;

    //    // �|�C���g
    //    // Update�̒��Œl����Ɏ擾���邱�ƁB
    //    direction = rigid2d.velocity;
    //}

    void CursorContorol()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 10.0f;
        Vector3 objPos = Camera.main.ScreenToWorldPoint(mousePos);
        transform.position = new Vector2(objPos.x, objPos.y);
    }

    //void OnTriggerEnter2D(Collider2D other)
    //{
    //    if (other.gameObject.CompareTag("Player") 
    //        && Input.GetMouseButtonDown(0))
    //        //|| (other.gameObject.CompareTag("StarStop") 
    //        //&& Input.GetMouseButtonUp(0) == true))
    //    {
    //        //other.gameObject.ContorolStar.ChangeMoveTG();
    //        Debug.Log($"TriggerEnter2D");
    //        //ContorolStarScript = other.gameObject.GetComponent<ContorolStar>();
    //    }
    //}
}
