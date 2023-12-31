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
        //項目追加
        GameObject obj = Instantiate(MissionResultPrefab);
        //親子関係
        obj.transform.parent = gameObject.transform;
        //内容を決定
        obj.GetComponent<MissionResultScript>().Set(IsComp, type);
    }
}
