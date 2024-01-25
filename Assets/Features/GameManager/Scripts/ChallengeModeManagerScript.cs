using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using static MissionScript;

public class ChallengeModeManagerScript : StageManagerScript
{
    public override void StageManagerStart()
    {
        Initialize();
        CreateObjects();
    }

    public override void CreateObjects()
    {
        //�����f�[�^���������琶�����Ȃ�
        if (ConstellationDatas == null)
            return;

        SaveConstellationData temp = null;

        //�͂ߍ��ތ^��z�u
        {
            int index = UnityEngine.Random.Range(0, ConstellationDatas.Length - 1);
            temp = ConstellationDatas[index];
            ProceduralGenerator.CreateTargets(temp);
            GenerateConstellation = temp;
        }
        
        //�͂ߍ��ތ^�̐�����X�e�[�W��ݒ�
        int targetNum = GenerateConstellation.targets.Length;
        Setting.DischargeNumber = targetNum * 10;
        Setting.ShieldNumber = targetNum / 2;
        Setting.SpecialPointNumber = targetNum / 2;


        //���Ə�Q����z�u
        ProceduralGenerator.CreateStarObstacle(Setting.StageSize, Setting.Threshold);

        
        foreach (SaveConstellationData i in ConstellationDatas)
        {
            if (i.name == Setting.ConstellationName)
            {
                temp = i;
            }
        }

        //�~�b�V����������
        {
            int len = Setting.MissionTypes.Length;
            Missions = new MissionScript[len];
            int index = 0;
            foreach (MissionType i in Setting.MissionTypes)
            {
                Missions[index] = new MissionScript(i);
                index++;
            }
        }
    }
}
