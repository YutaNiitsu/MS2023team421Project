using System;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;

// 星座を保存するためのクラス
public class ConstellationSaveManager : MonoBehaviour
{
    //シングルトン
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
    // 星座のデータを保存
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

        // バイナリ形式でシリアル化
        BinaryFormatter bf = new BinaryFormatter();
        // 指定したパスにファイルを作成
        FileStream file = File.Create(SaveFilePath);
        // Closeが確実に呼ばれるように例外処理を用いる
        try
        {
            string dataJSON = JsonUtility.ToJson(constellationData_ConvArray, true);
        
            // 指定したオブジェクトを上で作成したストリームにシリアル化する
            bf.Serialize(file, dataJSON);
        }
        finally
        {
            // ファイル操作には明示的な破棄が必要です。Closeを忘れないように。
            if (file != null)
                file.Close();
        }
    }
}
