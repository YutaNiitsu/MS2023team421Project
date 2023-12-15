using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class CreateArrow : MonoBehaviour
{
    public ContorolPlayer contorolPlayerScript;
    public GameObject arrowPrefab;
    public LineRenderer lineRenderer;
    private Vector3 previousPosiotion;
    private Vector3 clickPosition;
    private float minDistance = 0.1f;
    private int StatusTg = 0;
  
    //private InputAction MovePointer;
    //private PlayerInput playerInput;
    private void Start()
    {
        //playerInput = GetComponent<PlayerInput>();
       
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        previousPosiotion = transform.position;
        StatusTg = 0;
    }
    void Update()
    {
    
        //lineRenderer.SetPosition(0,new Vector3(0.0f,0.0f,1.0f));
        Debug.Log($"助けて{contorolPlayerScript.GetPos()}");
       // lineRenderer.SetPosition(1, new Vector3(10.0f, 10.0f, 1.0f));
       //Vector2 inputMoveAxis = MovePointer.ReadValue<Vector2>();
        if ((Input.GetMouseButtonDown(0) || Input.GetButtonDown("Fire1")) && StatusTg == 0) // 左クリック
        {
            lineRenderer.positionCount = 2;
            Vector3 Pos = contorolPlayerScript.GetPos();
            clickPosition = Pos;
            //clickPosition = Input.mousePosition;
            //clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //clickPosition = contorolPlayerScript.GetPos();

            clickPosition.z = 1.0f;

            // 生成した矢印のインスタンスを取得
            // GameObject arrow = Instantiate(arrowPrefab, clickPosition, Quaternion.identity);

            // LineRendererの初期位置を設定
            //lineRenderer.positionCount = 2;
            //lineRenderer.SetPosition(0, clickPosition);
            lineRenderer.SetPosition(0, new Vector3(clickPosition.x, clickPosition.y,1.0f));
            StatusTg = 1;
            Debug.Log("asasasas");
        }
       //lineRenderer.SetPosition(0, new Vector3(0.0f, 0.0f, 1.0f));
        if ((Input.GetMouseButton(0) || Input.GetButton("Fire1")) && StatusTg == 1) // マウスが押されている間
        {
            //Vector3 mousePosition = Input.mousePosition;
            //Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 Pos = contorolPlayerScript.GetPos();
            Vector3 mousePosition = Pos;
            //Vector3 mousePosition = contorolPlayerScript.GetPos();
            mousePosition.z = 1.0f;

            // LineRendererの終点を更新
            lineRenderer.SetPosition(0, new Vector3(clickPosition.x, clickPosition.y, 1.0f));
            lineRenderer.SetPosition(1, new Vector3(mousePosition.x, mousePosition.y, 1.0f));
            //Debug.Log(mousePosition);
        }

        if ((Input.GetMouseButtonUp(0) || Input.GetButtonUp("Fire1")) && StatusTg == 1) // マウスが離されたら
        {
            // LineRendererを非表示にする
            lineRenderer.positionCount = 0;
            StatusTg = 0;
        }
    }
}