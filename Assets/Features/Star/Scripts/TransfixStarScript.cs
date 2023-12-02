using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class TransfixStarScript : MonoBehaviour
{
    [Header("衝突パーティクルのプレハブ")]
    public ParticleSystem Particle;

    private StarScript _StarScript;
    private CircleCollider2D Collider2D;
    // Start is called before the first frame update
    void Start()
    {
        _StarScript = GetComponent<StarScript>();
        foreach (CircleCollider2D i in GetComponents<CircleCollider2D>())
        {
            //トリガーじゃない法を参照
            if (!i.isTrigger)
            {
                Collider2D = i;
            }
        }
    }

    private void Update()
    {
        if (_StarScript != null)
        {
            if (_StarScript.IsMoving)
            {
                //動いている時はシールドだけ衝突
                Collider2D.includeLayers = LayerMask.GetMask("Shield");
                Collider2D.excludeLayers = LayerMask.GetMask("Default");
            }
            else
            {
                //動いていない時は全部衝突
                Collider2D.includeLayers = LayerMask.GetMask("Nothing");
                Collider2D.excludeLayers = LayerMask.GetMask("Nothing");
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //はめ込む型にはまってUntaggedになってたら実行しない
        if (collision.CompareTag("Star") && gameObject.tag != "Untagged")
        {
            //衝突パーティクル生成
            if (Particle != null)
            {
                ParticleSystem particle = Instantiate(Particle, gameObject.transform.position, new Quaternion());
                particle.Play();
                Destroy(particle.gameObject, 1.0f);
            }
            Destroy(collision.gameObject);
        }
        if (collision.CompareTag("Obstacle"))
        {
            //衝突パーティクル生成
            if (Particle != null)
            {
                ParticleSystem particle = Instantiate(Particle, gameObject.transform.position, new Quaternion());
                particle.Play();
                Destroy(particle.gameObject, 1.0f);
            }
            //障害物の破壊回数カウント
            GameManagerScript.instance.DestroyObstacle();
            Destroy(collision.gameObject);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        
    }
}
