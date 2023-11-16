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

    //�~�b�V�����̎��s
    public void ExecuteMission(MissionType type)
    {
       
    }

    //�~�b�V�����N���A�������ǂ���
    public bool IsMissionComplete()
    {
       

        return false;
    }

}
