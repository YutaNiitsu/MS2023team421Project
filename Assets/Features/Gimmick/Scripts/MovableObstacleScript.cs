using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class MovableObstacleScript : MonoBehaviour
{
    private Rigidbody2D Rigidbody;
    // Start is called before the first frame update
    void Start()
    {
        Rigidbody = gameObject.GetComponent<Rigidbody2D>();
        Destroy(gameObject, 20.0f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

    }
}
