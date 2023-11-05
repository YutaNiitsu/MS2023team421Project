using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEditor.PlayerSettings;
using static UnityEngine.GraphicsBuffer;

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
    public Button PutTargetInLineButton;
    public Button PutTargetInLoopButton;
    public Button DeterminationButton;
    //���͗�
    public GameObject InputName;
    // �ݒu���ꂽ�͂ߍ��ތ^
    private GameObject[] Targets;
    //�Z�[�u�p�̂͂ߍ��ތ^
    //private ST_Constellation[] Constellations;
    // �����Ȃ����̃��C�������_���[
    private LineRenderer[] LineRenderers;
    //�Z�[�u�p�̐����Ȃ���
    private Line[] Lines;
    //LineRenderers�̗v�f�ԍ�
    private int LineRendererIndex = 0;
    //Lines�̗v�f�ԍ�
    private int LineIndex = 0;
    //LineRenderer�̓_�̔ԍ�
    private int LineRendererPointIndex = 0;

    //�z�u���[�h
    private int Mode = 2;

    //�Z�[�u�f�[�^����ǂݍ��񂾃f�[�^
    private SaveConstellationData SavedConstellationData = null;

    // Start is called before the first frame update
    void Start()
    {
        Targets = new GameObject[0];
        LineRenderers = new LineRenderer[0];
        Lines = new Line[0];

        DeterminationButton.interactable = false;
    }

    //�z�u���[�h�؂�ւ�
    public void ChangeMode(int num)
    {
        Mode = num;
        switch (Mode)
        {
            case 0:
                // ����ɔz�u���郂�[�h
                SaveButton.interactable = false;
                PutTargetInLineButton.interactable = false;
                PutTargetInLoopButton.interactable = false;
                DeterminationButton.interactable = true;

                //���̃C���X�^���X�𐶐�
                Array.Resize<LineRenderer>(ref LineRenderers, LineRenderers.Length + 1);
                Array.Resize<Line>(ref Lines, Lines.Length + 1);
                LineRenderers[LineRendererIndex] = Instantiate(LineRendererPrefab.gameObject).GetComponent<LineRenderer>();
                LineRenderers[LineRendererIndex].positionCount = 0;
                break;
            case 1:
                // ��ɔz�u���郂�[�h
                SaveButton.interactable = false;
                PutTargetInLineButton.interactable = false;
                PutTargetInLoopButton.interactable = false;
                DeterminationButton.interactable = true;

                //���̃C���X�^���X�𐶐�
                Array.Resize<LineRenderer>(ref LineRenderers, LineRenderers.Length + 1);
                Array.Resize<Line>(ref Lines, Lines.Length + 1);
                LineRenderers[LineRendererIndex] = Instantiate(LineRendererPrefab.gameObject).GetComponent<LineRenderer>();
                LineRenderers[LineRendererIndex].positionCount = 0;
                break;
            case 2:
                // ����{�^�������ꂽ
                SaveButton.interactable = true;
                PutTargetInLoopButton.interactable = true;
                PutTargetInLineButton.interactable = true;
                DeterminationButton.interactable = false;


                //LineRenderer�̗v�f�ԍ���i�߂�
                LineRendererIndex++;
                //LineRenderer�̓_�̗v�f�ԍ���0�ɂ���
                LineRendererPointIndex = 0;
                break;
            default:
                break;
        }

       

    }

    //�͂ߍ��ތ^��ݒu����
    public void PutTarget(Vector3 pos)
    {
        switch (Mode)
        {
            case 0:
                // ����ɔz�u���郂�[�h
                PutTargetInLine(pos);
                break;
            case 1:
                // ��ɔz�u���郂�[�h
                PutTargetInLoop(pos);
                break;
            default:
                break;
        }
    }

    //�͂ߍ��ތ^�����ɐݒu����
    private void PutTargetInLine(Vector3 pos)
    {
        //�͂ߍ��ތ^��z�u
        Array.Resize<GameObject>(ref Targets, Targets.Length + 1);
        Targets[Targets.Length - 1] = Instantiate(TargetPrefab, pos, Quaternion.identity);

        // ����z�u
        LineRenderers[LineRendererIndex].startWidth = LineWidth;
        LineRenderers[LineRendererIndex].endWidth = LineWidth;
        LineRenderers[LineRendererIndex].positionCount++;
        pos.z += 5f;
        LineRenderers[LineRendererIndex].SetPosition(LineRendererPointIndex, pos);
        LineRendererPointIndex++;
        // �_���Q�ȏ�̎��͐��̏����X�V����
        if (LineRenderers[LineRendererIndex].positionCount > 1)
        {
            Array.Resize<Line>(ref Lines, LineIndex + 1);
            Vector3 start = LineRenderers[LineRendererIndex].GetPosition(LineRenderers[LineRendererIndex].positionCount - 2);
            Vector3 end = LineRenderers[LineRendererIndex].GetPosition(LineRenderers[LineRendererIndex].positionCount - 1);
            int startTargetIndex = Targets.Length - 2;
            int endTargetIndex = Targets.Length - 1;
            int index = LineRenderers[LineRendererIndex].positionCount - 2;
            Lines[LineIndex] = new Line();
            //�n�_�A�I�_�A�n�_�̂͂ߍ��ތ^�ƏI�_�̂͂ߍ��ތ^�̗v�f�ԍ���ۑ�
            Lines[LineIndex].Create(start, end, startTargetIndex, endTargetIndex);
            LineIndex++;
        }
    }

    //�͂ߍ��ތ^����ɐݒu����
    private void PutTargetInLoop(Vector3 pos)
    {
        
    }

    // ������
    public void Initialize()
    {
        //�C���X�^���X���폜
        foreach (GameObject i in Targets)
        {
            Destroy(i);
        }
        foreach (LineRenderer i in LineRenderers)
        {
            Destroy(i.gameObject);
        }

        Targets = new GameObject[0];
        LineRenderers = new LineRenderer[0];
        Lines = new Line[0];

        LineRendererIndex = 0;
        LineIndex = 0;
        LineRendererPointIndex = 0;
        Mode = 2;

        DeterminationButton.interactable = false;
        SavedConstellationData = null;
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
            

        saveConstellationData.Create(year, month, day, id, name, constellations, Lines);


        return saveConstellationData;
    }


    //�Z�[�u�f�[�^����ǂݍ���ŕ\��
    public void LoadConstellation(SaveConstellationData savedConstellationData)
    {
        Initialize();
        SavedConstellationData = savedConstellationData;
        ST_Constellation[] targets = savedConstellationData.constellations;
        Lines = savedConstellationData.lines;
        Array.Resize<GameObject>(ref Targets, targets.Length);

        int index = 0;
        foreach (ST_Constellation i in targets)
        {
            Targets[index] = Instantiate(TargetPrefab, i.position, Quaternion.identity);

            index++;
        }

        LineRenderers = new LineRenderer[1];
        LineRenderers[0] = Instantiate(LineRendererPrefab.gameObject).GetComponent<LineRenderer>();
        LineRenderers[0].startWidth = LineWidth;
        LineRenderers[0].endWidth = LineWidth;
        int preEndTargetIndex = -1;
        int _lineRendererIndex = 0;
        int pointIndex = 0;
        foreach (Line i in Lines)
        {
            if (preEndTargetIndex != -1)
            {
                //�O�̐��̏I�_�ƌ��݂̐��̎n�_�������Ȃ�q�����Ă���
                if (preEndTargetIndex == i.startTargetIndex)
                {
                    LineRenderers[_lineRendererIndex].positionCount++;
                    LineRenderers[_lineRendererIndex].SetPosition(pointIndex, i.start);
                    pointIndex++;
                    LineRenderers[_lineRendererIndex].positionCount++;
                    LineRenderers[_lineRendererIndex].SetPosition(pointIndex, i.end);
                    pointIndex++;

                    preEndTargetIndex = i.endTargetIndex;
                }
                //�q�����Ă��Ȃ�����
                else
                {
                    pointIndex = 0;
                    _lineRendererIndex++;

                    Array.Resize<LineRenderer>(ref LineRenderers, _lineRendererIndex + 1);
                    LineRenderers[_lineRendererIndex] = new LineRenderer();
                    LineRenderers[_lineRendererIndex] = Instantiate(LineRendererPrefab.gameObject).GetComponent<LineRenderer>();
                    LineRenderers[_lineRendererIndex].startWidth = LineWidth;
                    LineRenderers[_lineRendererIndex].endWidth = LineWidth;

                    LineRenderers[_lineRendererIndex].positionCount++;
                    LineRenderers[_lineRendererIndex].SetPosition(pointIndex, i.start);
                    pointIndex++;
                    LineRenderers[_lineRendererIndex].positionCount++;
                    LineRenderers[_lineRendererIndex].SetPosition(pointIndex, i.end);
                    pointIndex++;

                    preEndTargetIndex = i.endTargetIndex;
                }
            }
            else
            {
                //����
                LineRenderers[_lineRendererIndex].positionCount++;
                LineRenderers[_lineRendererIndex].SetPosition(pointIndex, i.start);
                pointIndex++;
                LineRenderers[_lineRendererIndex].positionCount++;
                LineRenderers[_lineRendererIndex].SetPosition(pointIndex, i.end);
                pointIndex++;
               
                preEndTargetIndex = i.endTargetIndex;
            }
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
}
