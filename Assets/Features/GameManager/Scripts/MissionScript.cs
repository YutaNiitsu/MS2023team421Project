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

    public MissionScript(MissionType type, GameManagerScript gameManager)
    {
        Type = type;
        GameManager = gameManager;
    }

    // Start is called before the first frame update
    void Start()
    {
        ProceduralGenerator = gameObject.GetComponent<ProceduralGenerator>();
        //GameManager = gameObject.GetComponent<GameManagerScript>();
    }

    ////ミッションの実行
    //public void ExecuteMission(MissionType type)
    //{
       
    //}

    //ミッションクリアしたかどうか
    public bool IsMissionComplete()
    {
        bool success = false;

        switch (Type)
        {
            case MissionType.ステージクリア:
                if (GameManager.IsStageComplete)
                {
                    Debug.Log("ステージクリアした");
                    success = true;
                }
                break;
            case MissionType.障害物衝突回数ｎ回以下:
                if (GameManager.Setting.ObstacleCollisionNumber >= GameManager.ObstacleCollisionNumber)
                {
                    string s = GameManager.Setting.ObstacleCollisionNumber.ToString();
                    s += "回障害物に衝突した";
                    Debug.Log(s);
                }
                break;
            case MissionType.特別ポイントにユニーク星をはめ込む:
                
                break;
            case MissionType.ｎターン以内にクリア:
                if (GameManager.Setting.DischargeNumberWithinClear >= GameManager.DischargeNumber)
                {
                    string s = GameManager.Setting.DischargeNumberWithinClear.ToString();
                    s += "ターン以内にクリアした";
                    Debug.Log(s);
                    success = true;
                }
                break;
            case MissionType.障害物ｎ回以上壊す:
                if (GameManager.Setting.ObstacleDestroyNumber >= GameManager.ObstacleDestroyNumber)
                {
                    string s = GameManager.Setting.ObstacleDestroyNumber.ToString();
                    s += "回障害物を壊した";
                    Debug.Log(s);
                }
                break;
            default:
                break;
        }

        return success;
    }

}
