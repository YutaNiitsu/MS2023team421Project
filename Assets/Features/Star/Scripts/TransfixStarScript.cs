using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class TransfixStarScript : MonoBehaviour
{
    [Header("衝突パーティクルのプレハブ")]
    public ParticleSystem Particle;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //はめ込む型にはまってUntaggedになってたら実行しない
        if (collision.collider.CompareTag("Star") && gameObject.tag != "Untagged")
        {
            //GameManagerScript.instance.CollisionObstacle();
            //衝突パーティクル生成
            if (Particle != null)
            {
                ParticleSystem particle = Instantiate(Particle, gameObject.transform.position, new Quaternion());
                particle.Play();
                Destroy(particle.gameObject, 1.0f);
            }
            Destroy(gameObject);
        }
        if (collision.collider.CompareTag("Obstacle"))
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
            Destroy(gameObject);
        }
    }
}
