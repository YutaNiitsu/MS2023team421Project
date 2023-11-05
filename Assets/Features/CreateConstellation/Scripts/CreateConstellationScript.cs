using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEditor.PlayerSettings;
using static UnityEngine.GraphicsBuffer;

public class CreateConstellationScript : MonoBehaviour
{
    //星をつなぐ線の太さ
    public float LineWidth;
    // はめ込む型のプレハブ
    public GameObject TargetPrefab;
    // 星をつなぐ線のプレハブ
    public LineRenderer LineRendererPrefab;
    //ボタン
    public Button SaveButton;
    public Button PutTargetInLineButton;
    public Button PutTargetInLoopButton;
    public Button DeterminationButton;
    //入力欄
    public GameObject InputName;
    // 設置されたはめ込む型
    private GameObject[] Targets;
    //セーブ用のはめ込む型
    //private ST_Constellation[] Constellations;
    // 星をつなぐ線のラインレンダラー
    private LineRenderer[] LineRenderers;
    //セーブ用の星をつなぐ線
    private Line[] Lines;
    //LineRenderersの要素番号
    private int LineRendererIndex = 0;
    //Linesの要素番号
    private int LineIndex = 0;
    //LineRendererの点の番号
    private int LineRendererPointIndex = 0;

    //配置モード
    private int Mode = 2;

    //セーブデータから読み込んだデータ
    private SaveConstellationData SavedConstellationData = null;

    // Start is called before the first frame update
    void Start()
    {
        Targets = new GameObject[0];
        LineRenderers = new LineRenderer[0];
        Lines = new Line[0];

        DeterminationButton.interactable = false;
    }

    //配置モード切り替え
    public void ChangeMode(int num)
    {
        Mode = num;
        switch (Mode)
        {
            case 0:
                // 線状に配置するモード
                SaveButton.interactable = false;
                PutTargetInLineButton.interactable = false;
                PutTargetInLoopButton.interactable = false;
                DeterminationButton.interactable = true;

                //線のインスタンスを生成
                Array.Resize<LineRenderer>(ref LineRenderers, LineRenderers.Length + 1);
                Array.Resize<Line>(ref Lines, Lines.Length + 1);
                LineRenderers[LineRendererIndex] = Instantiate(LineRendererPrefab.gameObject).GetComponent<LineRenderer>();
                LineRenderers[LineRendererIndex].positionCount = 0;
                break;
            case 1:
                // 環状に配置するモード
                SaveButton.interactable = false;
                PutTargetInLineButton.interactable = false;
                PutTargetInLoopButton.interactable = false;
                DeterminationButton.interactable = true;

                //線のインスタンスを生成
                Array.Resize<LineRenderer>(ref LineRenderers, LineRenderers.Length + 1);
                Array.Resize<Line>(ref Lines, Lines.Length + 1);
                LineRenderers[LineRendererIndex] = Instantiate(LineRendererPrefab.gameObject).GetComponent<LineRenderer>();
                LineRenderers[LineRendererIndex].positionCount = 0;
                break;
            case 2:
                // 決定ボタン押された
                SaveButton.interactable = true;
                PutTargetInLoopButton.interactable = true;
                PutTargetInLineButton.interactable = true;
                DeterminationButton.interactable = false;


                //LineRendererの要素番号を進める
                LineRendererIndex++;
                //LineRendererの点の要素番号を0にする
                LineRendererPointIndex = 0;
                break;
            default:
                break;
        }

       

    }

    //はめ込む型を設置する
    public void PutTarget(Vector3 pos)
    {
        switch (Mode)
        {
            case 0:
                // 線状に配置するモード
                PutTargetInLine(pos);
                break;
            case 1:
                // 環状に配置するモード
                PutTargetInLoop(pos);
                break;
            default:
                break;
        }
    }

    //はめ込む型を線状に設置する
    private void PutTargetInLine(Vector3 pos)
    {
        //はめ込む型を配置
        Array.Resize<GameObject>(ref Targets, Targets.Length + 1);
        Targets[Targets.Length - 1] = Instantiate(TargetPrefab, pos, Quaternion.identity);

        // 線を配置
        LineRenderers[LineRendererIndex].startWidth = LineWidth;
        LineRenderers[LineRendererIndex].endWidth = LineWidth;
        LineRenderers[LineRendererIndex].positionCount++;
        pos.z += 5f;
        LineRenderers[LineRendererIndex].SetPosition(LineRendererPointIndex, pos);
        LineRendererPointIndex++;
        // 点が２つ以上の時は線の情報を更新する
        if (LineRenderers[LineRendererIndex].positionCount > 1)
        {
            Array.Resize<Line>(ref Lines, LineIndex + 1);
            Vector3 start = LineRenderers[LineRendererIndex].GetPosition(LineRenderers[LineRendererIndex].positionCount - 2);
            Vector3 end = LineRenderers[LineRendererIndex].GetPosition(LineRenderers[LineRendererIndex].positionCount - 1);
            int startTargetIndex = Targets.Length - 2;
            int endTargetIndex = Targets.Length - 1;
            int index = LineRenderers[LineRendererIndex].positionCount - 2;
            Lines[LineIndex] = new Line();
            //始点、終点、始点のはめ込む型と終点のはめ込む型の要素番号を保存
            Lines[LineIndex].Create(start, end, startTargetIndex, endTargetIndex);
            LineIndex++;
        }
    }

    //はめ込む型を環状に設置する
    private void PutTargetInLoop(Vector3 pos)
    {
        
    }

    // 初期化
    public void Initialize()
    {
        //インスタンスを削除
        foreach (GameObject i in Targets)
        {
            Destroy(i);
        }
        foreach (LineRenderer i in LineRenderers)
        {
            Destroy(i.gameObject);
        }

        Targets = new GameObject[0];
        LineRenderers = new LineRenderer[0];
        Lines = new Line[0];

        LineRendererIndex = 0;
        LineIndex = 0;
        LineRendererPointIndex = 0;
        Mode = 2;

        DeterminationButton.interactable = false;
        SavedConstellationData = null;
    }

    // セーブデータ作成
    public SaveConstellationData CreateSaveData()
    {
        SaveConstellationData saveConstellationData = new SaveConstellationData();
        int year = DateTime.Now.Date.Year;
        int month = DateTime.Now.Date.Month;
        int day = DateTime.Now.Date.Day;

        ST_Constellation[] constellations = new ST_Constellation[Targets.Length];
        int index = 0;
        foreach(GameObject i in Targets)
        {
            constellations[index].position = i.transform.position;
            index++;
        }

        InputField input = InputName.GetComponent<InputField>();
        //入力された星座の名前
        string name = input.text;
        uint id = 0;

        //セーブデータから読み込んだ星座ならIDコピー
        if (SavedConstellationData != null)
        {
            id = SavedConstellationData.id;
        }
            

        saveConstellationData.Create(year, month, day, id, name, constellations, Lines);


        return saveConstellationData;
    }


    //セーブデータから読み込んで表示
    public void LoadConstellation(SaveConstellationData savedConstellationData)
    {
        Initialize();
        SavedConstellationData = savedConstellationData;
        ST_Constellation[] targets = savedConstellationData.constellations;
        Lines = savedConstellationData.lines;
        Array.Resize<GameObject>(ref Targets, targets.Length);

        int index = 0;
        foreach (ST_Constellation i in targets)
        {
            Targets[index] = Instantiate(TargetPrefab, i.position, Quaternion.identity);

            index++;
        }

        LineRenderers = new LineRenderer[1];
        LineRenderers[0] = Instantiate(LineRendererPrefab.gameObject).GetComponent<LineRenderer>();
        LineRenderers[0].startWidth = LineWidth;
        LineRenderers[0].endWidth = LineWidth;
        int preEndTargetIndex = -1;
        int _lineRendererIndex = 0;
        int pointIndex = 0;
        foreach (Line i in Lines)
        {
            if (preEndTargetIndex != -1)
            {
                //前の線の終点と現在の線の始点が同じなら繋がっている
                if (preEndTargetIndex == i.startTargetIndex)
                {
                    LineRenderers[_lineRendererIndex].positionCount++;
                    LineRenderers[_lineRendererIndex].SetPosition(pointIndex, i.start);
                    pointIndex++;
                    LineRenderers[_lineRendererIndex].positionCount++;
                    LineRenderers[_lineRendererIndex].SetPosition(pointIndex, i.end);
                    pointIndex++;

                    preEndTargetIndex = i.endTargetIndex;
                }
                //繋がっていなかった
                else
                {
                    pointIndex = 0;
                    _lineRendererIndex++;

                    Array.Resize<LineRenderer>(ref LineRenderers, _lineRendererIndex + 1);
                    LineRenderers[_lineRendererIndex] = new LineRenderer();
                    LineRenderers[_lineRendererIndex] = Instantiate(LineRendererPrefab.gameObject).GetComponent<LineRenderer>();
                    LineRenderers[_lineRendererIndex].startWidth = LineWidth;
                    LineRenderers[_lineRendererIndex].endWidth = LineWidth;

                    LineRenderers[_lineRendererIndex].positionCount++;
                    LineRenderers[_lineRendererIndex].SetPosition(pointIndex, i.start);
                    pointIndex++;
                    LineRenderers[_lineRendererIndex].positionCount++;
                    LineRenderers[_lineRendererIndex].SetPosition(pointIndex, i.end);
                    pointIndex++;

                    preEndTargetIndex = i.endTargetIndex;
                }
            }
            else
            {
                //初回時
                LineRenderers[_lineRendererIndex].positionCount++;
                LineRenderers[_lineRendererIndex].SetPosition(pointIndex, i.start);
                pointIndex++;
                LineRenderers[_lineRendererIndex].positionCount++;
                LineRenderers[_lineRendererIndex].SetPosition(pointIndex, i.end);
                pointIndex++;
               
                preEndTargetIndex = i.endTargetIndex;
            }
        }
    }

    //セーブデータから読み込んだ星座かどうか
    public bool IsSavedData()
    {
        if (SavedConstellationData == null)
        {
            return false;
        }
        return true;
    }
}
