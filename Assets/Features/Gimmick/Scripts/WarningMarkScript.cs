using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WarningMarkScript : MonoBehaviour
{
    private float Interval;
    private Image WarningMark;
    private Coroutine _Coroutine;
    // Start is called before the first frame update
    void Start()
    {
        WarningMark = GetComponent<Image>();
        WarningMark.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
    }

    public void StartWarning(Vector3 pos, float interval)
    {
        //_Coroutine = StartCoroutine(WarningCoroutine());
        //gameObject.GetComponent<RectTransform>().localPosition = pos;
        //Interval = interval;
    }
    public void StopWarning()
    {
        //StopCoroutine(_Coroutine);
        //WarningMark.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
    }

    IEnumerator WarningCoroutine()
    {
        while (true)
        {
            WarningMark.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            yield return new WaitForSeconds(Interval);
            WarningMark.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
            yield return new WaitForSeconds(Interval);
        }
    }
}
