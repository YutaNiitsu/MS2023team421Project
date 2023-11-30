using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionStarScript : MonoBehaviour
{
    [Header("爆発のパーティクルのプレハブ")]
    public ParticleSystem Particle;
    [Header("爆発範囲")]
    public float Range;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Star"))
        {
            //GameManagerScript.instance.CollisionObstacle();
            //衝突パーティクル生成
            if (Particle != null)
            {
                ParticleSystem particle = Instantiate(Particle, gameObject.transform.position, new Quaternion());
                particle.Play();
                Destroy(particle.gameObject, 1.0f);
            }

        }
        if (collision.collider.CompareTag("Obstacle"))
        {
            //障害物の衝突回数カウント
            GameManagerScript.instance.CollisionObstacle();

        }
    }
}
