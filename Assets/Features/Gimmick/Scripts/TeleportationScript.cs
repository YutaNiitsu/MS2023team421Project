using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportationScript : MonoBehaviour
{
    public GameObject Gate1;
    public GameObject Gate2;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Set(Color color)
    {
        Gate1.GetComponent<SpriteRenderer>().color = color;
        Gate2.GetComponent<SpriteRenderer>().color = color;
    }
}
