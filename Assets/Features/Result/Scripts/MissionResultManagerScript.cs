using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MissionScript;

public class MissionResultManagerScript : MonoBehaviour
{
    public GameObject MissionResultPrefab;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void AddContent(bool IsComp, MissionType type)
    {
        //���ڒǉ�
        GameObject obj = Instantiate(MissionResultPrefab);
        //�e�q�֌W
        obj.transform.parent = gameObject.transform;
        //���e������
        obj.GetComponent<MissionResultScript>().Set(IsComp, type);
    }
}
