using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManagerScript : MonoBehaviour
{
    public GameObject MiniMap;
    public GameObject WarningMark;
    public GameObject Result;
    public GameObject Pause;

    // Start is called before the first frame update
    void Start()
    {
        MiniMap.SetActive(true);
        WarningMark.SetActive(true);
        Result.SetActive(false);
        Pause.SetActive(false);
    }

    //ƒŠƒUƒ‹ƒg•\Ž¦
    public void DisplayResult()
    {
        MiniMap.SetActive(false);
        WarningMark.SetActive(false);
        Result.SetActive(true);
        Result.GetComponent<ResultScript>().DisplayResult();
    }

    public void DisplayPauseMenu()
    {
        MiniMap.SetActive(false);
        WarningMark.SetActive(false);
        Result.SetActive(false);
        Pause.SetActive(true);
    }

    public void HiddenPauseMenu()
    {
        MiniMap.SetActive(true);
        WarningMark.SetActive(true);
        Result.SetActive(false);
        Pause.SetActive(false);
    }
}
