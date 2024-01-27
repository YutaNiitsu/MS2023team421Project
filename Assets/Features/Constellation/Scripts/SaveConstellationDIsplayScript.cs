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
        NewData,      // �V�K�쐬���������������ꍇ
        SavedData,    // �Z�[�u�f�[�^����̓ǂݍ��݂������ꍇ
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
                // �V�K�쐬���������������ꍇ�͏㏑���Z�[�u�p�{�^���𖳌��ɂ���
                OverwriteSaveButton.interactable = false;

                //�i�r�Q�[�V�����̐ݒ�
                //SetNavigation(Input, NewSaveButton, NewSaveButton, null, null);
                //SetNavigation(NewSaveButton, Input, Input, CancelButton, CancelButton);
                //SetNavigation(CancelButton, Input, Input, NewSaveButton, NewSaveButton);
                SetNavigation(NewSaveButton, null, null, CancelButton, CancelButton);
                SetNavigation(CancelButton, null, null, NewSaveButton, NewSaveButton);
                break;
            case SaveConstellationType.SavedData:
                // �Z�[�u�f�[�^����̓ǂݍ��݂������ꍇ�͏㏑���Z�[�u�p�{�^����L���ɂ���
                OverwriteSaveButton.interactable = true;

                //�i�r�Q�[�V�����̐ݒ�
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
