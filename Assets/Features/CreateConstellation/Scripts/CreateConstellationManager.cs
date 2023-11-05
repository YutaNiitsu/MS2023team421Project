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
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
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
    }
    // �Z�[�u���L�����Z��
    public void OnSaveCancel()
    {
        //�Z�[�u��ʂ�����
        saveConstellationDIsplay.gameObject.SetActive(false);
    }
    // �����̃f�[�^��I�����ĕ\��
    private void DisplayList()
    {
        DeterminationButton.interactable = false;
        int cnt = ScrollViewportContents.transform.childCount;
        foreach (SelectButtonScript i in SelectConstellationButtons)
        {
            Destroy(i.gameObject);
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
        //����{�^����L���ɂ���
        DeterminationButton.interactable = true;
        //����{�^���������Ƃ��̏���
        DeterminationButton.onClick.AddListener(clickAction);
    }

    //�I�����������Ō��肵�ēǂݍ��ޏ���
    private void LoadSelectConstellation(SaveConstellationData data)
    {
        createConstellationScript.LoadConstellation(data);
        //�ꗗ�X�N���[�����\��
        ConstellationListDisplay.SetActive(false);
    }

    //�I�����������Ō��肵�č폜���鏈��
    private void DeleteSelectConstellation(uint deleteID)
    {
        SaveConstellationData[] newDatas = new SaveConstellationData[ConstellationDatas.Length - 1];
        int index = 0;
        foreach (SaveConstellationData i in ConstellationDatas)
        {
            if (i.id != deleteID)
            {
                //�폜����v�f�ԍ��ȊO����
                newDatas[index] = i;
                index++;
            }
        }
        ConstellationDatas = newDatas;
        //�Z�[�u����
        constellationSaveManager.OnSaveNewData(newDatas);
        //�ꗗ�X�N���[�����\��
        ConstellationListDisplay.SetActive(false);
    }
    //���[�h�{�^���������Ƃ��̏���
    public void ClickLoadButton()
    {
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
        saveConstellationDIsplay.gameObject.SetActive(true);
        if (createConstellationScript.IsSavedData())
            saveConstellationDIsplay.SetVisibility(SaveConstellationType.SavedData);
        else
            saveConstellationDIsplay.SetVisibility(SaveConstellationType.NewData);

    }

    //�Z�[�u�f�[�^���폜
    public void DeleteSavedData()
    {
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
}
