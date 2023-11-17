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

    ////�~�b�V�����̎��s
    //public void ExecuteMission(MissionType type)
    //{
       
    //}

    //�~�b�V�����N���A�������ǂ���
    public bool IsMissionComplete()
    {
        bool success = false;

        switch (Type)
        {
            case MissionType.�X�e�[�W�N���A:
                if (GameManager.IsStageComplete)
                {
                    Debug.Log("�X�e�[�W�N���A����");
                    success = true;
                }
                break;
            case MissionType.��Q���Փˉ񐔂���ȉ�:
                if (GameManager.Setting.ObstacleCollisionNumber >= GameManager.ObstacleCollisionNumber)
                {
                    string s = GameManager.Setting.ObstacleCollisionNumber.ToString();
                    s += "���Q���ɏՓ˂���";
                    Debug.Log(s);
                }
                break;
            case MissionType.���ʃ|�C���g�Ƀ��j�[�N�����͂ߍ���:
                
                break;
            case MissionType.���^�[���ȓ��ɃN���A:
                if (GameManager.Setting.DischargeNumberWithinClear >= GameManager.DischargeNumber)
                {
                    string s = GameManager.Setting.DischargeNumberWithinClear.ToString();
                    s += "�^�[���ȓ��ɃN���A����";
                    Debug.Log(s);
                    success = true;
                }
                break;
            case MissionType.��Q������ȏ��:
                if (GameManager.Setting.ObstacleDestroyNumber >= GameManager.ObstacleDestroyNumber)
                {
                    string s = GameManager.Setting.ObstacleDestroyNumber.ToString();
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
