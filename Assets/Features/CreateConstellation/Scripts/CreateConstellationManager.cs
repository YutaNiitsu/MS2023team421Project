using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateConstellationManager : MonoBehaviour
{
    // 星座データ
    private SaveConstellationData[] ConstellationDatas;
    private ConstellationSaveManager constellationSaveManager;

    // Start is called before the first frame update
    void Start()
    {
        // 星座データの読み込み
        ConstellationDatas = GetComponent<ConstellationLoadManager>().LoadData();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // 星座のデータを保存
    public void OnSaveNewData()
    {
        constellationSaveManager.OnSaveNewData(ConstellationDatas);
    }
}
