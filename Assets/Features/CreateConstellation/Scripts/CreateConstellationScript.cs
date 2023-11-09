using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
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
    public GameObject LineObject { get; protected set; }

    private ConstellationLine() { }
    public ConstellationLine(GameObject prefab, Vector3 _start, Vector3 _end, int _startTargetIndex, int _endTargetIndex, float _lineWidth) 
    {
        LineObject = Instantiate(prefab);
        line = new Line();
        line.Create(_start, _end, _startTargetIndex, _endTargetIndex);

        LineRenderer lineRenderer = LineObject.GetComponent<LineRenderer>();
        lineRenderer.startWidth = _lineWidth;
        lineRenderer.endWidth = _lineWidth;
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, _start);
        lineRenderer.SetPosition(1, _end);
        Debug.Log(LineObject);
    }

    public void Destroy()
    {
        Debug.Log(LineObject);
        if (LineObject != null)
        {
            Destroy(LineObject);
            
        }
        
    }
}

public class CreateConstellationScript : MonoBehaviour
{
    //星をつなぐ線の太さ
    public float LineWidth;
    // はめ込む型のプレハブ
    public GameObject TargetPrefab;
    // 星をつなぐ線のプレハブ
    public GameObject LineRendererPrefab;
    //ボタン
    public Button SaveButton;
    public Button PutTargeButton;
    public Button DeterminationButton;
    //入力欄
    public GameObject InputName;
    // 設置されたはめ込む型
    private List<TargetInCreateModeScript> Targets;
    private int TargetKey;
    //カーソルで選択されたはめ込む型
    private TargetInCreateModeScript SelectedTarget;
    // 星をつなぐ線
    private List<ConstellationLine> ConstellationLines;

    //セーブデータから読み込んだデータ
    private SaveConstellationData SavedConstellationData = null;

    // Start is called before the first frame update
    void Start()
    {
        Targets = new List<TargetInCreateModeScript>();
        ConstellationLines = new List<ConstellationLine>();

        DeterminationButton.interactable = false;
        SelectedTarget = null;
        TargetKey = 0;
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
        TargetInCreateModeScript preSelectedTarget= SelectedTarget;

        if (CheckCursorHitTarget())
        {
            // カーソルが当たったはめ込む型に線を繋げる
            //カーソルが既に設置されたはめ込む型と当たっていたとき
            //選択されたはめ込む型が無かったら線のインスタンスは生成しない
            if (SelectedTarget == null)
            {
                return;
            }
            //前に選択されていたはめ込む型がなかったら線のインスタンスは生成しない
            if (preSelectedTarget == null)
            {
                return;
            }

            
            foreach (ConstellationLine i in ConstellationLines)
            {
                if ((i.line.startTargetIndex == preSelectedTarget.Key && i.line.endTargetIndex == SelectedTarget.Key)
                    || (i.line.startTargetIndex == SelectedTarget.Key && i.line.endTargetIndex == preSelectedTarget.Key))
                {
                    //既に線が生成されていたら実行しない
                    return;
                }
            }

            //始点
            Vector3 start = preSelectedTarget.gameObject.transform.position;
            start.z += 5f;
            //終点
            Vector3 end = SelectedTarget.gameObject.transform.position;
            end.z += 5f;
            //始点のはめ込む型の要素番号
            int startTargetKey = preSelectedTarget.Key;
            //終点のはめ込む型の要素番号
            int endTargetKey = SelectedTarget.Key;
            //線のインスタンス生成
            CreateLine(start, end, startTargetKey, endTargetKey);
        }
        else
        {
            //はめ込む型を配置
            TargetInCreateModeScript temp = Instantiate(TargetPrefab, pos, Quaternion.identity).GetComponent<TargetInCreateModeScript>();
            temp.SetKey(TargetKey);
            Targets.Add(temp);

            
            //終点のはめ込む型の要素番号
            int endTargetIndex = TargetKey;
            //はめ込む型の要素番号を進める
            TargetKey++;

            //選択されたはめ込む型が無かったら線のインスタンスは生成しない
            if (SelectedTarget == null)
            {
                return;
            }
            //始点のはめ込む型の要素番号
            int startTargetIndex = SelectedTarget.Key;

            // 線を配置
            //始点
            Vector3 start = SelectedTarget.gameObject.transform.position;
            start.z += 5f;
            //終点
            Vector3 end = pos;
            end.z += 5f;
            
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
            if (SelectedTarget == null)
            {
                return;
            }

            //選択されたはめ込む型を含む線を全て消す
            {
                //リストから削除しない線を一時的に保存するための配列
                ConstellationLine[] temp = new ConstellationLine[ConstellationLines.Count];
                int index = 0;
                foreach (ConstellationLine i in ConstellationLines)
                {
                    if ((i.line.startTargetIndex != SelectedTarget.Key)
                        && (i.line.endTargetIndex != SelectedTarget.Key))
                    {
                        //リストに残す
                        temp[index] = i;
                        index++;
                    }
                    else
                    {
                        //削除する
                        i.Destroy();
                    }
                }
                //リストから削除する線を取り除く
                ConstellationLines = new List<ConstellationLine>();
                for (int i = 0; i < index; i++)
                {
                    ConstellationLines.Add(temp[i]);
                }
            }

            //選択されたはめ込む型を削除する
            Targets.Remove(SelectedTarget);
            Destroy(SelectedTarget.gameObject);
            //未選択状態にする
            SelectedTarget = null;
        }
    }

    //線作成
    private void CreateLine(Vector3 _start, Vector3 _end, int _startTargetIndex, int _endTargetIndex)
    {
        ConstellationLine temp = new ConstellationLine(LineRendererPrefab, _start, _end, _startTargetIndex, _endTargetIndex, LineWidth);
        ConstellationLines.Add(temp);
    }

    // 初期化
    public void Initialize()
    {
        //インスタンスを削除
        foreach (TargetInCreateModeScript i in Targets)
        {
            Destroy(i.gameObject);
        }
        foreach (ConstellationLine i in ConstellationLines)
        {
            i.Destroy();
        }
        ConstellationLines = new List<ConstellationLine>();
        Targets = new List<TargetInCreateModeScript>();

        DeterminationButton.interactable = false;
        SavedConstellationData = null;
        SelectedTarget = null;
        TargetKey = 0;
    }

    // セーブデータ作成
    public SaveConstellationData CreateSaveData()
    {
        SaveConstellationData saveConstellationData = new SaveConstellationData();
        int year = DateTime.Now.Date.Year;
        int month = DateTime.Now.Date.Month;
        int day = DateTime.Now.Date.Day;

        ST_Constellation[] constellations = new ST_Constellation[Targets.Count];
        int index = 0;
        foreach(TargetInCreateModeScript i in Targets)
        {
            constellations[index].position = i.gameObject.transform.position;
            constellations[index].Index = i.Key;
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

        Line[] lines = new Line[ConstellationLines.Count];

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

        //はめ込む型をインスタンス生成
        int index = 0;
        foreach (ST_Constellation i in targets)
        {
            TargetInCreateModeScript temp = Instantiate(TargetPrefab, i.position, Quaternion.identity).GetComponent<TargetInCreateModeScript>();
            temp.SetKey(i.Index);
            Targets.Add(temp);
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
                if (targetScript == null)
                    return false;

                if (SelectedTarget == null)
                {
                    //何も選択されてなかった
                    SelectedTarget = targetScript;
                    targetScript.SetIsSelected(true);
                }
                else if (SelectedTarget.Key != targetScript.Key)
                {
                    //前クリックしたものと違うものだった
                    //前クリックしたものを未選択状態にする
                    SelectedTarget.SetIsSelected(false);
                    SelectedTarget = targetScript;
                    targetScript.SetIsSelected(true);
                }
                else if (SelectedTarget.Key == targetScript.Key)
                {
                    //前クリックしたものと同じだったら選択解除
                    targetScript.SetIsSelected(false);
                    SelectedTarget = null;
                    
                }
                else
                {
                    return false;
                }
                return true;
            }
        }

        return false;
    }

    
}
