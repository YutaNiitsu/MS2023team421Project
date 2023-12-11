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
        //�~�b�V�������e���L�q
        string s = "";
        switch (type)
        {
            case MissionType.�X�e�[�W�N���A:
                ContentText.text = "�X�e�[�W�N���A����";

                break;
            case MissionType.��Q���Փˉ񐔂���ȉ�:
                s = gameManager.Setting.ObstacleCollisionNumber.ToString();
                s = "��Q���Փ˂�" + s + "��ȓ�";
                ContentText.text = s;

                break;
            case MissionType.���ʃ|�C���g�Ƀ��j�[�N�����͂ߍ���:
                ContentText.text = "���ʃ|�C���g�Ƀ��j�[�N�����͂ߍ���";

                break;
            case MissionType.���^�[���ȓ��ɃN���A:
                s = gameManager.Setting.DischargeNumberWithinClear.ToString();
                s += "�^�[���ȓ��ɃN���A����";
                ContentText.text = s;

                break;
            case MissionType.��Q������ȏ��:
                s = gameManager.ObstacleDestroyNumber.ToString();
                s += "���Q�����󂵂�";
                ContentText.text = s;

                break;
            default:
                break;
        }
    }
}
