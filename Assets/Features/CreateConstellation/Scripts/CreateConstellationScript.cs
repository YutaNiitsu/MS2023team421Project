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
    //�����Ȃ����̑���
    public float LineWidth;
    // �͂ߍ��ތ^�̃v���n�u
    public GameObject TargetPrefab;
    // �����Ȃ����̃v���n�u
    public LineRenderer LineRendererPrefab;
    //�{�^��
    public Button SaveButton;
    public Button PutTargeButton;
    public Button DeterminationButton;
    //���͗�
    public GameObject InputName;
    // �ݒu���ꂽ�͂ߍ��ތ^
    private GameObject[] Targets;
    //�J�[�\���őI�����ꂽ�͂ߍ��ތ^�̗v�f�ԍ�
    private int SelectedTargetIndex;
    private GameObject SelectedTarget;
    // �����Ȃ���
    private ConstellationLine[] ConstellationLines;
    private int ConstellationLinesIndex;

    //�Z�[�u�f�[�^����ǂݍ��񂾃f�[�^
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

    //�z�u�{�^�������ꂽ��
    public void ClickPutTargetButton()
    {
        SaveButton.interactable = false;
        PutTargeButton.interactable = false;
        DeterminationButton.interactable = true;
    }

    //�z�u����{�^�������ꂽ��
    public void ClickPutTargetDeterminationButton()
    {
        SaveButton.interactable = true;
        PutTargeButton.interactable = true;
        DeterminationButton.interactable = false;
    }
  
    //�͂ߍ��ތ^��ݒu����
    public void PutTarget(Vector3 pos)
    {
        // �z�u�{�^�����L���i������Ă��Ȃ����j�ɂȂ��Ă�������s���Ȃ�
        if (PutTargeButton.interactable)
            return;

        //�O�ɑI������Ă����͂ߍ��ތ^�̗v�f�ԍ���ۑ�
        int preSelectedTargetIndex = SelectedTargetIndex;

        if (CheckCursorHitTarget())
        {
            //�J�[�\�������ɐݒu���ꂽ�͂ߍ��ތ^�Ɠ������Ă����Ƃ�
            //�I�����ꂽ�͂ߍ��ތ^��������������̃C���X�^���X�͐������Ȃ�
            if (SelectedTargetIndex == -1)
            {
                return;
            }
            //�O�ɑI������Ă����͂ߍ��ތ^���Ȃ���������̃C���X�^���X�͐������Ȃ�
            if (preSelectedTargetIndex == -1)
            {
                return;
            }
            // �J�[�\�������������͂ߍ��ތ^�ɐ����q����

            foreach (ConstellationLine i in ConstellationLines)
            {
                if ((i.line.startTargetIndex == preSelectedTargetIndex && i.line.endTargetIndex == SelectedTargetIndex)
                    || (i.line.startTargetIndex == SelectedTargetIndex && i.line.endTargetIndex == preSelectedTargetIndex))
                {
                    //���ɐ�����������Ă�������s���Ȃ�
                    return;
                }
            }

            //�n�_
            Vector3 start = Targets[preSelectedTargetIndex].transform.position;
            start.z += 5f;
            //�I�_
            Vector3 end = Targets[SelectedTargetIndex].transform.position;
            end.z += 5f;
            //�n�_�̂͂ߍ��ތ^�̗v�f�ԍ�
            int startTargetIndex = preSelectedTargetIndex;
            //�I�_�̂͂ߍ��ތ^�̗v�f�ԍ�
            int endTargetIndex = SelectedTargetIndex;
            //���̃C���X�^���X����
            CreateLine(start, end, startTargetIndex, endTargetIndex);
        }
        else
        {
            //�͂ߍ��ތ^��z�u
            Array.Resize<GameObject>(ref Targets, Targets.Length + 1);
            Targets[Targets.Length - 1] = Instantiate(TargetPrefab, pos, Quaternion.identity);
            Targets[Targets.Length - 1].GetComponent<TargetInCreateModeScript>().SetIndex(Targets.Length - 1);


            //�I�����ꂽ�͂ߍ��ތ^��������������̃C���X�^���X�͐������Ȃ�
            if (SelectedTargetIndex == -1)
            {
                return;
            }

            // ����z�u
            //�n�_
            Vector3 start = Targets[SelectedTargetIndex].transform.position;
            start.z += 5f;
            //�I�_
            Vector3 end = pos;
            end.z += 5f;
            //�n�_�̂͂ߍ��ތ^�̗v�f�ԍ�
            int startTargetIndex = SelectedTargetIndex;
            //�I�_�̂͂ߍ��ތ^�̗v�f�ԍ�
            int endTargetIndex = Targets.Length - 1;
            //���̃C���X�^���X����
            CreateLine(start, end, startTargetIndex, endTargetIndex);
        }
    }

    //�͂ߍ��ތ^���폜����
    public void DeleteTarget()
    {
        // �z�u�{�^�����L���i������Ă��Ȃ����j�ɂȂ��Ă�������s���Ȃ�
        if (PutTargeButton.interactable)
            return;

        if (CheckCursorHitTarget())
        {
            //���I����Ԃ���������s���Ȃ�
            if (SelectedTargetIndex == -1)
            {
                return;
            }

            //�I�����ꂽ�͂ߍ��ތ^���܂ސ���S�ď���
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
            //�T�C�Y�ύX
            Array.Resize<ConstellationLine>(ref ConstellationLines, index);
            for (int i = 0; i < ConstellationLines.Length; i++)
            {
                ConstellationLines[i] = clTemp[i];
            }

            //�I�����ꂽ�͂ߍ��ތ^���폜����
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
                //�I�����ꂽ�͂ߍ��ތ^�̗v�f�ԍ��ƈ�v�������΂�
                if (index != SelectedTargetIndex)
                {
                    Targets[newIndex] = targetTemp[oldIndex];
                    Targets[newIndex].GetComponent<TargetInCreateModeScript>().SetIndex(newIndex);
                    foreach (ConstellationLine i in ConstellationLines)
                    {
                        //�����̗v�f�ԍ����X�V
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

            //���I����Ԃɂ���
            SelectedTargetIndex = -1;
        }
    }

    //���쐬
    private void CreateLine(Vector3 _start, Vector3 _end, int _startTargetIndex, int _endTargetIndex)
    {
        Array.Resize<ConstellationLine>(ref ConstellationLines, ConstellationLinesIndex + 1);
        ConstellationLines[ConstellationLinesIndex] 
            = new ConstellationLine(LineRendererPrefab, _start, _end, _startTargetIndex, _endTargetIndex, LineWidth);
        ConstellationLinesIndex++;
    }

    // ������
    public void Initialize()
    {
        //�C���X�^���X���폜
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

    // �Z�[�u�f�[�^�쐬
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
        //���͂��ꂽ�����̖��O
        string name = input.text;
        uint id = 0;

        //�Z�[�u�f�[�^����ǂݍ��񂾐����Ȃ�ID�R�s�[
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

    //�Z�[�u�f�[�^����ǂݍ���ŕ\��
    public void LoadConstellation(SaveConstellationData savedConstellationData)
    {
        Initialize();
        SavedConstellationData = savedConstellationData;
        ST_Constellation[] targets = savedConstellationData.constellations;
        Line[] lines = savedConstellationData.lines;
        Array.Resize<GameObject>(ref Targets, targets.Length);

        //�͂ߍ��ތ^���C���X�^���X����
        int index = 0;
        foreach (ST_Constellation i in targets)
        {
            Targets[index] = Instantiate(TargetPrefab, i.position, Quaternion.identity);
            Targets[index].GetComponent<TargetInCreateModeScript>().SetIndex(index);
            index++;
        }

        //����z�u
        //���̃C���X�^���X����
        foreach (Line i in lines)
        {
            //���̃C���X�^���X����
            CreateLine(i.start, i.end, i.startTargetIndex, i.endTargetIndex);
        }
    }

    //�Z�[�u�f�[�^����ǂݍ��񂾐������ǂ���
    public bool IsSavedData()
    {
        if (SavedConstellationData == null)
        {
            return false;
        }
        return true;
    }
    
    //�J�[�\���Ƃ͂ߍ��ތ^�̓����蔻��
    private bool CheckCursorHitTarget()
    {
        //���C�L���X�g
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

        if (hit.collider != null)
        {
            //�J�[�\���Ƃ͂ߍ��ތ^���������Ă���
            if (hit.collider.CompareTag("Target"))
            {
                TargetInCreateModeScript targetScript = hit.collider.gameObject.GetComponent<TargetInCreateModeScript>();
               
                if (SelectedTargetIndex == -1)
                {
                    //�����I������ĂȂ�����
                    SelectedTargetIndex = targetScript.Index;
                    targetScript.SetIsSelected(true);
                }
                else if (SelectedTargetIndex != targetScript.Index)
                {
                    //�O�N���b�N�������̂ƈႤ���̂�����
                    //�O�N���b�N�������̂𖢑I����Ԃɂ���
                    Targets[SelectedTargetIndex].GetComponent<TargetInCreateModeScript>().SetIsSelected(false);
                    SelectedTargetIndex = targetScript.Index;
                    targetScript.SetIsSelected(true);
                }
                else if (SelectedTargetIndex == targetScript.Index)
                {
                    //�O�N���b�N�������̂Ɠ�����������I������
                    targetScript.SetIsSelected(false);
                    SelectedTargetIndex = -1;
                    
                }
                return true;
            }
        }

        return false;
    }
}
