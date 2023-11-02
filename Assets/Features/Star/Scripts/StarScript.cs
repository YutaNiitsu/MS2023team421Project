using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarScript : MonoBehaviour
{
    public int Rarity;
    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        //AddForce(new Vector2(0, -1), 100);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void AddForce(Vector2 direction, float power)
    {
        // —Í‚ð‰Á‚¦‚é
        rb.AddForce(direction * power);
    }
}
