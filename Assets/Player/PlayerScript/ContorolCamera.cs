using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContorolCamera : MonoBehaviour
{
    string[] joystickNames;
    bool ContorolerTG;
    // Start is called before the first frame update
    void Start()
    {
        ContorolerTG = false;
    }

    // Update is called once per frame
    void Update()
    {
        ContorolerTG = isContoroler();
        Input.GetAxis("Debug Horizontal");
        if (Time.timeScale == 1 && !GameManagerScript.instance.StageManager.IsFinished)
        {
            if (!ContorolerTG)
            {
                if (Input.GetKey("w"))
                {
                    this.transform.position += new Vector3(0.0f, 0.05f, 0.0f);
                }
                else if (Input.GetKey("s"))
                {
                    this.transform.position += new Vector3(0.0f, -0.05f, 0.0f);
                }
                if (Input.GetKey("a"))
                {
                    this.transform.position += new Vector3(-0.05f, 0.0f, 0.0f);
                }
                else if (Input.GetKey("d"))
                {
                    this.transform.position += new Vector3(0.05f, 0.0f, 0.0f);
                }
            }
            else if (ContorolerTG)
            {
                // 左スティックの水平方向の入力を取得
                float horizontalInput = Input.GetAxis("Debug Horizontal");

                // 左スティックの垂直方向の入力を取得
                float verticalInput = Input.GetAxis("Debug Vertical");

                // 入力に基づいて移動方向を計算
                Vector3 movementDirection = new Vector3(horizontalInput, verticalInput, 0.0f).normalized;

                // 移動方向に速度を掛けて移動
                transform.Translate(movementDirection * 0.05f);
            }
        }
        
    }

    bool isContoroler()
    {
        joystickNames = Input.GetJoystickNames();

        // ジョイスティックが1つ以上接続されているかを判定
        if (joystickNames.Length > 0 && !string.IsNullOrEmpty(joystickNames[0]))
        {
            //Debug.Log("ジョイスティックが接続されています。");
            return true;
        }
        else
        {
            //Debug.Log("ジョイスティックが接続されていません。");
            return false;
        }
    }
}
