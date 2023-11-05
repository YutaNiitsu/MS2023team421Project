using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class SaveConstellationDIsplayScript : MonoBehaviour
{
    public enum SaveConstellationType
    { 
        NewData,      // �V�K�쐬���������������ꍇ
        SavedData,    // �Z�[�u�f�[�^����̓ǂݍ��݂������ꍇ
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
                // �V�K�쐬���������������ꍇ�͏㏑���Z�[�u�p�{�^���𖳌��ɂ���
                SavedButton.interactable = false;
                break;
            case SaveConstellationType.SavedData:
                // �Z�[�u�f�[�^����̓ǂݍ��݂������ꍇ�͏㏑���Z�[�u�p�{�^����L���ɂ���
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
