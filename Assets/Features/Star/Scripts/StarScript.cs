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
   
    [Header("���̃��A���e�B")]
    public StarRarity Rarity;
    [Header("�ړ����̃p�[�e�B�N���̃v���n�u")]
    public ParticleSystem Particle;
   

    public Rigidbody2D Rigidbody { get; protected set; }
    private bool IsPlaying;

    //Start is called before the first frame update
    void Start()
    {
        //ParticleSystem.MainModule particle;
        //if (Particle != null)
        //{
        //    particle = Particle.main;
        //    particle.loop = false;
        //}
        IsPlaying = false;
        Rigidbody = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //ParticleSystem.MainModule particle;
        //if (Particle != null) particle = Particle.main;
        if (Rigidbody != null)
        {
            if (Rigidbody.velocity.magnitude > 1.0f)
            {
                PlayParticle();
            }
            else
            {
                StopParticle();
            }
        }
        
    }

    //��Q���Փ˃e�X�g�p
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if (collision.collider.CompareTag("Obstacle"))
        //{
        //    GameManagerScript.instance.CollisionObstacle();

        //}
    }

    public void PlayParticle()
    {
        if (Particle != null && !IsPlaying)
        {
            IsPlaying = true;
            Particle.Play();
            Debug.Log("PlayParticle");
        }
    }

    public void StopParticle()
    {
        if (Particle != null && IsPlaying)
        {
            IsPlaying = false;
            Particle.Stop();
            Debug.Log("StopParticle");
        }
    }
}
