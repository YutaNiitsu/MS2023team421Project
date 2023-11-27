using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum StarRarity
{
    Normal,
    Rare,
    Unique,
    Legendary,
    [InspectorName("")]  StarRarityMax
}
public class StarScript : MonoBehaviour
{
   
    [Header("星のレアリティ")]
    public StarRarity Rarity;
    public ParticleSystem Particle;
    private Rigidbody2D rb;

    //Start is called before the first frame update
    void Start()
    {
        //ParticleSystem.MainModule particle;
        //if (Particle != null)
        //{
        //    particle = Particle.main;
        //    particle.loop = false;
        //}
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //ParticleSystem.MainModule particle;
        //if (Particle != null) particle = Particle.main;
        if (rb != null)
        {
            if (Vector2.Dot(rb.velocity, rb.velocity) > 1.0f)
            {
                PlayParticle();
            }
            else
            {
                StopParticle();
            }
        }
        
    }

    //障害物衝突テスト用
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Obstacle"))
        {
            GameManagerScript.instance.CollisionObstacle();

        }
    }

    public void PlayParticle()
    {
        if (Particle != null && !Particle.isPlaying)
        {
            Particle.Play();
            Debug.Log("PlayParticle");
        }
    }

    public void StopParticle()
    {
        if (Particle != null && Particle.isPlaying)
        {
            Particle.Stop();
            Debug.Log("StopParticle");
        }
    }
}
