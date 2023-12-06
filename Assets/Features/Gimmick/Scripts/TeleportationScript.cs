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

    public void Set(Vector2 gate1Pos, Vector2 gate2Pos, Color color)
    {
        Gate1.GetComponent<SpriteRenderer>().color = color;
        Gate2.GetComponent<SpriteRenderer>().color = color;

        Gate1.transform.position = gate1Pos;
        Gate2.transform.position = gate2Pos;
    }
}
