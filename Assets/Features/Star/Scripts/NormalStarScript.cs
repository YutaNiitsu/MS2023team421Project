using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalStarScript : MonoBehaviour
{
    [Header("消滅自のパーティクルのプレハブ")]
    public ParticleSystem Particle;

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
            ParticleSystem particle = Instantiate(Particle, gameObject.transform.position, new Quaternion());
            particle.Play();
            Destroy(gameObject);
            Destroy(collision.collider.gameObject);
            Destroy(particle.gameObject, 1.0f);
        }
    }
}
