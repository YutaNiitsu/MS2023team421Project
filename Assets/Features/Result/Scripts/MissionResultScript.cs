using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static MissionScript;

public class MissionResultScript : MonoBehaviour
{
    public Image StarImage;
    public Text ContentText;
    public Sprite UncompStarSprite;
    public Sprite CompStarSprite;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Set(bool IsComp, MissionType type)
    {
        GameManagerScript gameManager = GameManagerScript.instance;

        if (IsComp)
        {
            StarImage.sprite = CompStarSprite;
        }
        else
        {
            StarImage.sprite = UncompStarSprite;
        }
        //ミッション内容を記述
        string s = "";
        switch (type)
        {
            case MissionType.ステージクリア:
                ContentText.text = "ステージクリアした";

                break;
            case MissionType.障害物衝突回数ｎ回以下:
                s = gameManager.Setting.ObstacleCollisionNumber.ToString();
                s = "障害物衝突が" + s + "回以内";
                ContentText.text = s;

                break;
            case MissionType.特別ポイントにユニーク星をはめ込む:
                ContentText.text = "特別ポイントにユニーク星をはめ込んだ";

                break;
            case MissionType.ｎターン以内にクリア:
                s = gameManager.Setting.DischargeNumberWithinClear.ToString();
                s += "ターン以内にクリアした";
                ContentText.text = s;

                break;
            case MissionType.障害物ｎ回以上壊す:
                s = gameManager.ObstacleDestroyNumber.ToString();
                s += "回障害物を壊した";
                ContentText.text = s;

                break;
            default:
                break;
        }
    }
}
