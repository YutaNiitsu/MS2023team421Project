using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.GraphicsBuffer;

public class CreateConstellationScript : MonoBehaviour
{
    // �͂ߍ��ތ^�̃v���n�u
    public GameObject TargetPrefab;
    // �����Ȃ����̃v���n�u
    public LineRenderer LineRenderer;
    // �ݒu���ꂽ�͂ߍ��ތ^
    private GameObject[] Targets;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // �{�^�������ꂽ���͎��s���Ȃ�
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }
            //�}�E�X���W�̎擾
            Vector3 mousePos = Input.mousePosition;
            //�X�N���[�����W�����[���h���W�ɕϊ�
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 10f));
            PutTarget(worldPos);
        }
    }

    //�͂ߍ��ތ^��ݒu����
    public void PutTarget(Vector3 pos)
    {
        Instantiate(TargetPrefab, pos, Quaternion.identity);
    }
}
