using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateConstellationManager : MonoBehaviour
{
    // �����f�[�^
    private SaveConstellationData[] ConstellationDatas;
    private ConstellationSaveManager constellationSaveManager;

    // Start is called before the first frame update
    void Start()
    {
        // �����f�[�^�̓ǂݍ���
        ConstellationDatas = GetComponent<ConstellationLoadManager>().LoadData();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // �����̃f�[�^��ۑ�
    public void OnSaveNewData()
    {
        constellationSaveManager.OnSaveNewData(ConstellationDatas);
    }
}
