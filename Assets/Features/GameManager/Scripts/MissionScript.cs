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

    //ステージの最初にミッションを設定する
    //stageSetting ステージデータ
    //constellationDatas 星座データ
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
            Debug.Log("ミッションが設定されていない");
        }

        ConstellationDatas = constellationDatas;
    }
    
    //ミッションの実行
    //実行できたらtrue返す
    public bool ExecuteMission()
    {
        if (ConstellationDatas != null && ConstellationDatas.Length > 0)
        {
            Determination = null;
            if (Missions1.Length > 0 && Missions1.Length <= MissinIndex + 1)
            {
                //ミッションで完成させる星座を名前で指定
                string[] name = Missions1[MissinIndex].Name;
                int nameIndex = 0;
                if (name.Length > 0)
                {
                    //ランダムでどれか一つ
                    nameIndex = UnityEngine.Random.Range(0, name.Length);
                    foreach (SaveConstellationData i in ConstellationDatas)
                    {
                        if (i.name == Missions1[MissinIndex].Name[nameIndex])
                        {
                            //名前が一致した
                            Determination = i;
                            break;
                        }
                    }
                }
                else
                {
                    //何も指定されてなかった場合は全ての星座データからランダムで一つ
                    int rand = UnityEngine.Random.Range(0, ConstellationDatas.Length);
                    Determination = ConstellationDatas[rand];
                }
            }
            else if (Missions2.Length > 0 && Missions2.Length <= MissinIndex + 1)
            {
                //ミッションで完成させる星座を星をはめ込む型の数で指定
                int min = Missions2[MissinIndex].minNumber;
                int max = Missions2[MissinIndex].maxNumber;

                //はめ込む型の数が指定範囲内ならリストに追加
                List<SaveConstellationData> fits = new List<SaveConstellationData>();
                foreach (SaveConstellationData i in ConstellationDatas)
                {
                    //はめ込む型の数取得
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
            //星座の生成
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
                //星がはまっていないものがあったら失敗
                isComp = false;
                break;
            }
        }

        if (isComp)
        {
            //全部はまっていた
            return true;
            //ミッションクリア処理

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
