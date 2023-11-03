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
    private ST_Constellation[] Constellations;
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

    // Start is called before the first frame update
    void Start()
    {
        Targets = new GameObject[0];
        LineRenderers = new LineRenderer[1];
        LineRenderers[0] = Instantiate(LineRendererPrefab.gameObject).GetComponent<LineRenderer>();
        LineRenderers[0].positionCount = 0;
        LineRenderers[0].startWidth = LineWidth;
        LineRenderers[0].endWidth = LineWidth;
        Lines = new Line[1];

        DeterminationButton.interactable = false;
    }

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
                break;
            case 1:
                // 環状に配置するモード
                SaveButton.interactable = false;
                PutTargetInLineButton.interactable = false;
                PutTargetInLoopButton.interactable = false;
                DeterminationButton.interactable = true;
                break;
            case 2:
                // 決定ボタン押された
                SaveButton.interactable = true;
                PutTargetInLoopButton.interactable = true;
                PutTargetInLineButton.interactable = true;
                DeterminationButton.interactable = false;

                

                LineRendererIndex++;
                LineRendererPointIndex = 0;
                //LineRendererの配列のサイズを増やす
                Array.Resize<LineRenderer>(ref LineRenderers, LineRendererIndex + 1);
                LineRenderers[LineRendererIndex] = new LineRenderer();
                LineRenderers[LineRendererIndex] = Instantiate(LineRendererPrefab.gameObject).GetComponent<LineRenderer>();
                LineRenderers[LineRendererIndex].positionCount = 0;
                LineRenderers[LineRendererIndex].startWidth = LineWidth;
                LineRenderers[LineRendererIndex].endWidth = LineWidth;
                break;
            default:
                break;
        }
        
       
       
    }
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
        LineRenderers[LineRendererIndex].positionCount++;
        pos.z += 5f;
        LineRenderers[LineRendererIndex].SetPosition(LineRendererPointIndex, pos);
        LineRendererPointIndex++;
        // 点が２つ以上の時
        if (LineRenderers[LineRendererIndex].positionCount > 1)
        {
            Array.Resize<Line>(ref Lines, LineIndex + 1);
            Vector3 start = LineRenderers[LineRendererIndex].GetPosition(LineRenderers[LineRendererIndex].positionCount - 2);
            Vector3 end = LineRenderers[LineRendererIndex].GetPosition(LineRenderers[LineRendererIndex].positionCount - 1);
            int startTargetIndex = Targets.Length - 2;
            int endTargetIndex = Targets.Length - 1;
            int index = LineRenderers[LineRendererIndex].positionCount - 2;
            Lines[LineIndex] = new Line();
            Lines[LineIndex].Create(start, end, startTargetIndex, endTargetIndex);
            LineIndex++;
        }
    }

    //はめ込む型を環状に設置する
    private void PutTargetInLoop(Vector3 pos)
    {
        
    }

    // 

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
        }

        InputField input = InputName.GetComponent<InputField>();
        //入力された星座の名前
        string name = input.text;

        saveConstellationData.Create(year, month, day, 0, name, constellations, Lines);


        return saveConstellationData;
    }
}
