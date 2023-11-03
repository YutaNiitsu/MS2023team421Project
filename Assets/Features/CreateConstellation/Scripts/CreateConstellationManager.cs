using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CreateConstellationManager : MonoBehaviour
{
    // 星座データ
    private SaveConstellationData[] ConstellationDatas;
    private ConstellationSaveManager constellationSaveManager;
    private CreateConstellationScript createConstellationScript;

    // Start is called before the first frame update
    void Start()
    {
        // 星座データの読み込み
        ConstellationDatas = GetComponent<ConstellationLoadManager>().LoadData();

        createConstellationScript = GetComponent<CreateConstellationScript>();
        constellationSaveManager = GetComponent<ConstellationSaveManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // 画面上に配置されたボタンが押された時は実行しない
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }
            //マウス座標の取得
            Vector3 mousePos = Input.mousePosition;
            //スクリーン座標をワールド座標に変換
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 10f));
            createConstellationScript.PutTarget(worldPos);
        }
    }

    // 星座のデータを保存
    public void OnSaveNewData()
    {
        SaveConstellationData[] data = new SaveConstellationData[1];
        data[0] = createConstellationScript.CreateSaveData();
        constellationSaveManager.OnSaveNewData(data);
    }
}
