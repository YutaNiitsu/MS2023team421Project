using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// ゲーム全体を制御する
public class GameManagerScript : MonoBehaviour
{
    ProceduralGenerator proceduralGenerator;
    // Start is called before the first frame update
    void Start()
    {
        proceduralGenerator = GetComponent<ProceduralGenerator>();
        // オブジェクトを配置
        proceduralGenerator.Generate();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
