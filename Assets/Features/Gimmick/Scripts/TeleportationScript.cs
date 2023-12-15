using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class TeleportationScript : MonoBehaviour
{
    public UnityEngine.Color GateColor;
    public GameObject Gate1;
    public GameObject Gate2;

    // Start is called before the first frame update
    void Start()
    {
        Gate1.GetComponent<SpriteRenderer>().color = GateColor;
        Gate2.GetComponent<SpriteRenderer>().color = GateColor;
    }

    public void Set(Vector2 gate1Pos, Vector2 gate2Pos, UnityEngine.Color color)
    {
        Gate1.GetComponent<SpriteRenderer>().color = color;
        Gate2.GetComponent<SpriteRenderer>().color = color;
        GateColor = color;
        Gate1.transform.position = gate1Pos;
        Gate2.transform.position = gate2Pos;
    }
}
