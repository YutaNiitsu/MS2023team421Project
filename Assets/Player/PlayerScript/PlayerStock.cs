using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStock : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject cubeObj;
    int PlayerNum = 5;
    Vector2 StartPos;
    void Start()
    {
        StartPos = transform.position;
        PlayerNum = 5;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Sporn()
    {

        Debug.Log($"aaa{StartPos.x}");
        Debug.Log($"bbbb{PlayerNum}");
        //if (PlayerNum > 0)
        {
            //Destory(this.gameObject);
            //Debug.Log($"bbbbb{StartPos.x}");
            Instantiate(cubeObj, transform.position, Quaternion.identity);
            PlayerNum--;
        }
    }
}
