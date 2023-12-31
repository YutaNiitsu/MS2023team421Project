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

    public MissionType Type { get; protected set; }


    private MissionScript() { }

    public MissionScript(MissionType type)
    {
        Type = type;
    }

    //ミッションクリアしたかどうか
    public bool IsMissionComplete()
    {
        bool success = false;
        StageManagerScript gameManager = GameManagerScript.instance.StageManager;

        switch (Type)
        {
            case MissionType.ステージクリア:
                if (gameManager.IsStageComplete)
                {
                    Debug.Log("ステージクリアした");
                    success = true;
                }
                break;
            case MissionType.障害物衝突回数ｎ回以下:
                if (gameManager.Setting.ObstacleCollisionNumber >= gameManager.ObstacleCollisionNumber)
                {
                    string s = gameManager.Setting.ObstacleCollisionNumber.ToString();
                    s = "障害物衝突が" + s + "回以内";
                    Debug.Log(s);
                    success = true;
                }
                break;
            case MissionType.特別ポイントにユニーク星をはめ込む:
                if (gameManager.ProceduralGenerator.IsRareStarGoaledOnSpecialTargetAll())
                {
                    Debug.Log("特別ポイントにユニーク星をはめ込んだ");
                    success = true;
                }
                break;
            case MissionType.ｎターン以内にクリア:
                if (gameManager.Setting.DischargeNumberWithinClear >= gameManager.Setting.DischargeNumber - gameManager.DischargeNumber)
                {
                    string s = gameManager.Setting.DischargeNumberWithinClear.ToString();
                    s += "ターン以内にクリアした";
                    Debug.Log(s);
                    success = true;
                }
                break;
            case MissionType.障害物ｎ回以上壊す:
                if (gameManager.Setting.ObstacleDestroyNumber <= gameManager.ObstacleDestroyNumber)
                {
                    string s = gameManager.ObstacleDestroyNumber.ToString();
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
