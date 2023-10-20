using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddSocrePoint : MonoBehaviour
{
    //public GameObject[2] cubeArray;
    private int count;
    public GameObject cubeObj;
    Vector2 StartPos;

    void Start()
    {
        count = 0;
        //cubeObj = GameObject.Instantiate(cubeArray[count]) as GameObject;
        StartPos = this.transform.position;
    }

    public void CubeSet()
    {
        if (count == 0)
        {
            count = 1;
            //cubeObj = GameObject.Instantiate(cubeObj) as GameObject;
            Instantiate(cubeObj, new Vector2(StartPos.x,StartPos.y), Quaternion.identity);
            Destroy(this);
        }
        //GameObject.Instantiate(cubeArray[count]) as GameObject;
        //cubeObj = GameObject.Instantiate(cubeObj) as GameObject;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            CubeSet();
            Destroy(this.gameObject);
        }
    }
}
