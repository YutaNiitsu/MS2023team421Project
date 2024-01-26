using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
//using static UnityEditor.PlayerSettings;
using static UnityEngine.GraphicsBuffer;
using System.Runtime.InteropServices;
using UnityEngine.InputSystem;

public class ConstellationLine : MonoBehaviour
{
    public Line line;
    public GameObject LineObject { get; protected set; }

    private ConstellationLine() { }
    public ConstellationLine(GameObject prefab, Vector3 _start, Vector3 _end, int _startTargetKey, int _endTargetKey, float _lineWidth) 
    {
        LineObject = Instantiate(prefab);
        line = new Line();
        line.start = _start;
        line.end = _end;
        line.startTargetKey = _startTargetKey;
        line.endTargetKey = _endTargetKey;

        LineRenderer lineRenderer = LineObject.GetComponent<LineRenderer>();
        lineRenderer.startWidth = _lineWidth;
        lineRenderer.endWidth = _lineWidth;
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, _start);
        lineRenderer.SetPosition(1, _end);
        //Debug.Log(LineObject);
    }

    public void Destroy()
    {
        //Debug.Log(LineObject);
        if (LineObject != null)
        {
            Destroy(LineObject);
            
        }
        
    }
}

public class CreateConstellationScript : MonoBehaviour
{
    //�����Ȃ����̑���
    public float LineWidth;
    // �͂ߍ��ތ^�̃v���n�u
    public GameObject TargetPrefab;
    // �����Ȃ����̃v���n�u
    public GameObject LineRendererPrefab;
    
    //���͗�
    public GameObject InputName;
    // �ݒu���ꂽ�͂ߍ��ތ^
    private List<TargetInCreateModeScript> Targets;
    private int TargetKey;
    //�J�[�\���őI�����ꂽ�͂ߍ��ތ^
    private TargetInCreateModeScript SelectedTarget;
    // �����Ȃ���
    private List<ConstellationLine> ConstellationLines;

    //�Z�[�u�f�[�^����ǂݍ��񂾃f�[�^
    private SaveConstellationData SavedConstellationData = null;

    public Vector3 CursorPosition { get; protected set; }

    [DllImport("user32.dll")]
    static extern bool SetCursorPos(int X, int Y);

    // Start is called before the first frame update
    void Start()
    {
        Targets = new List<TargetInCreateModeScript>();
        ConstellationLines = new List<ConstellationLine>();
        CursorPosition = new Vector2(Screen.width / 2, Screen.height / 2);

        SelectedTarget = null;
        TargetKey = 0;
    }

    
  
    //�͂ߍ��ތ^��ݒu����
    public void PutTarget(Vector3 pos)
    {

        //�O�ɑI������Ă����͂ߍ��ތ^�̗v�f�ԍ���ۑ�
        TargetInCreateModeScript preSelectedTarget= SelectedTarget;

        if (CheckCursorHitTarget())
        {
            // �J�[�\�������������͂ߍ��ތ^�ɐ����q����
            //�J�[�\�������ɐݒu���ꂽ�͂ߍ��ތ^�Ɠ������Ă����Ƃ�
            //�I�����ꂽ�͂ߍ��ތ^��������������̃C���X�^���X�͐������Ȃ�
            if (SelectedTarget == null)
            {
                return;
            }
            //�O�ɑI������Ă����͂ߍ��ތ^���Ȃ���������̃C���X�^���X�͐������Ȃ�
            if (preSelectedTarget == null)
            {
                return;
            }

            
            foreach (ConstellationLine i in ConstellationLines)
            {
                if ((i.line.startTargetKey == preSelectedTarget.Key && i.line.endTargetKey == SelectedTarget.Key)
                    || (i.line.startTargetKey == SelectedTarget.Key && i.line.endTargetKey == preSelectedTarget.Key))
                {
                    //���ɐ�����������Ă�������s���Ȃ�
                    return;
                }
            }

            //�n�_
            Vector3 start = preSelectedTarget.gameObject.transform.position;
            start.z += 5f;
            //�I�_
            Vector3 end = SelectedTarget.gameObject.transform.position;
            end.z += 5f;
            //�n�_�̂͂ߍ��ތ^�̗v�f�ԍ�
            int startTargetKey = preSelectedTarget.Key;
            //�I�_�̂͂ߍ��ތ^�̗v�f�ԍ�
            int endTargetKey = SelectedTarget.Key;
            //���̃C���X�^���X����
            CreateLine(start, end, startTargetKey, endTargetKey);
        }
        else
        {
            //�͂ߍ��ތ^��z�u
            TargetInCreateModeScript temp = Instantiate(TargetPrefab, pos, Quaternion.identity).GetComponent<TargetInCreateModeScript>();
            temp.SetKey(TargetKey);
            Targets.Add(temp);

            
            //�I�_�̂͂ߍ��ތ^�̗v�f�ԍ�
            int endTargetIndex = TargetKey;
            //�͂ߍ��ތ^�̗v�f�ԍ���i�߂�
            TargetKey++;

            //�I�����ꂽ�͂ߍ��ތ^��������������̃C���X�^���X�͐������Ȃ�
            if (SelectedTarget == null)
            {
                return;
            }
            //�n�_�̂͂ߍ��ތ^�̗v�f�ԍ�
            int startTargetIndex = SelectedTarget.Key;

            // ����z�u
            //�n�_
            Vector3 start = SelectedTarget.gameObject.transform.position;
            start.z += 5f;
            //�I�_
            Vector3 end = pos;
            end.z += 5f;
            
            //���̃C���X�^���X����
            CreateLine(start, end, startTargetIndex, endTargetIndex);
            
        }
    }

    //�͂ߍ��ތ^���폜����
    public void DeleteTarget()
    {
        
        if (CheckCursorHitTarget())
        {
            //���I����Ԃ���������s���Ȃ�
            if (SelectedTarget == null)
            {
                return;
            }

            //�I�����ꂽ�͂ߍ��ތ^���܂ސ���S�ď���
            {
                //���X�g����폜���Ȃ������ꎞ�I�ɕۑ����邽�߂̔z��
                ConstellationLine[] temp = new ConstellationLine[ConstellationLines.Count];
                int index = 0;
                foreach (ConstellationLine i in ConstellationLines)
                {
                    if ((i.line.startTargetKey != SelectedTarget.Key)
                        && (i.line.endTargetKey != SelectedTarget.Key))
                    {
                        //���X�g�Ɏc��
                        temp[index] = i;
                        index++;
                    }
                    else
                    {
                        //�폜����
                        i.Destroy();
                    }
                }
                //���X�g����폜���������菜��
                ConstellationLines = new List<ConstellationLine>();
                for (int i = 0; i < index; i++)
                {
                    ConstellationLines.Add(temp[i]);
                }
            }

            //�I�����ꂽ�͂ߍ��ތ^���폜����
            Targets.Remove(SelectedTarget);
            Destroy(SelectedTarget.gameObject);
            //���I����Ԃɂ���
            SelectedTarget = null;
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

        SavedConstellationData = null;
        SelectedTarget = null;
        TargetKey = 0;
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
            constellations[index].Key = i.Key;
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
        ST_Constellation[] targets = savedConstellationData.targets;
        Line[] lines = savedConstellationData.lines;

        //�͂ߍ��ތ^���C���X�^���X����
        foreach (ST_Constellation i in targets)
        {
            TargetInCreateModeScript temp = Instantiate(TargetPrefab, i.position, Quaternion.identity).GetComponent<TargetInCreateModeScript>();
            temp.SetKey(i.Key);
            Targets.Add(temp);
        }

        //����z�u
        //���̃C���X�^���X����
        foreach (Line i in lines)
        {
            //���̃C���X�^���X����
            CreateLine(i.start, i.end, i.startTargetKey, i.endTargetKey);
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

                if (SelectedTarget == null)
                {
                    //�����I������ĂȂ�����
                    SelectedTarget = targetScript;
                    targetScript.SetIsSelected(true);
                }
                else if (SelectedTarget.Key != targetScript.Key)
                {
                    //�O�N���b�N�������̂ƈႤ���̂�����
                    //�O�N���b�N�������̂𖢑I����Ԃɂ���
                    SelectedTarget.SetIsSelected(false);
                    SelectedTarget = targetScript;
                    targetScript.SetIsSelected(true);
                }
                else if (SelectedTarget.Key == targetScript.Key)
                {
                    //�O�N���b�N�������̂Ɠ�����������I������
                    targetScript.SetIsSelected(false);
                    SelectedTarget = null;
                    
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

    private bool IsGamePad()
    {
        
        string[] joystickNames = Input.GetJoystickNames();

        // �W���C�X�e�B�b�N��1�ȏ�ڑ�����Ă��邩�𔻒�
        if (joystickNames.Length > 0 && !string.IsNullOrEmpty(joystickNames[0]))
        {
            return true;
        }

        return false;
    }

    public void MoveCursor(bool pad)
    {
        if (IsGamePad() && pad)
        {
            float moveX = Input.GetAxis("Horizontal") + Input.GetAxis("Mouse X");
            float moveY = -Input.GetAxis("Vertical") - Input.GetAxis("Mouse Y");
            CursorPosition += new Vector3(moveX, moveY, 0.0f) * 10.0f;
        }
        else
        {
            float moveX = Input.GetAxis("Mouse X");
            float moveY = -Input.GetAxis("Mouse Y");
            CursorPosition += new Vector3(moveX, moveY, 0.0f) * 10.0f;

        }

        if (CursorPosition.x <= 0.0f)
            CursorPosition = new Vector3(0.0f, CursorPosition.y, 0.0f);
        if (CursorPosition.x >= Screen.width)
            CursorPosition = new Vector3(Screen.width, CursorPosition.y, 0.0f);
        if (CursorPosition.y <= 0.0f)
            CursorPosition = new Vector3(CursorPosition.x, 0.0f, 0.0f);
        if (CursorPosition.y >= Screen.height)
            CursorPosition = new Vector3(CursorPosition.x, Screen.height, 0.0f);
        //Debug.Log(CursorPosition);
        SetCursorPos((int)CursorPosition.x, (int)CursorPosition.y);
    }
}
