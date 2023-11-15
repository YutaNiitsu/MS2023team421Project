using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionScript : MonoBehaviour
{
    private ProceduralGenerator ProceduralGenerator;
    private SaveConstellationData[] ConstellationDatas;
    private SaveConstellationData Determination;
    private int MissinIndex;
    private Mission1[] Missions1;
    private Mission2[] Missions2;
    // Start is called before the first frame update
    void Start()
    {
        ProceduralGenerator = GetComponent<ProceduralGenerator>();
    }

    //�X�e�[�W�̍ŏ��Ƀ~�b�V������ݒ肷��
    //stageSetting �X�e�[�W�f�[�^
    //constellationDatas �����f�[�^
    public void SetMission(StageSetting stageSetting, SaveConstellationData[] constellationDatas)
    {
        if (stageSetting.missions1.Length > 0)
        {
            Missions1 = stageSetting.missions1;
        }
        else if (stageSetting.missions2.Length > 0)
        {
            Missions2 = stageSetting.missions2;
        }
        else
        {
            Debug.Log("�~�b�V�������ݒ肳��Ă��Ȃ�");
        }

        ConstellationDatas = constellationDatas;
    }
    
    //�~�b�V�����̎��s
    //���s�ł�����true�Ԃ�
    public bool ExecuteMission()
    {
        if (ConstellationDatas != null && ConstellationDatas.Length > 0)
        {
            Determination = null;
            if (Missions1.Length > 0 && Missions1.Length <= MissinIndex + 1)
            {
                //�~�b�V�����Ŋ��������鐯���𖼑O�Ŏw��
                string[] name = Missions1[MissinIndex].Name;
                int nameIndex = 0;
                if (name.Length > 0)
                {
                    //�����_���łǂꂩ���
                    nameIndex = UnityEngine.Random.Range(0, name.Length);
                    foreach (SaveConstellationData i in ConstellationDatas)
                    {
                        if (i.name == Missions1[MissinIndex].Name[nameIndex])
                        {
                            //���O����v����
                            Determination = i;
                            break;
                        }
                    }
                }
                else
                {
                    //�����w�肳��ĂȂ������ꍇ�͑S�Ă̐����f�[�^���烉���_���ň��
                    int rand = UnityEngine.Random.Range(0, ConstellationDatas.Length);
                    Determination = ConstellationDatas[rand];
                }
            }
            else if (Missions2.Length > 0 && Missions2.Length <= MissinIndex + 1)
            {
                //�~�b�V�����Ŋ��������鐯���𐯂��͂ߍ��ތ^�̐��Ŏw��
                int min = Missions2[MissinIndex].minNumber;
                int max = Missions2[MissinIndex].maxNumber;

                //�͂ߍ��ތ^�̐����w��͈͓��Ȃ烊�X�g�ɒǉ�
                List<SaveConstellationData> fits = new List<SaveConstellationData>();
                foreach (SaveConstellationData i in ConstellationDatas)
                {
                    //�͂ߍ��ތ^�̐��擾
                    int num = i.constellations.Length;
                    if (num >= min && num <= max)
                    {
                        fits.Add(i);
                    }
                }
                int rand = UnityEngine.Random.Range(0, fits.Count);
                Determination = fits[rand];
            }

        }

        MissinIndex++;

        if (Determination != null)
        {
            //�����̐���
            ProceduralGenerator.GenerateTargets(Determination.constellations);
            return true;
        }
        return false;
    }

    public bool IsMissionComplete()
    {
        bool isComp = true;
        foreach (TargetScript i in ProceduralGenerator.GetTargets())
        {
            if (!i.IsGoal())
            {
                //�����͂܂��Ă��Ȃ����̂��������玸�s
                isComp = false;
                break;
            }
        }

        if (isComp)
        {
            //�S���͂܂��Ă���
            return true;
            //�~�b�V�����N���A����

        }

        return false;
    }

    public bool IsAllMissionsComplete()
    {
        if (!IsMissionComplete())
            return false;

        if (Missions1.Length < MissinIndex + 1)
        {
            return true;
        }
        else if (Missions2.Length < MissinIndex + 1)
        {
            return true;
        }
        return false;
    }
}
