using System;
using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.PlayerSettings;
// ゲーム開始時に星とそれをはめ込む場所を配置するシステム
public class ProceduralGenerator : MonoBehaviour
{
    [Header("星をはめ込む型のプレハブ")]
    public GameObject Target;
    [Header("星のプレハブ")]
    public GameObject[] Star;
    [Header("レア星生成エリアのプレハブ")]
    public GameObject[] RareStarArea;
    [Header("動かない障害物のプレハブ")]
    public GameObject NormalObstacle;
    [Header("ダークホールのプレハブ")]
    public GameObject DarkHoleObstacle;
    [Header("ワープ障害物のプレハブ")]
    public GameObject TeleportationObstacle;
    public TargetScript[] Targets { get; protected set; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    //はめ込む型を配置
    //targets : はめ込む型のセーブデータ
    //range : 星の生成範囲
    //threshold : 星の密度が変わる
    public void GenerateTargets(SaveConstellationData data)
    {
        if (Targets != null && Targets.Length > 0)
        {
            foreach (TargetScript i in Targets)
            {
                Destroy(i.gameObject);
            }
        }

        Targets = new TargetScript[data.targets.Length];
        // 星をはめ込む型生成
        StageSetting setting = GameManagerScript.instance.Setting;
        //特別ポイント、シールドにする型の要素番号リスト
        List<int> specialPoints = SelectRandomElements(setting.SpecialPointNumber, 0, Targets.Length - 1);
        List<int> shields = SelectRandomElements(setting.ShieldNumber, 0, Targets.Length - 1);


        int index = 0;
        foreach (ST_Constellation i in data.targets)
        {
            TargetScript obj = Instantiate(Target, i.position, Quaternion.identity).GetComponent<TargetScript>();

            //特別ポイント、シールドにする型の要素番号リストからindexに一致する要素番号が見つかったら設定する
            bool isSpecialPoint = false;
            bool isShield = false;
            if (specialPoints.Contains(index))
                isSpecialPoint = true;
       
            if (shields.Contains(index))
                isShield = true;

            obj.Set(isSpecialPoint, isShield);
            Targets[index] = obj;
            index++;
        }

    }
    //生成位置を決定
    //private void SetRandomPositions(out Vector2[] positions)
    //{

    //}

    //星を生成
    //range : 生成範囲
    //threshold : 閾値
    public void GenerateStars(Vector2 range, float threshold)
    {
        //スクリーンのサイズをワールド内のサイズに変換
        Vector3 worldScreen = Camera.main.ScreenToWorldPoint(new Vector3(Screen.currentResolution.width, Screen.currentResolution.height, 0.0f));
        // 星を生成
        for (int y = 0; y < (int)range.y; y += 2)
        {
            for (int x = 0; x < (int)range.x; x += 2)
            {
                Vector2 pos = new Vector2(x - range.x / 2, y - range.y / 2);
                if (pos.x > -worldScreen.x && pos.x < worldScreen.x
                    && pos.y > -worldScreen.y && pos.y < worldScreen.y)
                    // カメラスクリーンには生成しない
                    continue;

                float noise = Mathf.PerlinNoise((float)x * 0.7f, (float)y * 0.7f);
                //閾値より大きかったら生成
                if (noise > threshold)
                {
                    float lenSq = Vector2.Dot(pos, pos);
                    float len = Vector2.Distance(pos, new Vector2(0.0f, 0.0f));
                    float rand = UnityEngine.Random.Range(0.0f, 1.0f);
                    if (lenSq > Math.Pow(200, 2) && rand > (400.0f - (len - 200.0f) * 0.3f) / 400.0f)
                    {
                        Instantiate(RareStarArea[0], new Vector3(pos.x, pos.y, 0.0f), Quaternion.identity);
                    }
                    else
                    {
                        Instantiate(Star[0], new Vector3(pos.x, pos.y, 0.0f), Quaternion.identity);
                    }
                    
                }
            }
        }


    }

    //全てのはめ込む型に星がはまっているか
    public bool IsAllGoaled()
    {
        bool success = true;
        foreach (TargetScript i in Targets)
        {
            if (!i.Goaled)
            {
                //星がはまっていないものがあったら失敗
                success = false;
                break;
            }
        }

        return success;
    }

    //特別ポイントに指定されているはめ込む型全てにユニーク以上のレアリティの星がはまっているかどうか
    public bool IsRareStarGoaledOnSpecialTargetAll()
    {
        bool success = false;
        foreach (TargetScript i in Targets)
        {
            if (!i.Goaled)
            {
                //星がはまっていなかったらとばす
                continue;
            }

            if (i.IsRareStarGoaledOnSpecialTarget())
            {
                success = true;
            }
            else
            {
                //はまっている星がユニーク以上のレアリティではなかったら失敗
                success = false;
                break;
            }
        }

        return success;
    }

    //ランダムな要素を指定された個数だけ選択
    public List<int> SelectRandomElements(int count, int min, int max)
    {
        List<int> result = new List<int>();
        List<int> data = new List<int>();
        //minからmaxまでの数字データ作成
        for (int i = min; i < max + 1; i++)
        {
            data.Add(i);
        }

        for (int i = 0; i < count; i++)
        {
            if (data.Count == 0)
                break;

            int rand = UnityEngine.Random.Range(0, data.Count - 1);
            result.Add(data[rand]);
            data.RemoveAt(rand);
        }
        return result;
    }
}
