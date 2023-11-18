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

    private MissionType Type;
    private ProceduralGenerator ProceduralGenerator;
    private SaveConstellationData[] ConstellationDatas;
    private SaveConstellationData Determination;

    private MissionScript() { }

    public MissionScript(MissionType type)
    {
        Type = type;
    }

    // Start is called before the first frame update
    void Start()
    {
        ProceduralGenerator = gameObject.GetComponent<ProceduralGenerator>();
        //GameManager = gameObject.GetComponent<GameManagerScript>();
    }

    ////�~�b�V�����̎��s
    //public void ExecuteMission(MissionType type)
    //{
       
    //}

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
                    s += "���Q���ɏՓ˂���";
                    Debug.Log(s);
                }
                break;
            case MissionType.���ʃ|�C���g�Ƀ��j�[�N�����͂ߍ���:
                if (ProceduralGenerator.IsRareStarGoaledOnSpecialTargetAll())
                {
                    Debug.Log("���ʃ|�C���g�Ƀ��j�[�N�����͂ߍ���");
                    success = true;
                }
                break;
            case MissionType.���^�[���ȓ��ɃN���A:
                if (gameManager.Setting.DischargeNumberWithinClear >= gameManager.DischargeNumber)
                {
                    string s = gameManager.Setting.DischargeNumberWithinClear.ToString();
                    s += "�^�[���ȓ��ɃN���A����";
                    Debug.Log(s);
                    success = true;
                }
                break;
            case MissionType.��Q������ȏ��:
                if (gameManager.Setting.ObstacleDestroyNumber >= gameManager.ObstacleDestroyNumber)
                {
                    string s = gameManager.Setting.ObstacleDestroyNumber.ToString();
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
