using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
// �Q�[���S�̂𐧌䂷��
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
            // �I�u�W�F�N�g��z�u
            ProceduralGenerator.Generate(ConstellationDatas[index].constellations, new Vector2(500, 500), 0.9f);
        }
       
    }

    // Update is called once per frame
    void Update()
    {
       
    }
}
