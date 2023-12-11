using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class MissionScript : MonoBehaviour
{
    public enum MissionType
    {
        �X�e�[�W�N���A,
        ��Q���Փˉ񐔂���ȉ�,
        ���ʃ|�C���g�Ƀ��j�[�N�����͂ߍ���,
        ���^�[���ȓ��ɃN���A,
        ��Q������ȏ��,
        [InspectorName("")]  MissionTypeMax
    }

    public MissionType Type { get; protected set; }


    private MissionScript() { }

    public MissionScript(MissionType type)
    {
        Type = type;
    }

    //�~�b�V�����N���A�������ǂ���
    public bool IsMissionComplete()
    {
        bool success = false;
        GameManagerScript gameManager = GameManagerScript.instance;

        switch (Type)
        {
            case MissionType.�X�e�[�W�N���A:
                if (gameManager.IsStageComplete)
                {
                    Debug.Log("�X�e�[�W�N���A����");
                    success = true;
                }
                break;
            case MissionType.��Q���Փˉ񐔂���ȉ�:
                if (gameManager.Setting.ObstacleCollisionNumber >= gameManager.ObstacleCollisionNumber)
                {
                    string s = gameManager.Setting.ObstacleCollisionNumber.ToString();
                    s = "��Q���Փ˂�" + s + "��ȓ�";
                    Debug.Log(s);
                }
                break;
            case MissionType.���ʃ|�C���g�Ƀ��j�[�N�����͂ߍ���:
                if (gameManager.ProceduralGenerator.IsRareStarGoaledOnSpecialTargetAll())
                {
                    Debug.Log("���ʃ|�C���g�Ƀ��j�[�N�����͂ߍ���");
                    success = true;
                }
                break;
            case MissionType.���^�[���ȓ��ɃN���A:
                if (gameManager.Setting.DischargeNumberWithinClear >= gameManager.Setting.DischargeNumber - gameManager.DischargeNumber)
                {
                    string s = gameManager.Setting.DischargeNumberWithinClear.ToString();
                    s += "�^�[���ȓ��ɃN���A����";
                    Debug.Log(s);
                    success = true;
                }
                break;
            case MissionType.��Q������ȏ��:
                if (gameManager.Setting.ObstacleDestroyNumber <= gameManager.ObstacleDestroyNumber)
                {
                    string s = gameManager.ObstacleDestroyNumber.ToString();
                    s += "���Q�����󂵂�";
                    Debug.Log(s);
                }
                break;
            default:
                break;
        }

        return success;
    }


}
