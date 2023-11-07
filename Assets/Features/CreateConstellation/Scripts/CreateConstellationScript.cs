using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEditor.PlayerSettings;
using static UnityEngine.GraphicsBuffer;

public class ConstellationLine : MonoBehaviour
{
    public Line line { get; protected set; }
    public LineRenderer lineRenderer { get; protected set; }

    private ConstellationLine() { }
    public ConstellationLine(LineRenderer prefab, Vector3 _start, Vector3 _end, int _startTargetIndex, int _endTargetIndex, float _lineWidth) 
    {
        lineRenderer = Instantiate(prefab.gameObject).GetComponent<LineRenderer>();
        line = new Line();
        line.Create(_start, _end, _startTargetIndex, _endTargetIndex);

        lineRenderer.startWidth = _lineWidth;
        lineRenderer.endWidth = _lineWidth;
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, _start);
        lineRenderer.SetPosition(1, _end);
    }

    public void Destroy()
    {
        Destroy(lineRenderer.gameObject);
    }
}

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
    public Button PutTargeButton;
    public Button DeterminationButton;
    //入力欄
    public GameObject InputName;
    // 設置されたはめ込む型
    private GameObject[] Targets;
    //カーソルで選択されたはめ込む型の要素番号
    private int SelectedTargetIndex;
    private GameObject SelectedTarget;
    // 星をつなぐ線
    private ConstellationLine[] ConstellationLines;
    private int ConstellationLinesIndex;

    //セーブデータから読み込んだデータ
    private SaveConstellationData SavedConstellationData = null;

    // Start is called before the first frame update
    void Start()
    {
        Targets = new GameObject[0];
        ConstellationLines = new ConstellationLine[0];
        ConstellationLinesIndex = 0;

        DeterminationButton.interactable = false;
        SelectedTarget = null;
        SelectedTargetIndex = -1;

    }

    //配置ボタン押された時
    public void ClickPutTargetButton()
    {
        SaveButton.interactable = false;
        PutTargeButton.interactable = false;
        DeterminationButton.interactable = true;
    }

    //配置決定ボタン押された時
    public void ClickPutTargetDeterminationButton()
    {
        SaveButton.interactable = true;
        PutTargeButton.interactable = true;
        DeterminationButton.interactable = false;
    }
  
    //はめ込む型を設置する
    public void PutTarget(Vector3 pos)
    {
        // 配置ボタンが有効（押されていない時）になっていたら実行しない
        if (PutTargeButton.interactable)
            return;

        //前に選択されていたはめ込む型の要素番号を保存
        int preSelectedTargetIndex = SelectedTargetIndex;

        if (CheckCursorHitTarget())
        {
            //カーソルが既に設置されたはめ込む型と当たっていたとき
            //選択されたはめ込む型が無かったら線のインスタンスは生成しない
            if (SelectedTargetIndex == -1)
            {
                return;
            }
            //前に選択されていたはめ込む型がなかったら線のインスタンスは生成しない
            if (preSelectedTargetIndex == -1)
            {
                return;
            }
            // カーソルが当たったはめ込む型に線を繋げる

            foreach (ConstellationLine i in ConstellationLines)
            {
                if ((i.line.startTargetIndex == preSelectedTargetIndex && i.line.endTargetIndex == SelectedTargetIndex)
                    || (i.line.startTargetIndex == SelectedTargetIndex && i.line.endTargetIndex == preSelectedTargetIndex))
                {
                    //既に線が生成されていたら実行しない
                    return;
                }
            }

            //始点
            Vector3 start = Targets[preSelectedTargetIndex].transform.position;
            start.z += 5f;
            //終点
            Vector3 end = Targets[SelectedTargetIndex].transform.position;
            end.z += 5f;
            //始点のはめ込む型の要素番号
            int startTargetIndex = preSelectedTargetIndex;
            //終点のはめ込む型の要素番号
            int endTargetIndex = SelectedTargetIndex;
            //線のインスタンス生成
            CreateLine(start, end, startTargetIndex, endTargetIndex);
        }
        else
        {
            //はめ込む型を配置
            Array.Resize<GameObject>(ref Targets, Targets.Length + 1);
            Targets[Targets.Length - 1] = Instantiate(TargetPrefab, pos, Quaternion.identity);
            Targets[Targets.Length - 1].GetComponent<TargetInCreateModeScript>().SetIndex(Targets.Length - 1);


            //選択されたはめ込む型が無かったら線のインスタンスは生成しない
            if (SelectedTargetIndex == -1)
            {
                return;
            }

            // 線を配置
            //始点
            Vector3 start = Targets[SelectedTargetIndex].transform.position;
            start.z += 5f;
            //終点
            Vector3 end = pos;
            end.z += 5f;
            //始点のはめ込む型の要素番号
            int startTargetIndex = SelectedTargetIndex;
            //終点のはめ込む型の要素番号
            int endTargetIndex = Targets.Length - 1;
            //線のインスタンス生成
            CreateLine(start, end, startTargetIndex, endTargetIndex);
        }
    }

    //はめ込む型を削除する
    public void DeleteTarget()
    {
        // 配置ボタンが有効（押されていない時）になっていたら実行しない
        if (PutTargeButton.interactable)
            return;

        if (CheckCursorHitTarget())
        {
            //未選択状態だったら実行しない
            if (SelectedTargetIndex == -1)
            {
                return;
            }

            //選択されたはめ込む型を含む線を全て消す
            ConstellationLine[] clTemp = new ConstellationLine[ConstellationLines.Length];
            int index = 0;
            foreach (ConstellationLine i in ConstellationLines)
            {
                if (i.line.startTargetIndex == SelectedTargetIndex 
                    || i.line.endTargetIndex == SelectedTargetIndex)
                {
                    i.Destroy();
                }
                else
                {
                    clTemp[index] = i;
                    index++;
                }
            }
            //サイズ変更
            Array.Resize<ConstellationLine>(ref ConstellationLines, index);
            for (int i = 0; i < ConstellationLines.Length; i++)
            {
                ConstellationLines[i] = clTemp[i];
            }

            //選択されたはめ込む型を削除する
            Destroy(Targets[SelectedTargetIndex]);
            GameObject[] targetTemp = new GameObject[Targets.Length];
            index = 0;
            foreach (GameObject i in Targets)
            {
                targetTemp[index] = i;
            }

            Array.Resize<GameObject>(ref Targets, Targets.Length - 1);
            int newIndex = 0;
            for (int oldIndex = 0; oldIndex < targetTemp.Length; oldIndex++)
            {
                //選択されたはめ込む型の要素番号と一致したら飛ばす
                if (index != SelectedTargetIndex)
                {
                    Targets[newIndex] = targetTemp[oldIndex];
                    Targets[newIndex].GetComponent<TargetInCreateModeScript>().SetIndex(newIndex);
                    foreach (ConstellationLine i in ConstellationLines)
                    {
                        //線情報の要素番号を更新
                        if (i.line.startTargetIndex == oldIndex)
                        {
                            i.line.SetIndex(newIndex, i.line.endTargetIndex);
                        }
                        if (i.line.endTargetIndex == oldIndex)
                        {
                            i.line.SetIndex(i.line.startTargetIndex, newIndex);
                        }
                    }

                    newIndex++;
                }
            }

            //未選択状態にする
            SelectedTargetIndex = -1;
        }
    }

    //線作成
    private void CreateLine(Vector3 _start, Vector3 _end, int _startTargetIndex, int _endTargetIndex)
    {
        Array.Resize<ConstellationLine>(ref ConstellationLines, ConstellationLinesIndex + 1);
        ConstellationLines[ConstellationLinesIndex] 
            = new ConstellationLine(LineRendererPrefab, _start, _end, _startTargetIndex, _endTargetIndex, LineWidth);
        ConstellationLinesIndex++;
    }

    // 初期化
    public void Initialize()
    {
        //インスタンスを削除
        foreach (GameObject i in Targets)
        {
            Destroy(i);
        }
        foreach (ConstellationLine i in ConstellationLines)
        {
            i.Destroy();
        }
        ConstellationLines = new ConstellationLine[0];
        ConstellationLinesIndex = 0;
        Targets = new GameObject[0];

        DeterminationButton.interactable = false;
        SavedConstellationData = null;
        SelectedTarget = null;
        SelectedTargetIndex = -1;
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

        Line[] lines = new Line[ConstellationLines.Length];

        index = 0;
        foreach (ConstellationLine i in ConstellationLines)
        {
            lines[index] = i.line;
            index++;
        }

        saveConstellationData.Create(year, month, day, id, name, constellations, lines);


        return saveConstellationData;
    }

    //セーブデータから読み込んで表示
    public void LoadConstellation(SaveConstellationData savedConstellationData)
    {
        Initialize();
        SavedConstellationData = savedConstellationData;
        ST_Constellation[] targets = savedConstellationData.constellations;
        Line[] lines = savedConstellationData.lines;
        Array.Resize<GameObject>(ref Targets, targets.Length);

        //はめ込む型をインスタンス生成
        int index = 0;
        foreach (ST_Constellation i in targets)
        {
            Targets[index] = Instantiate(TargetPrefab, i.position, Quaternion.identity);
            Targets[index].GetComponent<TargetInCreateModeScript>().SetIndex(index);
            index++;
        }

        //線を配置
        //線のインスタンス生成
        foreach (Line i in lines)
        {
            //線のインスタンス生成
            CreateLine(i.start, i.end, i.startTargetIndex, i.endTargetIndex);
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
    
    //カーソルとはめ込む型の当たり判定
    private bool CheckCursorHitTarget()
    {
        //レイキャスト
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

        if (hit.collider != null)
        {
            //カーソルとはめ込む型が当たっていた
            if (hit.collider.CompareTag("Target"))
            {
                TargetInCreateModeScript targetScript = hit.collider.gameObject.GetComponent<TargetInCreateModeScript>();
               
                if (SelectedTargetIndex == -1)
                {
                    //何も選択されてなかった
                    SelectedTargetIndex = targetScript.Index;
                    targetScript.SetIsSelected(true);
                }
                else if (SelectedTargetIndex != targetScript.Index)
                {
                    //前クリックしたものと違うものだった
                    //前クリックしたものを未選択状態にする
                    Targets[SelectedTargetIndex].GetComponent<TargetInCreateModeScript>().SetIsSelected(false);
                    SelectedTargetIndex = targetScript.Index;
                    targetScript.SetIsSelected(true);
                }
                else if (SelectedTargetIndex == targetScript.Index)
                {
                    //前クリックしたものと同じだったら選択解除
                    targetScript.SetIsSelected(false);
                    SelectedTargetIndex = -1;
                    
                }
                return true;
            }
        }

        return false;
    }
}
