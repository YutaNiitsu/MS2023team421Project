using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.GraphicsBuffer;

public class CreateConstellationScript : MonoBehaviour
{
    // はめ込む型のプレハブ
    public GameObject TargetPrefab;
    // 星をつなぐ線のプレハブ
    public LineRenderer LineRenderer;
    // 設置されたはめ込む型
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
            // ボタン押された時は実行しない
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }
            //マウス座標の取得
            Vector3 mousePos = Input.mousePosition;
            //スクリーン座標をワールド座標に変換
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 10f));
            PutTarget(worldPos);
        }
    }

    //はめ込む型を設置する
    public void PutTarget(Vector3 pos)
    {
        Instantiate(TargetPrefab, pos, Quaternion.identity);
    }
}
