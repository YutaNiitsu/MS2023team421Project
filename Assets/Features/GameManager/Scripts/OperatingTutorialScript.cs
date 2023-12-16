using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OperatingTutorialScript : MonoBehaviour
{
    public GameObject StarPrefab;
    public GameObject TargetPrefab;
    private GameObject Star;
    private GameObject Target;
    private int State = 0;
    private GameObject Camera;
    private TutorialManagerScript TutorialManager;
    private Vector2 MoveDistance;
    private Vector3 PrePosition;
    private bool IsClickStar;
    private bool IsDischarged;
    private bool IsPutOnTareget;

    // Start is called before the first frame update
    void Start()
    {
        Camera = GameObject.FindGameObjectWithTag("MainCamera");
        TutorialManager = (TutorialManagerScript)GameManagerScript.instance.StageManager;
        
        TutorialManager.TutorialUI.SetAllDisable();
        PrePosition = Camera.transform.position;
        //関数ポインタ
        TutorialManager.ClickStarFunc = ClickStar;
        TutorialManager.DischargeFunc = Discharge;
        TutorialManager.PutOnTaregetFunc = PutOnTareget;
        IsClickStar = false;
        IsDischarged = false;
        IsPutOnTareget = false;
    }

    private void Update()
    {
        Vector3 dis;
        Vector3 pos;

        switch (State)
        {
            case 0:
                //移動
                TutorialManager.UIManager.SetUIActive(false, false, false, false, false, false);
                TutorialManager.TutorialUI.SetActiveOperatingTutorial(0);
                dis = PrePosition - Camera.transform.position;
                //合計移動距離計算
                MoveDistance += new Vector2(Mathf.Abs(dis.x), Mathf.Abs(dis.y));
                PrePosition = Camera.transform.position;

                if (MoveDistance.x > 10.0f && MoveDistance.y > 10.0f)
                {
                    State = 1;
                    MoveDistance = Vector2.zero;
                    PrePosition = Input.mousePosition;
                    TutorialManager.TutorialUI.SetActiveOperatingTutorial(1);
                    Debug.Log("移動完了");
                }
                break;
            case 1:
                //カーソル移動
                
                dis = PrePosition - Input.mousePosition;
                //合計移動距離計算
                MoveDistance += new Vector2(Mathf.Abs(dis.x), Mathf.Abs(dis.y));
                PrePosition = Input.mousePosition;

                if (MoveDistance.x > 100.0f && MoveDistance.y > 100.0f)
                {
                    State = 2;
                    //星生成
                    pos = Camera.transform.position;
                    pos.z = 0.0f;
                    Star = Instantiate(StarPrefab, pos, new Quaternion());
                    TutorialManager.TutorialUI.SetActiveOperatingTutorial(2);

                    Debug.Log("移動完了");
                }
                break;
            case 2:
                //星をクリック
               
                if (IsClickStar)
                {
                    MoveDistance = Vector2.zero;
                    PrePosition = Input.mousePosition;
                    State = 3;
                    TutorialManager.TutorialUI.SetActiveOperatingTutorial(3);
                }
                break;
            case 3:
                //発射方向決める
                
                dis = PrePosition - Input.mousePosition;
                //合計移動距離計算
                MoveDistance += new Vector2(Mathf.Abs(dis.x), Mathf.Abs(dis.y));
                PrePosition = Input.mousePosition;

                if (MoveDistance.x > 0.0f && MoveDistance.y > 0.0f)
                {
                    State = 4;
                    MoveDistance = Vector2.zero;
                    PrePosition = Input.mousePosition;
                    TutorialManager.TutorialUI.SetActiveOperatingTutorial(4);
                    Debug.Log("移動完了");
                }
                break;
            case 4:
                //発射
                
                if (IsDischarged)
                {
                    State = 5;
                    //はめ込む型生成
                    pos = Star.transform.position - new Vector3(10.0f, 0.0f, 0.0f);
                    pos.z = 0.0f;
                    Target = Instantiate(TargetPrefab, pos, new Quaternion());
                    TutorialManager.TutorialUI.SetActiveOperatingTutorial(5);
                }
                break;
            case 5:
                //はめ込む型に星をはめ込む
               
               
                if(IsPutOnTareget)
                {
                    State = 6;
                    TutorialManager.UIManager.SetUIActive(true, true, true, false, false, false);
                    Destroy(Star);
                    Destroy(Target);
                    Camera.transform.position = new Vector3(0.0f, 0.0f, Camera.transform.position.z);
                    //星と障害物を配置
                    TutorialManager.CreateObjects();
                    TutorialManager.EnableAddScore = true;
                    TutorialManager.TutorialUI.SetActiveOperatingTutorial(6);
                }
                break;
            case 6:
                
                if (IsDischarged)
                {
                    State = 7;
                    TutorialManager.TutorialUI.SetActiveOperatingTutorial(7);
                }
                break;
            case 7:
                
                break;
            default:
                break;
        }

    }

    private void ClickStar()
    {
        IsClickStar = true;
    }
    private void Discharge()
    {
        IsDischarged = true;
    }
    private void PutOnTareget()
    {
        IsPutOnTareget = true;
    }
}
