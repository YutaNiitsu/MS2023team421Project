using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class MissionScript : MonoBehaviour
{
    public enum MissionType
    {
        ステージクリア,
        障害物衝突回数ｎ回以下,
        特別ポイントにユニーク星をはめ込む,
        ｎターン以内にクリア,
        障害物ｎ回以上壊す,
        [InspectorName("")]  MissionTypeMax
    }

    private MissionType Type;
    private ProceduralGenerator ProceduralGenerator;
    private SaveConstellationData[] ConstellationDatas;
    private SaveConstellationData Determination;
    private GameManagerScript GameManager;

    private MissionScript() { }

    MissionScript(MissionType type)
    {
        Type = type;
    }

    // Start is called before the first frame update
    void Start()
    {
        ProceduralGenerator = gameObject.GetComponent<ProceduralGenerator>();
        GameManager = gameObject.GetComponent<GameManagerScript>();
    }

    //ミッションの実行
    public void ExecuteMission(MissionType type)
    {
       
    }

    //ミッションクリアしたかどうか
    public bool IsMissionComplete()
    {
       

        return false;
    }

}
