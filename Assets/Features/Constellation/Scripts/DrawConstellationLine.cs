using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class DrawConstellationLine : MonoBehaviour
{
    [Header("���C�����ЂƂ��q�����Ă����̑���")]
    [SerializeField, Range(0f, 1.0f)]
    public float DrawSpeed;
    [Header("���C�������_���[�����v���n�u")]
    public GameObject LinePrefab;
    [Header("�p�[�e�B�N���̃v���n�u")]
    public ParticleSystem ParticlePrefab;

    private ProceduralGenerator ProceduralGenerator;
    private bool IsStartedDrawing;
    private int state;
    private int CurLineIndex;
    Vector3 DrawStart = new Vector3();
    Vector3 DrawEnd = new Vector3();
    private Vector3 Vector;
    private Vector3 CurPos;
    private GameObject LineObject;
    private LineRenderer lineRenderer;
    private int DrawNumber;    //�n�_����I�_�܂ŕ`�悷��܂ł̃t���[����
    private ParticleSystem Particle;

    // Start is called before the first frame update
    void Start()
    {
        DrawStart = new Vector3();
        DrawEnd = new Vector3();
        state = 0;
        CurLineIndex = 0;
        CurPos = new Vector3();
        DrawNumber = 0;
    }

    private void Update()
    {
        if (GameManagerScript.instance.StageManager.GenerateConstellation == null)
            return;

        ST_Constellation[] targets = GameManagerScript.instance.StageManager.GenerateConstellation.targets;
        Line[] lines = GameManagerScript.instance.StageManager.GenerateConstellation.lines;

        switch (state)
        {
            case 0:
                break;
            case 1:
                foreach (ST_Constellation tar in targets)
                {
                    //�n�_�I�_���߂�
                    if (tar.Key == lines[CurLineIndex].startTargetKey)
                        DrawStart = tar.position;
                    if (tar.Key == lines[CurLineIndex].endTargetKey)
                        DrawEnd = tar.position;
                }
                //���������������߂�
                Vector = Vector3.Normalize(DrawEnd - DrawStart);
                //���̃C���X�^���X����
                LineObject = Instantiate(LinePrefab);
                lineRenderer = LineObject.GetComponent<LineRenderer>();
                CurPos = DrawStart;
                state = 2;
                DrawNumber = (int)(Vector3.Distance(DrawStart, DrawEnd) / DrawSpeed);
                Particle.Play();
                Particle.gameObject.transform.position = CurPos;
                break;
            case 2:
                //����i�߂�
                if (DrawNumber <= 0)
                    state = 3;
                lineRenderer.startWidth = 0.1f;
                lineRenderer.endWidth = 0.1f;
                lineRenderer.positionCount = 2;
                lineRenderer.SetPosition(0, DrawStart);
                lineRenderer.SetPosition(1, CurPos);
                CurPos += Vector * DrawSpeed;
                Particle.gameObject.transform.position = CurPos;
                DrawNumber--;
                break;
            case 3:
                CurLineIndex++;
                state = 1;

                if (CurLineIndex >= lines.Length)
                {
                    //���̕`�抮��
                    Particle.Stop();
                    Destroy(Particle.gameObject, 1.0f);
                    state = 4;
                }
                break;
            default:
                break;
        }
    }

    public void DrawLine()
    {
        ProceduralGenerator = GameManagerScript.instance.StageManager.ProceduralGenerator;
        state = 1;
        //�p�[�e�B�N������
        Particle = Instantiate(ParticlePrefab, CurPos, new Quaternion());
        //StartCoroutine(DrawLineCoroutine(Mathf.Lerp(10.0f, 0.01f, 1.0f - Mathf.Pow(1.0f - DrawSpeed, 5))));
    }

    //���`�抮��
    public bool FinishDraw()
    {
        if (state == 4)
            return true;

        return false;
    }

    IEnumerator DrawLineCoroutine(float time)
    {
        ST_Constellation[] targets = GameManagerScript.instance.StageManager.GenerateConstellation.targets;
        Line[] lines = GameManagerScript.instance.StageManager.GenerateConstellation.lines;

        foreach (Line i in lines)
        {
            //������
            Vector3 start = new Vector3();
            Vector3 end = new Vector3();
            foreach (ST_Constellation tar in targets)
            {
                if (tar.Key == i.startTargetKey)
                    start = tar.position;
                if (tar.Key == i.endTargetKey)
                    end = tar.position;
            }

            GameObject LineObject = Instantiate(LinePrefab);
            LineRenderer lineRenderer = LineObject.GetComponent<LineRenderer>();
            lineRenderer.startWidth = 0.1f;
            lineRenderer.endWidth = 0.1f;
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, start);
            lineRenderer.SetPosition(1, end);
            yield return new WaitForSeconds(time);
        }

    }
}
