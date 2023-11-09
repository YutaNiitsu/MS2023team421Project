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
    //�ǂݍ��񂾐����f�[�^�ꗗ��\���I�����邽�߂̃X�N���[��
    public GameObject ConstellationListDisplay;
    //�X�N���[���ɃA�C�e����ǉ�����Ƃ��ɂ���
    public GameObject ScrollViewportContents;
    //�I���Ɏg���{�^���v���n�u
    public GameObject SelectButton;
    //�I�����������Ɍ��肷��{�^��
    public Button DeterminationButton;
    //�{�^��
    public Button NewButton;
    public Button PutTargeButton;
    public Button PutDeterminationButton;
    public Button SaveButton;
    public Button LoadButton;
    public Button DeleteSavedDataButton;
    //�Z�[�u���
    public SaveConstellationDIsplayScript saveConstellationDIsplay;

    private SelectButtonScript[] SelectConstellationButtons;

    // �����f�[�^
    private SaveConstellationData[] ConstellationDatas;
    private ConstellationSaveManager constellationSaveManager;
    private CreateConstellationScript createConstellationScript;


    // Start is called before the first frame update
    void Start()
    {
        // �����f�[�^�̓ǂݍ���
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
        //�E�N���b�N�Őݒu
        if (Input.GetMouseButtonDown(1))
        {
            // �z�u�{�^�����L���i������Ă��Ȃ����j�ɂȂ��Ă�������s���Ȃ�
            if (PutTargeButton.interactable)
                return;
            // ��ʏ�ɔz�u���ꂽ�{�^���������ꂽ���͎��s���Ȃ�
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }
            createConstellationScript.DeleteTarget();
        }
        //���N���b�N�Őݒu
        if (Input.GetMouseButtonDown(0))
        {

            // �z�u�{�^�����L���i������Ă��Ȃ����j�ɂȂ��Ă�������s���Ȃ�
            if (PutTargeButton.interactable)
                return;

            // ��ʏ�ɔz�u���ꂽ�{�^���������ꂽ���͎��s���Ȃ�
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }
            //�}�E�X���W�̎擾
            Vector3 mousePos = Input.mousePosition;
            //�X�N���[�����W�����[���h���W�ɕϊ�
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 10f));
            createConstellationScript.PutTarget(worldPos);
        }
    }

    //�V�K�쐬
    public void New()
    {
        createConstellationScript.Initialize();
        SetButtonInteractable(true, true, false, true, true, true);
    }

    //�z�u�{�^�������ꂽ��
    public void ClickPutTargetButton()
    {
        SetButtonInteractable(false, false, true, false, false, false);
    }

    //�z�u����{�^�������ꂽ��
    public void ClickPutTargetDeterminationButton()
    {
        SetButtonInteractable(true, true, false, true, true, true);
    }

    // �����̃f�[�^��V�K�ŕۑ�
    public void OnSaveNewData()
    {
        //ID�����߂�
        uint id = 0;
        for (; ; id++)
        {
            bool success = true;
            foreach (SaveConstellationData i in ConstellationDatas)
            {
                if (i.id == id)
                {
                    // ����Ă����玸�s
                    success = false;
                }
            }
            if (success)
            {
                //����Ă��Ȃ�������I��
                break;
            }
        }

        //����Ă��Ȃ�����������ID�Ɍ���
        Array.Resize<SaveConstellationData>(ref ConstellationDatas, ConstellationDatas.Length + 1);
        ConstellationDatas[ConstellationDatas.Length - 1] = createConstellationScript.CreateSaveData();
        ConstellationDatas[ConstellationDatas.Length - 1].SetID(id);
        //�f�[�^���Z�[�u
        constellationSaveManager.OnSaveNewData(ConstellationDatas);
        //�Z�[�u��ʂ�����
        saveConstellationDIsplay.gameObject.SetActive(false);
    }
    // �����̃f�[�^���㏑���ۑ�
    public void OnSaveOverwrite()
    {
        SaveConstellationData data = createConstellationScript.CreateSaveData();
        //����ID��T��
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
        //�Z�[�u��ʂ�����
        saveConstellationDIsplay.gameObject.SetActive(false);
        SetButtonInteractable(true, true, false, true, true, true);
    }
    // �Z�[�u���L�����Z��
    public void OnSaveCancel()
    {
        //�Z�[�u��ʂ�����
        saveConstellationDIsplay.gameObject.SetActive(false);
        SetButtonInteractable(true, true, false, true, true, true);
    }
    // �����̃f�[�^��I�����ĕ\��
    private void DisplayList()
    {
        DeterminationButton.interactable = false;
        int cnt = ScrollViewportContents.transform.childCount;
        // �R���e���c�ɃA�C�e�����c���Ă�����폜
        for (int i = 0; i < cnt; i++)
        {
            Destroy(ScrollViewportContents.transform.GetChild(i).gameObject);
        }
       
        //�ꗗ�X�N���[����\��
        ConstellationListDisplay.SetActive(true);
        //�X�N���[���ɃA�C�e����ǉ�
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

    //�I���{�^�����������̏���
    public void ClickSelectButton(UnityAction clickAction)
    {
        //�ǉ�����������S���폜
        DeterminationButton.onClick.RemoveAllListeners();
        //����{�^����L���ɂ���
        DeterminationButton.interactable = true;
        //����{�^���������Ƃ��ɏ�������֐���ǉ�
        DeterminationButton.onClick.AddListener(clickAction);
    }

    //�I�����������Ō��肵�ēǂݍ��ޏ���
    private void LoadSelectConstellation(SaveConstellationData data)
    {
        createConstellationScript.LoadConstellation(data);
        //�ꗗ�X�N���[�����\��
        ConstellationListDisplay.SetActive(false);
        SetButtonInteractable(true, true, false, true, true, true);
    }

    //�I�����������Ō��肵�č폜���鏈��
    private void DeleteSelectConstellation(uint deleteID)
    {
        SaveConstellationData[] newDatas = new SaveConstellationData[ConstellationDatas.Length - 1];
        int index = 0;
        //�f�[�^�����Ȃ��Ă������΂�
        if (newDatas.Length != 0)
        {
            foreach (SaveConstellationData i in ConstellationDatas)
            {
                if (i.id != deleteID)
                {
                    //�폜����v�f�ԍ��ȊO����
                    newDatas[index] = i;
                    index++;
                }
            }
        }

        Array.Resize<SaveConstellationData>(ref ConstellationDatas, ConstellationDatas.Length - 1);
        ConstellationDatas = newDatas;
        //�Z�[�u����
        //newDatas = new SaveConstellationData[0];
        constellationSaveManager.OnSaveNewData(newDatas);
        //�ꗗ�X�N���[�����\��
        ConstellationListDisplay.SetActive(false);
        SetButtonInteractable(true, true, false, true, true, true);
    }
    //���[�h�{�^���������Ƃ��̏���
    public void ClickLoadButton()
    {
        SetButtonInteractable(false, false, false, false, false, false);
        DisplayList();

        int index = 0;
        foreach (SaveConstellationData i in ConstellationDatas)
        {
            SelectConstellationButtons[index].SetText(i.name, 24);
            // �{�^���N���b�N�����Ƃ��Ɏ��s����֐���ǉ�
            SelectConstellationButtons[index].AddClickAction(() => ClickSelectButton(() => LoadSelectConstellation(i)));
            index++;
        }
    }

    //�Z�[�u�{�^���������Ƃ��̏���
    public void ClickSaveButton()
    {
        SetButtonInteractable(false, false, false, false, false, false);
        saveConstellationDIsplay.gameObject.SetActive(true);
        if (createConstellationScript.IsSavedData())
            saveConstellationDIsplay.SetVisibility(SaveConstellationType.SavedData);
        else
            saveConstellationDIsplay.SetVisibility(SaveConstellationType.NewData);

    }

    //�Z�[�u�f�[�^���폜�{�^�����������̏���
    public void DeleteSavedData()
    {
        SetButtonInteractable(false, false, false, false, false, false);
        DisplayList();

        int index = 0;
        foreach (SaveConstellationData i in ConstellationDatas)
        {
            SelectConstellationButtons[index].SetText(i.name, 24);
            // �{�^���N���b�N�����Ƃ��Ɏ��s����֐���ǉ�
            SelectConstellationButtons[index].AddClickAction(() => ClickSelectButton(() => DeleteSelectConstellation(i.id)));
            index++;
        }
        
    }

    //�Z�[�u�f�[�^�ꗗ�\�����L�����Z��
    public void DisplayListCancel()
    {
        //�ꗗ�X�N���[�����\��
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
    //�{�^���̗L��������؂�ւ�
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
