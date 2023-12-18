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
                // ���X�e�B�b�N�̐��������̓��͂��擾
                float horizontalInput = Input.GetAxis("Debug Horizontal");

                // ���X�e�B�b�N�̐��������̓��͂��擾
                float verticalInput = Input.GetAxis("Debug Vertical");

                // ���͂Ɋ�Â��Ĉړ��������v�Z
                Vector3 movementDirection = new Vector3(horizontalInput, verticalInput, 0.0f).normalized;

                // �ړ������ɑ��x���|���Ĉړ�
                transform.Translate(movementDirection * 0.05f);
            }
        }
        
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
}
