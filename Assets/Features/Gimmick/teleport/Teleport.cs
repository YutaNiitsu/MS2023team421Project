using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{

    [SerializeField]
    GameObject teleport;

   

    float Counter = 0.0f;

    //  GameObject player;

    //[SerializeField]
    //GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        //  player = GameObject.FindGameObjectWithTag("Player");



    }

    // Update is called once per frame
    void Update()
    {
        if (Gimic_data.TeleportCooltime)
        {
            Counter += Time.deltaTime;
            if (Counter>1.0f)
            {
                Gimic_data.TeleportCooltime = false;
                teleport.SetActive(true);
                Counter = 0.0f;
            }

        }



    }



    private void OnTriggerEnter2D(Collider2D other)
    {
        //
        if (other.gameObject.tag == "Player" && !Gimic_data.TeleportCooltime)
        {
            Debug.Log("テレポートします");
            other.transform.position = teleport.transform.position;
            teleport.SetActive(false);
            Gimic_data.TeleportCooltime = true;
        }
    }


}
