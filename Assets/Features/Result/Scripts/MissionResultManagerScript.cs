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
        //€–Ú’Ç‰Á
        GameObject obj = Instantiate(MissionResultPrefab);
        //eqŠÖŒW
        obj.transform.parent = gameObject.transform;
        //“à—e‚ğŒˆ’è
        obj.GetComponent<MissionResultScript>().Set(IsComp, type);
    }
}
