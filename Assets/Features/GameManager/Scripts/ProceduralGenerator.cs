using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;
// ゲーム開始時に星とそれをはめ込む場所を配置するシステム
public class ProceduralGenerator : MonoBehaviour
{
    public GameObject Target;
    public GameObject[] Star;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // オブジェクトを配置
    public void Generate()
    {
        // 星をはめ込む型生成
        Instantiate(Target, new Vector3(5.0f, 0.0f, 0.0f), Quaternion.identity);

        // 星を生成
        Instantiate(Star[0], new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
    }
}
