using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContorolPlayer : MonoBehaviour
{
    List<GameObject> clickedGameObjectjList = new List<GameObject>();

    public float sensitivity = 1.0f; // �X�e�B�b�N�̊��x

    GameObject clickedGameObject;
    public ContorolStar ContorolStarScript;
    // Start is called before the first frame update
    Rigidbody2D rigid2d;
    Vector2 startPos;
    Vector2 startDirection;

    //public Slider shotGauge;
    float speed = 300.0f;
    // float gaugeLength = 0.0f;
    float maxDistance = 0.0f;

    bool shotGaugeSet = false;

    Vector3 direction;
    Vector3 normal;
    Vector3 mousePos;

    string[] joystickNames;
    bool ContorolerTG;
    new Renderer collider;
    Vector3 objectSize;

    Vector3 GetPosition;
    void Start()
    {
        //maxDistance = GetComponent<GameObject>().transform.lossyScale.x / 2.0f;
        maxDistance = this.transform.localScale.x;
        ContorolerTG = false;

        // Collider �R���|�[�l���g���擾
        collider = GetComponent<Renderer>();

        // �I�u�W�F�N�g�̃T�C�Y���擾
        objectSize = collider.bounds.size;
    }

    // Update is called once per frame
    void Update()
    {
        //PlayerContorol();
        GetPosition = transform.position;
        ContorolerTG = isContoroler();

        CursorContorol();

        PlayerContorol();
    }

    // �V���b�g�Q�[�W�֐�
    void shotGaugeValue()
    {
        //Debug.Log("�Ăяo���m�F");
    }

    void PlayerContorol()
    {
        //if (ContorolerTG)
        //{

        //}
        //else
        //{
        //    transform.position = Input.mousePosition;
        //}
            // �}�E�X���������n�_�̍��W���L�^
            if (Input.GetMouseButtonDown(0) || Input.GetButtonDown("Fire1"))
        {
            //Debug.Log("SSSSdddddd�Ăяo���m�F");
            //Vector3 CursurPoint = Camera.main.ScreenToWorldPoint(transform.position);
            //this.startPos = CursurPoint;
            this.startPos = transform.position;
            //this.startPos = Input.mousePosition;
            shotGaugeSet = true;
           // Debug.Log($"ddddd�Ăяo���m�F{transform.position}");
            //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Vector3 origin = transform.position; // ���_
            Vector3 direction = new Vector3(0.5f, 0, 0); // X��������\���x�N�g���B�ʓ|������Scale�̒l��萔�œ���Ă�
            Ray ray = new Ray(origin, direction); // Ray�𐶐�;
            RaycastHit2D hit2d
                = Physics2D.CircleCast((Vector2)ray.origin, maxDistance, (Vector2)ray.direction);

            if (hit2d)
            {
                clickedGameObject = hit2d.transform.gameObject;
                if (clickedGameObject.gameObject.CompareTag("Player")
                    || clickedGameObject.gameObject.CompareTag("Star"))
                    ContorolStarScript = clickedGameObject.GetComponent<ContorolStar>();
                GameManagerScript.instance.StageManager.ClickStar();
            }

            //Debug.Log(clickedGameObject);
        }

        // �}�E�X�𗣂����n�_�̍��W����A���˕������v�Z
        if (Input.GetMouseButtonUp(0) || Input.GetButtonUp("Fire1"))
        {
            Vector3 CursurPoint = Camera.main.ScreenToWorldPoint(transform.position);
            
            //Vector2 endPos = CursurPoint;
            Vector2 endPos = transform.position;
            //Vector2 endPos = Input.mousePosition;
            startDirection = -1 * (endPos - startPos);
          
            shotGaugeSet = false;
            Debug.Log(startDirection);
            //Debug.Log("UUUUUUdddddd�Ăяo���m�F");
            if (ContorolStarScript != null)
            {
                //Debug.Log("Adddddd�Ăяo���m�F");
                ContorolStarScript.AddForce(startDirection * speed);
                GameManagerScript.instance.StageManager.Discharge(ContorolStarScript.rigid2d);
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
        if (ContorolerTG)
        {
            // ���X�e�B�b�N�̐��������̓��͂��擾
            float horizontalInput = Input.GetAxis("Horizontal");
            //Input.GetAxis("Debug Horizontal");
            // ���X�e�B�b�N�̐��������̓��͂��擾
            float verticalInput = Input.GetAxis("Vertical");
            // ���͂Ɋ�Â��Ĉړ��������v�Z
            Vector3 movementDirection = new Vector3(horizontalInput, verticalInput, 0.0f).normalized;

            // �ړ������ɑ��x���|���Ĉړ�
            transform.Translate(movementDirection * 0.05f);

            //// �X�e�B�b�N�̌X�����v�Z
            //float stickAngle = Mathf.Atan2(verticalInput, horizontalInput) * Mathf.Rad2Deg;

            //// �X�e�B�b�N�̊��x��K�p
            //stickAngle *= sensitivity;

            //// ���ʂ�\���i�f�o�b�O�p�j
            //Debug.Log("Stick Angle: " + stickAngle);
        }
        ////////////////////////////////////////////////////////////////////////////////
        else
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 10.0f;
            Vector3 objPos = Camera.main.ScreenToWorldPoint(mousePos);
            transform.position = new Vector2(objPos.x, objPos.y);
        }

        MoveInCamera();

    }

   bool isContoroler()
    {
        joystickNames = Input.GetJoystickNames();

        // �W���C�X�e�B�b�N��1�ȏ�ڑ�����Ă��邩�𔻒�
        if (joystickNames.Length > 0 && !string.IsNullOrEmpty(joystickNames[0]))
        {
            //Debug.Log("�W���C�X�e�B�b�N���ڑ�����Ă��܂��B");
            return true;
        }
        else
        {
            //Debug.Log("�W���C�X�e�B�b�N���ڑ�����Ă��܂���B");
            return false;
        }
    }

    void MoveInCamera()
    {
        Vector3 pointLB = Camera.main.ScreenToWorldPoint(Vector3.zero);
        Vector3 pointRU = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width - 1, Screen.height - 1, 0));
        // ��ʊO����
        {

            // �摜�T�C�Y�̔����̒l
            float x = objectSize.x;
            float y = objectSize.y;

            // ��
            if ((transform.position.x - x) < pointLB.x)
            {
                transform.position = new Vector2(pointLB.x + x, transform.position.y);
            }
            // ��
            if ((transform.position.y - y) < pointLB.y)
            {
                transform.position = new Vector2(transform.position.x, pointLB.y + y);
            }
            // �E
            if ((transform.position.x + x) > pointRU.x)
            {
                transform.position = new Vector2(pointRU.x - x, transform.position.y);
            }
            // ��
            if ((transform.position.y + y) > pointRU.y)
            {
                transform.position = new Vector2(transform.position.x, pointRU.y - y);
            }
        }
    }

    public Vector3 GetPos()
    {
        return GetPosition;
    }
}
