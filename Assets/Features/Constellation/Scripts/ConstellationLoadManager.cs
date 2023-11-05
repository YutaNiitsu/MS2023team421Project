using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class ConstellationLoadManager : MonoBehaviour
{
    //シングルトン
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
    // 星座のデータを読み込み
    public SaveConstellationData[] LoadData()
    {
        SaveConstellationData[] data = null;
        string dataJSON;
        if (File.Exists(SaveFilePath))
        {
            // バイナリ形式でデシリアライズ
            BinaryFormatter bf = new BinaryFormatter();
            // 指定したパスのファイルストリームを開く
            FileStream file = File.Open(SaveFilePath, FileMode.Open);
            try
            {
                // 指定したファイルストリームをオブジェクトにデシリアライズ。
                dataJSON = (string)bf.Deserialize(file);
            }
            finally
            {
                // ファイル操作には明示的な破棄が必要です。Closeを忘れないように。
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
