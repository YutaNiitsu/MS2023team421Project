using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static SaveConstellationDIsplayScript;

public class CreateConstellationManager : MonoBehaviour
{
    //読み込んだ星座データ一覧を表示選択するためのスクロール
    public GameObject ConstellationListDisplay;
    //スクロールにアイテムを追加するときにつかう
    public GameObject ScrollViewportContents;
    //選択に使うボタンプレハブ
    public GameObject SelectButton;
    //選択した星座に決定するボタン
    public Button DeterminationButton;
    //ボタン
    public Button NewButton;
    public Button PutTargeButton;
    public Button PutDeterminationButton;
    public Button SaveButton;
    public Button LoadButton;
    public Button DeleteSavedDataButton;
    //セーブ画面
    public SaveConstellationDIsplayScript saveConstellationDIsplay;

    private SelectButtonScript[] SelectConstellationButtons;

    // 星座データ
    private SaveConstellationData[] ConstellationDatas;
    private ConstellationSaveManager constellationSaveManager;
    private CreateConstellationScript createConstellationScript;


    // Start is called before the first frame update
    void Start()
    {
        // 星座データの読み込み
        ConstellationDatas = GetComponent<ConstellationLoadManager>().LoadData();

        createConstellationScript = GetComponent<CreateConstellationScript>();
        constellationSaveManager = GetComponent<ConstellationSaveManager>();

        SelectConstellationButtons = new SelectButtonScript[0];

        DeterminationButton.interactable = false;
        SetButtonInteractable(true, true, false, true, true, true);
    }
    
    // Update is called once per frame
    void Update()
    {
        //右クリックで設置
        if (Input.GetMouseButtonDown(1))
        {
            // 配置ボタンが有効（押されていない時）になっていたら実行しない
            if (PutTargeButton.interactable)
                return;
            // 画面上に配置されたボタンが押された時は実行しない
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }
            createConstellationScript.DeleteTarget();
        }
        //左クリックで設置
        if (Input.GetMouseButtonDown(0))
        {

            // 配置ボタンが有効（押されていない時）になっていたら実行しない
            if (PutTargeButton.interactable)
                return;

            // 画面上に配置されたボタンが押された時は実行しない
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }
            //マウス座標の取得
            Vector3 mousePos = Input.mousePosition;
            //スクリーン座標をワールド座標に変換
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 10f));
            createConstellationScript.PutTarget(worldPos);
        }
    }

    //新規作成
    public void New()
    {
        createConstellationScript.Initialize();
        SetButtonInteractable(true, true, false, true, true, true);
    }

    //配置ボタン押された時
    public void ClickPutTargetButton()
    {
        SetButtonInteractable(false, false, true, false, false, false);
    }

    //配置決定ボタン押された時
    public void ClickPutTargetDeterminationButton()
    {
        SetButtonInteractable(true, true, false, true, true, true);
    }

    // 星座のデータを新規で保存
    public void OnSaveNewData()
    {
        //IDを決める
        uint id = 0;
        for (; ; id++)
        {
            bool success = true;
            foreach (SaveConstellationData i in ConstellationDatas)
            {
                if (i.id == id)
                {
                    // 被っていたら失敗
                    success = false;
                }
            }
            if (success)
            {
                //被っていなかったら終了
                break;
            }
        }

        //被っていなかった数字をIDに決定
        Array.Resize<SaveConstellationData>(ref ConstellationDatas, ConstellationDatas.Length + 1);
        ConstellationDatas[ConstellationDatas.Length - 1] = createConstellationScript.CreateSaveData();
        ConstellationDatas[ConstellationDatas.Length - 1].SetID(id);
        //データをセーブ
        constellationSaveManager.OnSaveNewData(ConstellationDatas);
        //セーブ画面を消す
        saveConstellationDIsplay.gameObject.SetActive(false);
    }
    // 星座のデータを上書き保存
    public void OnSaveOverwrite()
    {
        SaveConstellationData data = createConstellationScript.CreateSaveData();
        //同じIDを探す
        int index = 0;
        foreach (SaveConstellationData i in ConstellationDatas)
        {
            if (i.id == data.id)
            {
                break;
            }
            index++;
        }
        ConstellationDatas[index] = data;

        constellationSaveManager.OnSaveNewData(ConstellationDatas);
        //セーブ画面を消す
        saveConstellationDIsplay.gameObject.SetActive(false);
        SetButtonInteractable(true, true, false, true, true, true);
    }
    // セーブをキャンセル
    public void OnSaveCancel()
    {
        //セーブ画面を消す
        saveConstellationDIsplay.gameObject.SetActive(false);
        SetButtonInteractable(true, true, false, true, true, true);
    }
    // 星座のデータを選択して表示
    private void DisplayList()
    {
        DeterminationButton.interactable = false;
        int cnt = ScrollViewportContents.transform.childCount;
        // コンテンツにアイテムが残っていたら削除
        for (int i = 0; i < cnt; i++)
        {
            Destroy(ScrollViewportContents.transform.GetChild(i).gameObject);
        }
       
        //一覧スクロールを表示
        ConstellationListDisplay.SetActive(true);
        //スクロールにアイテムを追加
        SelectConstellationButtons = new SelectButtonScript[ConstellationDatas.Length];
        int index = 0;
        foreach (SaveConstellationData i in ConstellationDatas)
        {
            GameObject button = Instantiate(SelectButton);
            button.transform.parent = ScrollViewportContents.transform;
            SelectConstellationButtons[index] = button.GetComponent<SelectButtonScript>();
            index++;
        }
    }

    //選択ボタン押した時の処理
    public void ClickSelectButton(UnityAction clickAction)
    {
        //追加した処理を全部削除
        DeterminationButton.onClick.RemoveAllListeners();
        //決定ボタンを有効にする
        DeterminationButton.interactable = true;
        //決定ボタン押したときに処理する関数を追加
        DeterminationButton.onClick.AddListener(clickAction);
    }

    //選択した星座で決定して読み込む処理
    private void LoadSelectConstellation(SaveConstellationData data)
    {
        createConstellationScript.LoadConstellation(data);
        //一覧スクロールを非表示
        ConstellationListDisplay.SetActive(false);
        SetButtonInteractable(true, true, false, true, true, true);
    }

    //選択した星座で決定して削除する処理
    private void DeleteSelectConstellation(uint deleteID)
    {
        SaveConstellationData[] newDatas = new SaveConstellationData[ConstellationDatas.Length - 1];
        int index = 0;
        //データ無くなっていたら飛ばす
        if (newDatas.Length != 0)
        {
            foreach (SaveConstellationData i in ConstellationDatas)
            {
                if (i.id != deleteID)
                {
                    //削除する要素番号以外を代入
                    newDatas[index] = i;
                    index++;
                }
            }
        }

        Array.Resize<SaveConstellationData>(ref ConstellationDatas, ConstellationDatas.Length - 1);
        ConstellationDatas = newDatas;
        //セーブする
        //newDatas = new SaveConstellationData[0];
        constellationSaveManager.OnSaveNewData(newDatas);
        //一覧スクロールを非表示
        ConstellationListDisplay.SetActive(false);
        SetButtonInteractable(true, true, false, true, true, true);
    }
    //ロードボタン押したときの処理
    public void ClickLoadButton()
    {
        SetButtonInteractable(false, false, false, false, false, false);
        DisplayList();

        int index = 0;
        foreach (SaveConstellationData i in ConstellationDatas)
        {
            SelectConstellationButtons[index].SetText(i.name, 24);
            // ボタンクリックしたときに実行する関数を追加
            SelectConstellationButtons[index].AddClickAction(() => ClickSelectButton(() => LoadSelectConstellation(i)));
            index++;
        }
    }

    //セーブボタン押したときの処理
    public void ClickSaveButton()
    {
        SetButtonInteractable(false, false, false, false, false, false);
        saveConstellationDIsplay.gameObject.SetActive(true);
        if (createConstellationScript.IsSavedData())
            saveConstellationDIsplay.SetVisibility(SaveConstellationType.SavedData);
        else
            saveConstellationDIsplay.SetVisibility(SaveConstellationType.NewData);

    }

    //セーブデータを削除ボタン押した時の処理
    public void DeleteSavedData()
    {
        SetButtonInteractable(false, false, false, false, false, false);
        DisplayList();

        int index = 0;
        foreach (SaveConstellationData i in ConstellationDatas)
        {
            SelectConstellationButtons[index].SetText(i.name, 24);
            // ボタンクリックしたときに実行する関数を追加
            SelectConstellationButtons[index].AddClickAction(() => ClickSelectButton(() => DeleteSelectConstellation(i.id)));
            index++;
        }
        
    }

    //セーブデータ一覧表示をキャンセル
    public void DisplayListCancel()
    {
        //一覧スクロールを非表示
        ConstellationListDisplay.SetActive(false);
        SetButtonInteractable(true, true, false, true, true, true);
    }

    public void LoadSprite(string path)
    {
        try
        {
            var rawData = System.IO.File.ReadAllBytes(path);
            Texture2D texture2D = new Texture2D(0, 0);
            texture2D.LoadImage(rawData);
            var sprite = Sprite.Create(texture2D, new Rect(0f, 0f, texture2D.width, texture2D.height),
                new Vector2(0.5f, 0.5f), 100f);
            //return sprite;
        }
        catch (Exception e)
        {
            //return null;
        }
    }
    //ボタンの有効無効を切り替え
    private void SetButtonInteractable(bool newButton,
    bool putTargeButton,
    bool putDeterminationButton,
    bool saveButton,
    bool loadButton,
    bool deleteSavedDataButton)
    {
        NewButton.interactable = newButton;
        PutTargeButton.interactable = putTargeButton;
        PutDeterminationButton.interactable = putDeterminationButton;
        SaveButton.interactable = saveButton;
        LoadButton.interactable = loadButton;
        DeleteSavedDataButton.interactable = deleteSavedDataButton;
    }
}
