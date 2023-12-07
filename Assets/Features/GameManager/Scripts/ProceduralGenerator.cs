using NUnit.Framework;
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
    public GameObject NormalStar;
    public GameObject BouncingStar;
    public GameObject TransfixStar;
    public GameObject IgnoreTeleportationStar;
    public GameObject ExplosionStar;
    [Header("障害物のプレハブ")]
    public GameObject Obstacle;
    public GameObject Teleportation;
    public GameObject DarkHole;
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
    public void CreateTargets(SaveConstellationData data)
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
    private void SetRandomPositions(out Vector2[] positions, Vector2 stageSize, float threshold)
    {
        positions = new Vector2[0];
        //既に設置された星を参照
        List<GameObject> stars = new List<GameObject>();
        GameObject.FindGameObjectsWithTag("Star", stars);
        //既に設置された障害物を参照
        List<GameObject> obstacles = new List<GameObject>();
        GameObject.FindGameObjectsWithTag("Obstacle", obstacles);
        int index = 0;
        //スクリーンのサイズをワールド内のサイズに変換
        Vector3 worldScreen = Camera.main.ScreenToWorldPoint(new Vector3(Screen.currentResolution.width, Screen.currentResolution.height, 0.0f));
        // 星を生成
        for (int y = 0; y < (int)stageSize.y; y += 2)
        {
            for (int x = 0; x < (int)stageSize.x; x += 2)
            {
                Vector2 pos = new Vector2(x - stageSize.x / 2, y - stageSize.y / 2);
                bool success = true;
                foreach (GameObject j in stars)
                {
                    Vector2 dis = new Vector2(j.transform.position.x, j.transform.position.y) + pos;
                    float lenSq = Vector2.Dot(dis, dis);
                    //既に置かれてる星と距離近かったら失敗
                    if (lenSq < 9.0f)
                        success = false;
                }
                foreach (GameObject j in obstacles)
                {
                    Vector2 dis = new Vector2(j.transform.position.x, j.transform.position.y) + pos;
                    float lenSq = Vector2.Dot(dis, dis);
                    //既に置かれてる障害物と距離近かったら失敗
                    if (lenSq < 9.0f)
                        success = false;
                }

                //失敗したらとばす
                if (!success)
                    continue;

                if (pos.x > -worldScreen.x && pos.x < worldScreen.x
                    && pos.y > -worldScreen.y && pos.y < worldScreen.y)
                    // カメラスクリーンには生成しない
                    continue;

                float noise = Mathf.PerlinNoise((float)x * 0.7f, (float)y * 0.7f);
                
                if (noise > threshold)
                {
                    //閾値より大きかった
                    Array.Resize(ref positions, positions.Length + 1);
                    positions[index] = pos;
                    index++;
                }
            }
        }
    }

    //星と障害物を生成
    //range : 生成範囲
    //threshold : 閾値
    public void CreateStar(Vector2 stageSize, float threshold)
    {
        Vector2[] positions;
        //生成位置を決定
        SetRandomPositions(out positions, stageSize, threshold);
        //既に何か生成されているかどうかの判別用
        bool[] determination = new bool[positions.Length];

        //障害物の生成
        int index = 0;

        GameObject[] obstacles = new GameObject[3] { Obstacle, DarkHole, Teleportation };
        foreach (Vector2 i in positions)
        {

            int rand = UnityEngine.Random.Range(0, 10);
            if (!determination[index])
            {
                if (rand < 2)
                {
                    //障害物の生成
                    Instantiate(obstacles[rand], new Vector3(i.x, i.y, 0.0f), Quaternion.identity);
                    determination[index] = true;
                }
                else if (rand == 2)
                {
                    //ワープ障害物の生成
                    //2か所位置決める
                    List<int> indexs = SelectRandomElements(2, 0, positions.Length - 1);
                    //何も置かれてなかったら生成
                    if (!determination[indexs[0]] && !determination[indexs[1]])
                    {
                        GameObject obj = Instantiate(Teleportation, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
                        Color color = new Color();
                        color.r = UnityEngine.Random.Range(0f, 1f);
                        color.g = UnityEngine.Random.Range(0f, 1f);
                        color.b = UnityEngine.Random.Range(0f, 1f);
                        color.a = 1.0f;
                        obj.GetComponent<TeleportationScript>().Set(positions[indexs[0]], positions[indexs[1]], color);
                        determination[indexs[0]] = true;
                        determination[indexs[1]] = true;
                    }
                }
            }
            index++;
        }

        //星の生成
        GameObject[] stars = new GameObject[5] { NormalStar, BouncingStar, TransfixStar, IgnoreTeleportationStar, ExplosionStar };
        
        index = 0;
        foreach (Vector2 i in positions)
        {
            if (!determination[index])
            {
                int rand = UnityEngine.Random.Range(0, 4);
                Instantiate(stars[rand], new Vector3(i.x, i.y, 0.0f), Quaternion.identity);
            }  
            index++;
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

    public StarRarity SetStarRarity(Vector3 pos)
    {
        StarRarity result = StarRarity.Unique;
        StageSetting setting = GameManagerScript.instance.Setting;
        Vector2 stageSize = setting.StageSize;
        float[] p = new float[4]{
        setting.NormalPoint,
        setting.RarePoint,
        setting.UniquePoint,
        setting.LegendaryPoint
        };

        float lenSq = Vector2.Dot(pos, pos);
        float t = lenSq / (stageSize.x * 0.5f * stageSize.y * 0.5f);

        result = (StarRarity)SelectRandomExp(p,t);

        return result;
    }

    //p : x値の配列
    //t : 極大値の位置をずらす
    int SelectRandomExp(float[] p, float t)
    {
        int result = 0;
        foreach (float x in p)
        {
            float y = (float)Math.Exp(-Math.Pow((x * 2.0f - t) * 1.0f, 2));
            float rand = UnityEngine.Random.Range(0.1f, 1.0f);
            if (y >= rand)
                break;
            result++;
        }
        if (result == 4)
            result = 0;

        return result;
    }
}
