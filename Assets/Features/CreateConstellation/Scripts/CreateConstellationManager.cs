using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CreateConstellationManager : MonoBehaviour
{
    // �����f�[�^
    private SaveConstellationData[] ConstellationDatas;
    private ConstellationSaveManager constellationSaveManager;
    private CreateConstellationScript createConstellationScript;

    // Start is called before the first frame update
    void Start()
    {
        // �����f�[�^�̓ǂݍ���
        ConstellationDatas = GetComponent<ConstellationLoadManager>().LoadData();

        createConstellationScript = GetComponent<CreateConstellationScript>();
        constellationSaveManager = GetComponent<ConstellationSaveManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // ��ʏ�ɔz�u���ꂽ�{�^���������ꂽ���͎��s���Ȃ�
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }
            //�}�E�X���W�̎擾
            Vector3 mousePos = Input.mousePosition;
            //�X�N���[�����W�����[���h���W�ɕϊ�
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 10f));
            createConstellationScript.PutTarget(worldPos);
        }
    }

    // �����̃f�[�^��ۑ�
    public void OnSaveNewData()
    {
        SaveConstellationData[] data = new SaveConstellationData[1];
        data[0] = createConstellationScript.CreateSaveData();
        constellationSaveManager.OnSaveNewData(data);
    }
}
