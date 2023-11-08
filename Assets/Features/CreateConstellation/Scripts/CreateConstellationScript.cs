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
    private List<TargetInCreateModeScript> Targets;
    private int TargetIndex;
    //�J�[�\���őI�����ꂽ�͂ߍ��ތ^�̗v�f�ԍ�
    private int SelectedTargetIndex;
    // �����Ȃ���
    private List<ConstellationLine> ConstellationLines;

    //�Z�[�u�f�[�^����ǂݍ��񂾃f�[�^
    private SaveConstellationData SavedConstellationData = null;

    // Start is called before the first frame update
    void Start()
    {
        Targets = new List<TargetInCreateModeScript>();
        ConstellationLines = new List<ConstellationLine>();

        DeterminationButton.interactable = false;
        SelectedTargetIndex = -1;
        TargetIndex = 0;
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
            // �J�[�\�������������͂ߍ��ތ^�ɐ����q����
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
            Vector3 start = findTargetAtKey(preSelectedTargetIndex).gameObject.transform.position;
            start.z += 5f;
            //�I�_
            Vector3 end = findTargetAtKey(SelectedTargetIndex).gameObject.transform.position;
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
            TargetInCreateModeScript temp = Instantiate(TargetPrefab, pos, Quaternion.identity).GetComponent<TargetInCreateModeScript>();
            temp.SetKey(TargetIndex);
            Targets.Add(temp);

            //�n�_�̂͂ߍ��ތ^�̗v�f�ԍ�
            int startTargetIndex = SelectedTargetIndex;
            //�I�_�̂͂ߍ��ތ^�̗v�f�ԍ�
            int endTargetIndex = TargetIndex;
            //�͂ߍ��ތ^�̗v�f�ԍ���i�߂�
            TargetIndex++;

            //�I�����ꂽ�͂ߍ��ތ^��������������̃C���X�^���X�͐������Ȃ�
            if (SelectedTargetIndex == -1)
            {
                return;
            }

            // ����z�u
            //�n�_
            Vector3 start = findTargetAtKey(SelectedTargetIndex).gameObject.transform.position;
            start.z += 5f;
            //�I�_
            Vector3 end = pos;
            end.z += 5f;
            
            //���̃C���X�^���X����
            CreateLine(start, end, startTargetIndex, endTargetIndex);
            Debug.Log("CreateLine");
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
            {
                //���X�g����폜��������ꎞ�I�ɕۑ����邽�߂̔z��
                int[] removeList = new int[ConstellationLines.Count];
                int index = 0;
                for (int i = 0; i < ConstellationLines.Count; i++)
                {
                    ConstellationLine cl = ConstellationLines[i];
                    if ((cl.line.startTargetIndex == SelectedTargetIndex)
                        || (cl.line.endTargetIndex == SelectedTargetIndex))
                    {
                        removeList[index] = i;
                        cl.Destroy();
                        Debug.Log("LineDestroy");
                        index++;
                    }
                    
                }
                //���X�g����폜���������菜��
                for (int i = 0; i < index; i++)
                {
                    ConstellationLines.RemoveAt(removeList[i]);
                }
            }

            //�I�����ꂽ�͂ߍ��ތ^���폜����
            TargetInCreateModeScript temp = findTargetAtKey(SelectedTargetIndex);
            Destroy(temp.gameObject);
            Targets.Remove(temp);
            //���I����Ԃɂ���
            SelectedTargetIndex = -1;
        }
    }

    //���쐬
    private void CreateLine(Vector3 _start, Vector3 _end, int _startTargetIndex, int _endTargetIndex)
    {
        ConstellationLine temp = new ConstellationLine(LineRendererPrefab, _start, _end, _startTargetIndex, _endTargetIndex, LineWidth);
        ConstellationLines.Add(temp);
    }

    // ������
    public void Initialize()
    {
        //�C���X�^���X���폜
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
        SelectedTargetIndex = -1;
        TargetIndex = 0;
    }

    // �Z�[�u�f�[�^�쐬
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
        //���͂��ꂽ�����̖��O
        string name = input.text;
        uint id = 0;

        //�Z�[�u�f�[�^����ǂݍ��񂾐����Ȃ�ID�R�s�[
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

    //�Z�[�u�f�[�^����ǂݍ���ŕ\��
    public void LoadConstellation(SaveConstellationData savedConstellationData)
    {
        Initialize();
        SavedConstellationData = savedConstellationData;
        ST_Constellation[] targets = savedConstellationData.constellations;
        Line[] lines = savedConstellationData.lines;

        //�͂ߍ��ތ^���C���X�^���X����
        int index = 0;
        foreach (ST_Constellation i in targets)
        {
            TargetInCreateModeScript temp = Instantiate(TargetPrefab, i.position, Quaternion.identity).GetComponent<TargetInCreateModeScript>();
            temp.SetKey(i.Index);
            Targets.Add(temp);
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
                if (targetScript == null)
                    return false;

                if (SelectedTargetIndex == -1)
                {
                    //�����I������ĂȂ�����
                    SelectedTargetIndex = targetScript.Key;
                    targetScript.SetIsSelected(true);
                }
                else if (SelectedTargetIndex != targetScript.Key)
                {
                    //�O�N���b�N�������̂ƈႤ���̂�����
                    //�O�N���b�N�������̂𖢑I����Ԃɂ���
                    findTargetAtKey(SelectedTargetIndex).SetIsSelected(false);
                    SelectedTargetIndex = targetScript.Key;
                    targetScript.SetIsSelected(true);
                }
                else if (SelectedTargetIndex == targetScript.Key)
                {
                    //�O�N���b�N�������̂Ɠ�����������I������
                    targetScript.SetIsSelected(false);
                    SelectedTargetIndex = -1;
                    
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

    private TargetInCreateModeScript findTargetAtKey(int key)
    {
        foreach (TargetInCreateModeScript i in Targets)
        {
            if (i.Key == key)
            {
                return i;
            }
           
        }
        return null;
    }
}
