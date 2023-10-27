using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtarTestScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Rigidbody2D rb = gameObject.GetComponent<Rigidbody2D>();
        rb.AddForceY(-20);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
