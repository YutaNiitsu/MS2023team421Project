using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class SaveConstellationDIsplayScript : MonoBehaviour
{
    public enum SaveConstellationType
    { 
        NewData,      // 新規作成した星座だった場合
        SavedData,    // セーブデータからの読み込みだった場合
    }

    public InputField Input;
    public Button NewSaveButton;
    public Button OverwriteSaveButton;
    public Button CancelButton;

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
                OverwriteSaveButton.interactable = false;

                //ナビゲーションの設定
                //SetNavigation(Input, NewSaveButton, NewSaveButton, null, null);
                //SetNavigation(NewSaveButton, Input, Input, CancelButton, CancelButton);
                //SetNavigation(CancelButton, Input, Input, NewSaveButton, NewSaveButton);
                SetNavigation(NewSaveButton, null, null, CancelButton, CancelButton);
                SetNavigation(CancelButton, null, null, NewSaveButton, NewSaveButton);
                break;
            case SaveConstellationType.SavedData:
                // セーブデータからの読み込みだった場合は上書きセーブ用ボタンを有効にする
                OverwriteSaveButton.interactable = true;

                //ナビゲーションの設定
                //SetNavigation(Input, NewSaveButton, NewSaveButton, null, null);
                //SetNavigation(NewSaveButton, Input, Input, CancelButton, OverwriteSaveButton);
                //SetNavigation(OverwriteSaveButton, Input, Input, NewSaveButton, CancelButton);
                //SetNavigation(CancelButton, Input, Input, OverwriteSaveButton, NewSaveButton);
                SetNavigation(NewSaveButton, null, null, CancelButton, OverwriteSaveButton);
                SetNavigation(OverwriteSaveButton, null, null, NewSaveButton, CancelButton);
                SetNavigation(CancelButton, null, null, OverwriteSaveButton, NewSaveButton);
                break;
            default:
                break;
        }
    }

    private void SetNavigation(Selectable target, Selectable up, Selectable down,
        Selectable left, Selectable right)
    {
        Navigation nav = target.navigation;
        nav.selectOnUp = up;
        nav.selectOnDown = down;
        nav.selectOnLeft = left;
        nav.selectOnRight = right;
        target.navigation = nav;
    }
}
