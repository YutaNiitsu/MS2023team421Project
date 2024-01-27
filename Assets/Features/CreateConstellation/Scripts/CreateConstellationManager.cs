using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static SaveConstellationDIsplayScript;

public class CreateConstellationManager : MonoBehaviour
{
    [Header("星座データのファイル名")]
    public string SavedFileName;

    //読み込んだ星座データ一覧を表示選択するためのスクロール
    public GameObject ConstellationListDisplay;
    //スクロールにアイテムを追加するときにつかう
    public GameObject ScrollViewportContents;
    //選択に使うボタンプレハブ
    public GameObject SelectButton;
    //選択した星座に決定するボタン
    public Button DeterminationButton;
    //キャンセルボタン
    public Button CanselButton;
    //ボタン
    public Button NewButton;
    public Button PutTargeButton;
    public Button PutDeterminationButton;
    public Button SaveButton;
    public Button LoadButton;
    public Button DeleteSavedDataButton;
    public Button TitleButton;
    //セーブ画面
    public SaveConstellationDIsplayScript SaveConstellationDisplay;
    //一覧表示に使うボタンのプレハブ
    private SelectButtonScript[] SelectConstellationButtons;

    // 星座データ
    private SaveConstellationData[] ConstellationDatas;
    private ConstellationSaveManager constellationSaveManager;
    private CreateConstellationScript createConstellationScript;

    private InputField SaveInputField;

    // Start is called before the first frame update
    void Start()
    {
        // 星座データの読み込み
        ConstellationDatas = GetComponent<ConstellationLoadManager>().LoadData(SavedFileName);

        if (ConstellationDatas == null)
        {
            ConstellationDatas = new SaveConstellationData[0];
        }

        createConstellationScript = GetComponent<CreateConstellationScript>();
        constellationSaveManager = ConstellationSaveManager.instance;

        SelectConstellationButtons = new SelectButtonScript[0];

        DeterminationButton.interactable = false;
        SetButtonInteractable(true, true, false, true, true, true, true);

        //フォーカスするボタン
        EventSystem.current.SetSelectedGameObject(NewButton.gameObject);
    }
    
    // Update is called once per frame
    void Update()
    {
        createConstellationScript.MoveCursor(PutDeterminationButton.interactable);

        //セーブ画面開いていた
        if (SaveConstellationDisplay.gameObject.activeSelf)
        {
            //createConstellationScript.MoveCursor(true);
            //            if (SaveInputField != null)
            //            {
            //                if (SaveInputField.isFocused && Input.GetButtonDown("Debug Validate"))
            //                {
            //                    SaveInputField.ActivateInputField();
            //                    //仮想キーボード表示
            //#if UNITY_STANDALONE_WIN
            //                    System.Diagnostics.Process.Start("osk.exe");

            //#endif
            //                }
            //            }
            return;
        }

        //星座データ一覧が表示されちたら実行しない
        if (ConstellationListDisplay.activeSelf)
            return;
        // 配置ボタンが有効（押されていない時）になっていたら実行しない
        if (PutTargeButton.interactable)
            return;
        // 画面上に配置されたボタンが押された時は実行しない
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        //右クリックで削除
        if (Input.GetMouseButtonDown(1) || Input.GetButtonDown("delete"))
        {
            createConstellationScript.DeleteTarget();
        }
        //左クリックで設置
        if (Input.GetMouseButtonDown(0) || Input.GetButtonDown("put"))
        {
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
        SetButtonInteractable(true, true, false, true, true, true, true);
    }

    //配置ボタン押された時
    public void ClickPutTargetButton()
    {
        SetButtonInteractable(false, false, true, false, false, false, false);
    }

    //配置決定ボタン押された時
    public void ClickPutTargetDeterminationButton()
    {
        SetButtonInteractable(true, true, false, true, true, true, true);
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
        constellationSaveManager.OnSaveNewData(ConstellationDatas, SavedFileName);
        //セーブ画面を消す
        SaveConstellationDisplay.gameObject.SetActive(false);
        SetButtonInteractable(true, true, false, true, true, true, true);
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

        constellationSaveManager.OnSaveNewData(ConstellationDatas, SavedFileName);
        //セーブ画面を消す
        SaveConstellationDisplay.gameObject.SetActive(false);
        SetButtonInteractable(true, true, false, true, true, true, true);
    }
    // セーブをキャンセル
    public void OnSaveCancel()
    {
        //セーブ画面を消す
        SaveConstellationDisplay.gameObject.SetActive(false);
        SetButtonInteractable(true, true, false, true, true, true, true);
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
        EventSystem.current.SetSelectedGameObject(DeterminationButton.gameObject);
        //決定ボタン押したときに処理する関数を追加
        DeterminationButton.onClick.AddListener(clickAction);
    }

    //選択した星座で決定して読み込む処理
    private void LoadSelectConstellation(SaveConstellationData data)
    {
        createConstellationScript.LoadConstellation(data);
        //一覧スクロールを非表示
        ConstellationListDisplay.SetActive(false);
        SetButtonInteractable(true, true, false, true, true, true, true);
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
        constellationSaveManager.OnSaveNewData(newDatas, SavedFileName);
        //一覧スクロールを非表示
        ConstellationListDisplay.SetActive(false);
        SetButtonInteractable(true, true, false, true, true, true, true);
    }
    //ロードボタン押したときの処理
    public void ClickLoadButton()
    {
        SetButtonInteractable(false, false, false, false, false, false, false);
        DisplayList();

        //フォーカスするボタン
        if (SelectConstellationButtons.Length > 0)
            EventSystem.current.SetSelectedGameObject(SelectConstellationButtons[0].gameObject);
        else
            EventSystem.current.SetSelectedGameObject(CanselButton.gameObject);

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
        SetButtonInteractable(false, false, false, false, false, false, false);
        SaveConstellationDisplay.gameObject.SetActive(true);
        //フォーカスするボタン
        EventSystem.current.SetSelectedGameObject(/*createConstellationScript.InputField*/SaveConstellationDisplay.NewSaveButton.gameObject);

        if (createConstellationScript.IsSavedData())
            SaveConstellationDisplay.SetVisibility(SaveConstellationType.SavedData);
        else
            SaveConstellationDisplay.SetVisibility(SaveConstellationType.NewData);

        //仮想キーボード表示
//#if UNITY_STANDALONE_WIN
//        System.Diagnostics.Process.Start("osk.exe");
//#endif
        SaveInputField = createConstellationScript.InputField.GetComponent<InputField>();

    }

    //セーブデータを削除ボタン押した時の処理
    public void DeleteSavedData()
    {
        SetButtonInteractable(false, false, false, false, false, false, false);
        DisplayList();

        //フォーカスするボタン
        if (SelectConstellationButtons.Length > 0)
            EventSystem.current.SetSelectedGameObject(SelectConstellationButtons[0].gameObject);
        else
            EventSystem.current.SetSelectedGameObject(CanselButton.gameObject);

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
        SetButtonInteractable(true, true, false, true, true, true, true);
    }
    //参照画像読み込む（使ってない）
    public void LoadSprite(string path)
    {
        //try
        //{
        //    var rawData = System.IO.File.ReadAllBytes(path);
        //    Texture2D texture2D = new Texture2D(0, 0);
        //    texture2D.LoadImage(rawData);
        //    var sprite = Sprite.Create(texture2D, new Rect(0f, 0f, texture2D.width, texture2D.height),
        //        new Vector2(0.5f, 0.5f), 100f);
        //    //return sprite;
        //}
        //catch (Exception e)
        //{
        //    //return null;
        //}
    }
    //ボタンの有効無効を切り替え
    private void SetButtonInteractable(bool newButton,
    bool putTargeButton,
    bool putDeterminationButton,
    bool saveButton,
    bool loadButton,
    bool deleteSavedDataButton,
    bool titleButton)
    {
        NewButton.interactable = newButton;
        PutTargeButton.interactable = putTargeButton;
        PutDeterminationButton.interactable = putDeterminationButton;
        SaveButton.interactable = saveButton;
        LoadButton.interactable = loadButton;
        DeleteSavedDataButton.interactable = deleteSavedDataButton;
        TitleButton.interactable = titleButton;

        //ナビゲーション設定
        Button[] buttons = new Button[7];
        int index = 0;
        AddButtonArray(ref buttons, ref index, ref NewButton, newButton);
        AddButtonArray(ref buttons, ref index, ref PutTargeButton, putTargeButton);
        AddButtonArray(ref buttons, ref index, ref PutDeterminationButton, putDeterminationButton);
        AddButtonArray(ref buttons, ref index, ref SaveButton, saveButton);
        AddButtonArray(ref buttons, ref index, ref LoadButton, loadButton);
        AddButtonArray(ref buttons, ref index, ref DeleteSavedDataButton, deleteSavedDataButton);
        AddButtonArray(ref buttons, ref index, ref TitleButton, titleButton);

        //フォーカスするボタン
        if (index > 0)
            EventSystem.current.SetSelectedGameObject(buttons[0].gameObject);

        for (int i = 0;i < index;i++)
        {
            Navigation nv = buttons[i].navigation;
            int up = (i + index - 1) % index;
            int down = (i + index + 1) % index;
            nv.selectOnUp = buttons[up];
            nv.selectOnDown = buttons[down];
            buttons[i].navigation = nv;
        }
    }

    private void AddButtonArray(ref Button[] buttons, ref int index, ref Button add, bool enable)
    {
        if (enable)
        {
            buttons[index] = add;
            index++;
        }
    }

    public void TitleScene()
    {
        SceneManager.LoadScene("Title"); 
    }
}
