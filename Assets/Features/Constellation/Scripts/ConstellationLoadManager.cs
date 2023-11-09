using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class ConstellationLoadManager : MonoBehaviour
{
    //�V���O���g��
    public static ConstellationLoadManager instance;

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
    // �����̃f�[�^��ǂݍ���
    public SaveConstellationData[] LoadData()
    {
        SaveConstellationData[] data = null;
        string dataJSON;
        if (File.Exists(SaveFilePath))
        {
            // �o�C�i���`���Ńf�V���A���C�Y
            BinaryFormatter bf = new BinaryFormatter();
            // �w�肵���p�X�̃t�@�C���X�g���[�����J��
            FileStream file = File.Open(SaveFilePath, FileMode.Open);
            try
            {
                // �w�肵���t�@�C���X�g���[�����I�u�W�F�N�g�Ƀf�V���A���C�Y�B
                dataJSON = (string)bf.Deserialize(file);
            }
            finally
            {
                // �t�@�C������ɂ͖����I�Ȕj�����K�v�ł��BClose��Y��Ȃ��悤�ɁB
                if (file != null)
                    file.Close();
            }

            SaveConstellationData_ConvArray dataConvs = JsonUtility.FromJson<SaveConstellationData_ConvArray>(dataJSON);

            int index = 0;
            data = new SaveConstellationData[dataConvs.saveConstellationDatas.Length];
            foreach (SaveConstellationData_Conv i in dataConvs.saveConstellationDatas)
            {
                data[index] = i.Normalize();
                index++;
            }

        }
        else
        {
            Debug.Log("no load file");
        }

        if (data != null)
            return data;

        return null;
    }
}
