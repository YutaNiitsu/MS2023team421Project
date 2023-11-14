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

    //ミッションを設定する
    //stageSetting ステージデータ
    //constellationDatas 星座データ
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
            Debug.Log("ミッションが設定されていない");
        }

        if (constellationDatas != null && constellationDatas.Length > 0)
        {
            SaveConstellationData determination = null;
            if (Missions1.Length > 0)
            {
                //ミッションで完成させる星座を名前で指定
                string[] name = Missions1[MissinNumber].Name;
                int nameIndex = 0;
                if (name.Length > 0)
                {
                    //ランダムでどれか一つ
                    nameIndex = UnityEngine.Random.Range(0, name.Length);
                    foreach (SaveConstellationData i in constellationDatas)
                    {
                        if (i.name == Missions1[MissinNumber].Name[nameIndex])
                        {
                            //名前が一致した
                            determination = i;
                            break;
                        }
                    }
                }
                else
                {
                    //何も指定されてなかった場合は全ての星座データからランダムで一つ
                    int rand = UnityEngine.Random.Range(0, constellationDatas.Length);
                    determination = constellationDatas[rand];
                }
            }
            else if (Missions2.Length > 0)
            {
                //ミッションで完成させる星座を星をはめ込む型の数で指定
                int min = Missions2[MissinNumber].minNumber;
                int max = Missions2[MissinNumber].maxNumber;

                //はめ込む型の数が指定範囲内ならリストに追加
                List<SaveConstellationData> fits = new List<SaveConstellationData>();
                foreach (SaveConstellationData i in constellationDatas)
                {
                    //はめ込む型の数取得
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
