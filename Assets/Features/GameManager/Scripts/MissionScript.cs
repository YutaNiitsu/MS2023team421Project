using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionScript : MonoBehaviour
{
    private ProceduralGenerator ProceduralGenerator;
    private int MissinNumber;
    private Mission1[] Missions1;
    private Mission2[] Missions2;
    // Start is called before the first frame update
    void Start()
    {
        ProceduralGenerator = GetComponent<ProceduralGenerator>();
    }

    //�~�b�V������ݒ肷��
    //stageSetting �X�e�[�W�f�[�^
    //constellationDatas �����f�[�^
    public SaveConstellationData SetMission(StageSetting stageSetting, SaveConstellationData[] constellationDatas)
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

        if (constellationDatas != null && constellationDatas.Length > 0)
        {
            SaveConstellationData determination = null;
            if (Missions1.Length > 0)
            {
                //�~�b�V�����Ŋ��������鐯���𖼑O�Ŏw��
                string[] name = Missions1[MissinNumber].Name;
                int nameIndex = 0;
                if (name.Length > 0)
                {
                    //�����_���łǂꂩ���
                    nameIndex = UnityEngine.Random.Range(0, name.Length);
                    foreach (SaveConstellationData i in constellationDatas)
                    {
                        if (i.name == Missions1[MissinNumber].Name[nameIndex])
                        {
                            //���O����v����
                            determination = i;
                            break;
                        }
                    }
                }
                else
                {
                    //�����w�肳��ĂȂ������ꍇ�͑S�Ă̐����f�[�^���烉���_���ň��
                    int rand = UnityEngine.Random.Range(0, constellationDatas.Length);
                    determination = constellationDatas[rand];
                }
            }
            else if (Missions2.Length > 0)
            {
                //�~�b�V�����Ŋ��������鐯���𐯂��͂ߍ��ތ^�̐��Ŏw��
                int min = Missions2[MissinNumber].minNumber;
                int max = Missions2[MissinNumber].maxNumber;

                //�͂ߍ��ތ^�̐����w��͈͓��Ȃ烊�X�g�ɒǉ�
                List<SaveConstellationData> fits = new List<SaveConstellationData>();
                foreach (SaveConstellationData i in constellationDatas)
                {
                    //�͂ߍ��ތ^�̐��擾
                    int num = i.constellations.Length;
                    if (num >= min && num <= max)
                    {
                        fits.Add(i);
                    }
                }
                int rand = UnityEngine.Random.Range(0, fits.Count);
                determination = fits[rand];
            }

            if (determination != null)
            {

            }

            return determination;

        }

        return null;
    }
    
    //public bool IsMissionComplete()
    //{
    //    //if (Missions1.Length > 0 && )
    //    //{
    //    //    return 
    //    //}
    //    //else if (Missions2.Length > 0)
    //    //{

    //    //}
    //}
}
