using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDScript : MonoBehaviour
{
    public Image DischargeNumber;
    private int PreDischargeNumber;

    // Start is called before the first frame update
    void Start()
    {
        PreDischargeNumber = 0;
    }

    // Update is called once per frame
    void Update()
    {
        int CurDischargeNumber = GameManagerScript.instance.StageManager.DischargeNumber;
        if (CurDischargeNumber != PreDischargeNumber)
        {
            PreDischargeNumber = CurDischargeNumber;
            DischargeNumber.material.SetFloat("_Number", 0);
        }
    }
}
