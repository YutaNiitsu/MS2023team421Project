using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
// ゲーム全体を制御する
public class GameManagerScript : MonoBehaviour
{
    private ProceduralGenerator ProceduralGenerator;
    private SaveConstellationData[] ConstellationDatas;
    
    // Start is called before the first frame update
    void Start()
    {
        ProceduralGenerator = GetComponent<ProceduralGenerator>();
       

        ConstellationDatas = GetComponent<ConstellationLoadManager>().LoadData();
        if (ConstellationDatas.Length > 0)
        {
            int index = UnityEngine.Random.Range(0, ConstellationDatas.Length);
            // オブジェクトを配置
            ProceduralGenerator.Generate(ConstellationDatas[index].constellations, new Vector2(500, 500), 0.9f);
        }
       
    }

    // Update is called once per frame
    void Update()
    {
       
    }
}
