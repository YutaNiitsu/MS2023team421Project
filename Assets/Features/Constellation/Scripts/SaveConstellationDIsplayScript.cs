using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class SaveConstellationDIsplayScript : MonoBehaviour
{
    public enum SaveConstellationType
    { 
        NewData,      // 新規作成した星座だった場合
        SavedData,    // セーブデータからの読み込みだった場合
    }

    public Button SavedButton;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SetVisibility(SaveConstellationType type)
    {

        switch (type)
        {
            case SaveConstellationType.NewData:
                // 新規作成した星座だった場合は上書きセーブ用ボタンを無効にする
                SavedButton.interactable = false;
                break;
            case SaveConstellationType.SavedData:
                // セーブデータからの読み込みだった場合は上書きセーブ用ボタンを有効にする
                SavedButton.interactable = true;
                break;
            default:
                break;
        }

    }

    //public void NewSave()
    //{

    //}
    //public void Overwrite()
    //{

    //}
    //public void Cancel()
    //{

    //}
}
