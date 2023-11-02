using System;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;

// ������ۑ����邽�߂̃N���X
public class ConstellationSaveManager : MonoBehaviour
{
    //�V���O���g��
    public static ConstellationSaveManager instance;

    private string SaveFilePath;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }


        SaveFilePath = "Assets/Features/Constellation/Scripts/SavedConstellation.save";
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }
    // �����̃f�[�^��ۑ�
    public void OnSaveNewData(SaveConstellationData[] data)
    {
        SaveConstellationData_ConvArray constellationData_ConvArray = new SaveConstellationData_ConvArray();
        constellationData_ConvArray.saveConstellationDatas = new SaveConstellationData_Conv[data.Length];
        int index = 0;
        foreach(SaveConstellationData i in data)
        {
            constellationData_ConvArray.saveConstellationDatas[index] = i.DataConv;
            index++;
        }

        // �o�C�i���`���ŃV���A����
        BinaryFormatter bf = new BinaryFormatter();
        // �w�肵���p�X�Ƀt�@�C�����쐬
        FileStream file = File.Create(SaveFilePath);
        // Close���m���ɌĂ΂��悤�ɗ�O������p����
        try
        {
            string dataJSON = JsonUtility.ToJson(constellationData_ConvArray, true);
        
            // �w�肵���I�u�W�F�N�g����ō쐬�����X�g���[���ɃV���A��������
            bf.Serialize(file, dataJSON);
        }
        finally
        {
            // �t�@�C������ɂ͖����I�Ȕj�����K�v�ł��BClose��Y��Ȃ��悤�ɁB
            if (file != null)
                file.Close();
        }
    }
}
