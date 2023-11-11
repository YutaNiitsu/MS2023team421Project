using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;
using static UnityEditor.PlayerSettings;
// ゲーム開始時に星とそれをはめ込む場所を配置するシステム
public class ProceduralGenerator : MonoBehaviour
{
    [Header("星をはめ込む型のプレハブ")]
    public GameObject Target;
    [Header("星のプレハブ")]
    public GameObject[] Star;
    [Header("レア星のプレハブ")]
    public GameObject[] RareStarArea;
    private List<TargetScript> Targets;
    private GameManagerScript GameManager;
    // Start is called before the first frame update
    void Start()
    {
        GameManager = gameObject.GetComponent<GameManagerScript>();
    }

    // オブジェクトを配置
    //targets : はめ込む型のセーブデータ
    //range : 星の生成範囲
    //threshold : 星の密度が変わる
    public void GenerateTargets(ST_Constellation[] targets)
    {
        Targets = new List<TargetScript>();
        // 星をはめ込む型生成
        foreach (ST_Constellation i in targets)
        {
            TargetScript obj = Instantiate(Target, i.position, Quaternion.identity).GetComponent<TargetScript>();
            obj.Set(GameManager);
            Targets.Add(obj);
        }

    }
    public void GenerateStars(Vector2 range, float threshold)
    {
        //スクリーンのサイズをワールド内のサイズに変換
        Vector3 worldScreen = Camera.main.ScreenToWorldPoint(new Vector3(Screen.currentResolution.width, Screen.currentResolution.height, 0.0f));
        // 星を生成
        for (int y = 0; y < (int)range.y; y += 2)
        {
            for (int x = 0; x < (int)range.x; x += 2)
            {
                Vector2 pos = new Vector2(x - range.x / 2, y - range.y / 2);
                if (pos.x > -worldScreen.x && pos.x < worldScreen.x
                    && pos.y > -worldScreen.y && pos.y < worldScreen.y)
                    // スクリーンには生成しない
                    continue;

                float noise = Mathf.PerlinNoise((float)x * 0.7f, (float)y * 0.7f);
                //閾値より大きかったら生成
                if (noise > threshold)
                {
                    Instantiate(Star[0], new Vector3(pos.x, pos.y, 0.0f), Quaternion.identity);
                }
            }
        }


    }

    public List<TargetScript> GetTargets()
    {
        return Targets;
    }
}
