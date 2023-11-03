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
    private ST_Constellation[] Constellations;
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

    // Start is called before the first frame update
    void Start()
    {
        Targets = new GameObject[0];
        LineRenderers = new LineRenderer[1];
        LineRenderers[0] = Instantiate(LineRendererPrefab.gameObject).GetComponent<LineRenderer>();
        LineRenderers[0].positionCount = 0;
        LineRenderers[0].startWidth = LineWidth;
        LineRenderers[0].endWidth = LineWidth;
        Lines = new Line[1];

        DeterminationButton.interactable = false;
    }

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
                break;
            case 1:
                // ��ɔz�u���郂�[�h
                SaveButton.interactable = false;
                PutTargetInLineButton.interactable = false;
                PutTargetInLoopButton.interactable = false;
                DeterminationButton.interactable = true;
                break;
            case 2:
                // ����{�^�������ꂽ
                SaveButton.interactable = true;
                PutTargetInLoopButton.interactable = true;
                PutTargetInLineButton.interactable = true;
                DeterminationButton.interactable = false;

                

                LineRendererIndex++;
                LineRendererPointIndex = 0;
                //LineRenderer�̔z��̃T�C�Y�𑝂₷
                Array.Resize<LineRenderer>(ref LineRenderers, LineRendererIndex + 1);
                LineRenderers[LineRendererIndex] = new LineRenderer();
                LineRenderers[LineRendererIndex] = Instantiate(LineRendererPrefab.gameObject).GetComponent<LineRenderer>();
                LineRenderers[LineRendererIndex].positionCount = 0;
                LineRenderers[LineRendererIndex].startWidth = LineWidth;
                LineRenderers[LineRendererIndex].endWidth = LineWidth;
                break;
            default:
                break;
        }
        
       
       
    }
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
        LineRenderers[LineRendererIndex].positionCount++;
        pos.z += 5f;
        LineRenderers[LineRendererIndex].SetPosition(LineRendererPointIndex, pos);
        LineRendererPointIndex++;
        // �_���Q�ȏ�̎�
        if (LineRenderers[LineRendererIndex].positionCount > 1)
        {
            Array.Resize<Line>(ref Lines, LineIndex + 1);
            Vector3 start = LineRenderers[LineRendererIndex].GetPosition(LineRenderers[LineRendererIndex].positionCount - 2);
            Vector3 end = LineRenderers[LineRendererIndex].GetPosition(LineRenderers[LineRendererIndex].positionCount - 1);
            int startTargetIndex = Targets.Length - 2;
            int endTargetIndex = Targets.Length - 1;
            int index = LineRenderers[LineRendererIndex].positionCount - 2;
            Lines[LineIndex] = new Line();
            Lines[LineIndex].Create(start, end, startTargetIndex, endTargetIndex);
            LineIndex++;
        }
    }

    //�͂ߍ��ތ^����ɐݒu����
    private void PutTargetInLoop(Vector3 pos)
    {
        
    }

    // 

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
        }

        InputField input = InputName.GetComponent<InputField>();
        //���͂��ꂽ�����̖��O
        string name = input.text;

        saveConstellationData.Create(year, month, day, 0, name, constellations, Lines);


        return saveConstellationData;
    }
}
