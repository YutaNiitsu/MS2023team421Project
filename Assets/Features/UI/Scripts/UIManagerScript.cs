using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManagerScript : MonoBehaviour
{
    public GameObject MiniMap;
    public GameObject WarningMark;
    public GameObject Result;

    //ƒVƒ“ƒOƒ‹ƒgƒ“
    public static UIManagerScript instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        MiniMap.SetActive(true);
        WarningMark.SetActive(true);
        Result.SetActive(false);
    }

    public void DisplayResult()
    {
        MiniMap.SetActive(false);
        WarningMark.SetActive(false);
        Result.SetActive(true);
        Result.GetComponent<ResultScript>().DisplayResult();
    }
}
