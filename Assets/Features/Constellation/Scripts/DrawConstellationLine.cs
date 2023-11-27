using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DrawConstellationLine : MonoBehaviour
{
    [Header("���C�����ЂƂ��q�����Ă����̑���")]
    [SerializeField, Range(0f, 1f)]
    public float DrawSpeed;
    [Header("���C�������_���[�����v���n�u")]
    public GameObject LinePrefab;

    private ProceduralGenerator ProceduralGenerator;
    private 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void DrawLine()
    {
        ProceduralGenerator = GameManagerScript.instance.ProceduralGenerator;
        StartCoroutine(DrawLineCoroutine(Mathf.Lerp(10.0f, 0.01f, 1.0f - Mathf.Pow(1.0f - DrawSpeed, 5))));
    }

    IEnumerator DrawLineCoroutine(float time)
    {
        ST_Constellation[] targets = GameManagerScript.instance.GenerateConstellation.targets;
        Line[] lines = GameManagerScript.instance.GenerateConstellation.lines;

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
