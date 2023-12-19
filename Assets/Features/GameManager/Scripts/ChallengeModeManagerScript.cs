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
        //星座データ無かったら生成しない
        if (ConstellationDatas == null)
            return;

        SaveConstellationData temp = null;

        //はめ込む型を配置
        {
            int index = UnityEngine.Random.Range(0, ConstellationDatas.Length - 1);
            temp = ConstellationDatas[index];
            ProceduralGenerator.CreateTargets(temp);
            GenerateConstellation = temp;
        }
        
        //はめ込む型の数からステージを設定
        int targetNum = GenerateConstellation.targets.Length;
        Setting.DischargeNumber = targetNum * 10;
        Setting.ShieldNumber = targetNum / 2;
        Setting.SpecialPointNumber = targetNum / 2;


        //星と障害物を配置
        ProceduralGenerator.CreateStarObstacle(Setting.StageSize, Setting.Threshold);

        
        foreach (SaveConstellationData i in ConstellationDatas)
        {
            if (i.name == Setting.ConstellationName)
            {
                temp = i;
            }
        }

        //ミッションを決定
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
