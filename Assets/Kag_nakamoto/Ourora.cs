using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using static SaveConstellationDIsplayScript;

public class Ourora : MonoBehaviour
{
    //List<GameObject> clickedGameObjectjList = new List<GameObject>();

    //public float sensitivity = 1.0f; // スティックの感度

    //GameObject clickedGameObject;
    //public ContorolStar ContorolStarScript;
    //// Start is called before the first frame update
    //Rigidbody2D rigid2d;
    //Vector2 startPos;
    //Vector2 startDirection;

    ////public Slider shotGauge;
    //float speed = 300.0f;
    //// float gaugeLength = 0.0f;
    //float maxDistance = 0.0f;

    //bool shotGaugeSet = false;

    //Vector3 direction;
    //Vector3 normal;
    //Vector3 mousePos;

    //string[] joystickNames;
    //bool ContorolerTG;
    //new Renderer collider;
    //Vector3 objectSize;

    //Vector3 GetPosition;
    //private StarScript StarScriptRef;
    //public Vector3 CursorPosition { get; protected set; }

    //[DllImport("user32.dll")]
    //static extern bool SetCursorPos(int X, int Y);

    void Start()
    {
      
    }
    private void FixedUpdate()
    {

    }
    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale == 1 && GameManagerScript.instance.StageManager.IsFinished)
        {
                gameObject.SetActive(false);
        }

    }

    //// ショットゲージ関数
    //void shotGaugeValue()
    //{
    //    //Debug.Log("呼び出し確認");
    //}

    //void PlayerContorol()
    //{
    //    //if (ContorolerTG)
    //    //{

    //    //}
    //    //else
    //    //{
    //    //    transform.position = Input.mousePosition;
    //    //}

    //    //ターン数０になったら実行しない
    //    if (GameManagerScript.instance.StageManager.DischargeNumber <= 0)
    //        return;

    //    // マウスを押した地点の座標を記録
    //    if (Input.GetMouseButtonDown(0) || Input.GetButtonDown("Fire1"))
    //    {
    //        //Debug.Log("SSSSdddddd呼び出し確認");
    //        //Vector3 CursurPoint = Camera.main.ScreenToWorldPoint(transform.position);
    //        //this.startPos = CursurPoint;
    //        this.startPos = transform.position;
    //        //this.startPos = Input.mousePosition;
    //        shotGaugeSet = true;
    //        // Debug.Log($"ddddd呼び出し確認{transform.position}");
    //        //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    //        Vector3 origin = transform.position; // 原点
    //        Vector3 direction = new Vector3(0.5f, 0, 0); // X軸方向を表すベクトル。面倒だからScaleの値を定数で入れてる
    //        Ray ray = new Ray(origin, direction); // Rayを生成;
    //        RaycastHit2D hit2d
    //            = Physics2D.CircleCast((Vector2)ray.origin, maxDistance, (Vector2)ray.direction);

    //        if (hit2d)
    //        {
    //            clickedGameObject = hit2d.transform.gameObject;
    //            if (clickedGameObject.gameObject.CompareTag("Player")
    //                || clickedGameObject.gameObject.CompareTag("Star"))
    //            {
    //                ContorolStarScript = clickedGameObject.GetComponent<ContorolStar>();
    //                StarScriptRef = clickedGameObject.GetComponent<StarScript>();
    //            }

    //            GameManagerScript.instance.StageManager.ClickStar();
    //        }

    //        //Debug.Log(clickedGameObject);
    //    }
    //    if (StarScriptRef != null)
    //        StarScriptRef.Charging(startDirection.magnitude);

    //    Vector2 endPos = transform.position;
    //    startDirection = -1 * (endPos - startPos);

    //    // マウスを離した地点の座標から、発射方向を計算
    //    if (Input.GetMouseButtonUp(0) || Input.GetButtonUp("Fire1"))
    //    {
    //        Vector3 CursurPoint = Camera.main.ScreenToWorldPoint(transform.position);

    //        ////Vector2 endPos = CursurPoint;
    //        //Vector2 endPos = transform.position;
    //        ////Vector2 endPos = Input.mousePosition;
    //        //startDirection = -1 * (endPos - startPos);

    //        shotGaugeSet = false;



    //        Debug.Log(startDirection);
    //        //Debug.Log("UUUUUUdddddd呼び出し確認");
    //        if (ContorolStarScript != null)
    //        {
    //            //Debug.Log("Adddddd呼び出し確認");
    //            ContorolStarScript.AddForce(startDirection * speed);
    //            GameManagerScript.instance.StageManager.Discharge(ContorolStarScript.rigid2d);
    //            ContorolStarScript = null;
    //            StarScriptRef = null;
    //        }
    //    }

    //    ////移動
    //    //this.rigid2d.velocity = (startDirection * speed);
    //    ////減速
    //    //FixedUpdate();

    //    // マウスが押されている間 ショットゲージを呼ぶ
    //    if (shotGaugeSet)
    //    {
    //        shotGaugeValue();
    //    }

    //    // テスト用：スペースキー押下で停止
    //    if (Input.GetKeyDown(KeyCode.Space))
    //    {
    //        this.rigid2d.velocity *= 0;
    //    }
    //}

    ////void FixedUpdate()
    ////{
    ////    this.rigid2d.velocity *= 0.995f;

    ////    // ポイント
    ////    // Updateの中で値を常に取得すること。
    ////    direction = rigid2d.velocity;
    ////}

    //void CursorContorol()
    //{
    //    //if (ContorolerTG)
    //    //{
    //    //    // 左スティックの水平方向の入力を取得
    //    //    float horizontalInput = Input.GetAxis("Horizontal");
    //    //    //Input.GetAxis("Debug Horizontal");
    //    //    // 左スティックの垂直方向の入力を取得
    //    //    float verticalInput = Input.GetAxis("Vertical");
    //    //    // 入力に基づいて移動方向を計算
    //    //    Vector3 movementDirection = new Vector3(horizontalInput, verticalInput, 0.0f).normalized;

    //    //    // 移動方向に速度を掛けて移動
    //    //    transform.Translate(movementDirection * 0.05f);

    //    //    //// スティックの傾きを計算
    //    //    //float stickAngle = Mathf.Atan2(verticalInput, horizontalInput) * Mathf.Rad2Deg;

    //    //    //// スティックの感度を適用
    //    //    //stickAngle *= sensitivity;

    //    //    //// 結果を表示（デバッグ用）
    //    //    //Debug.Log("Stick Angle: " + stickAngle);
    //    //}
    //    //////////////////////////////////////////////////////////////////////////////////
    //    //else
    //    //{
    //    //    Vector3 mousePos = Input.mousePosition;
    //    //    mousePos.z = 10.0f;
    //    //    Vector3 objPos = Camera.main.ScreenToWorldPoint(mousePos);
    //    //    transform.position = new Vector2(objPos.x, objPos.y);
    //    //}

    //    MoveInCamera();

    //}

    //bool isContoroler()
    //{
    //    joystickNames = Input.GetJoystickNames();

    //    // ジョイスティックが1つ以上接続されているかを判定
    //    if (joystickNames.Length > 0 && !string.IsNullOrEmpty(joystickNames[0]))
    //    {
    //        //Debug.Log("ジョイスティックが接続されています。");
    //        return true;
    //    }
    //    else
    //    {
    //        //Debug.Log("ジョイスティックが接続されていません。");
    //        return false;
    //    }
    //}

    //void MoveInCamera()
    //{
    //    //Vector3 pointLB = Camera.main.ScreenToWorldPoint(Vector3.zero);
    //    //Vector3 pointRU = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width - 1, Screen.height - 1, 0));
    //    //// 画面外判定
    //    //{

    //    //    // 画像サイズの半分の値
    //    //    float x = objectSize.x;
    //    //    float y = objectSize.y;

    //    //    // 左
    //    //    if ((transform.position.x - x) < pointLB.x)
    //    //    {
    //    //        transform.position = new Vector2(pointLB.x + x, transform.position.y);
    //    //    }
    //    //    // 下
    //    //    if ((transform.position.y - y) < pointLB.y)
    //    //    {
    //    //        transform.position = new Vector2(transform.position.x, pointLB.y + y);
    //    //    }
    //    //    // 右
    //    //    if ((transform.position.x + x) > pointRU.x)
    //    //    {
    //    //        transform.position = new Vector2(pointRU.x - x, transform.position.y);
    //    //    }
    //    //    // 上
    //    //    if ((transform.position.y + y) > pointRU.y)
    //    //    {
    //    //        transform.position = new Vector2(transform.position.x, pointRU.y - y);
    //    //    }
    //    //}

    //    if (ContorolerTG)
    //    {
    //        float moveX = Input.GetAxis("RS_X") * 5.0f * sensitivity + Input.GetAxis("Mouse X") * 10.0f;
    //        float moveY = Input.GetAxis("RS_Y") * 5.0f * sensitivity - Input.GetAxis("Mouse Y") * 10.0f;
    //        CursorPosition += new Vector3(moveX, moveY, 0.0f);
    //    }
    //    else
    //    {
    //        float moveX = Input.GetAxis("Mouse X");
    //        float moveY = -Input.GetAxis("Mouse Y");
    //        CursorPosition += new Vector3(moveX, moveY, 0.0f) * 10.0f;

    //    }

    //    if (CursorPosition.x <= 0.0f)
    //        CursorPosition = new Vector3(0.0f, CursorPosition.y, 0.0f);
    //    if (CursorPosition.x >= Screen.width)
    //        CursorPosition = new Vector3(Screen.width, CursorPosition.y, 0.0f);
    //    if (CursorPosition.y <= 0.0f)
    //        CursorPosition = new Vector3(CursorPosition.x, 0.0f, 0.0f);
    //    if (CursorPosition.y >= Screen.height)
    //        CursorPosition = new Vector3(CursorPosition.x, Screen.height, 0.0f);
    //    //Debug.Log(CursorPosition);
    //    SetCursorPos((int)CursorPosition.x, (int)CursorPosition.y);
    //    Vector3 mousePos = Input.mousePosition;
    //    mousePos.z = 10.0f;
    //    Vector3 objPos = Camera.main.ScreenToWorldPoint(mousePos);
    //    transform.position = new Vector2(objPos.x, objPos.y);
    //}

    //public Vector3 GetPos()
    //{
    //    return GetPosition;
    //}
}

