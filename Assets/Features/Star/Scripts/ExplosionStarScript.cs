using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionStarScript : MonoBehaviour
{
    [Header("爆発のパーティクルのプレハブ")]
    public ParticleSystem Particle;

    private List<GameObject> DestroyList;

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
            foreach (GameObject i in DestroyList)
            {
                Destroy(i);
            }
        }
        if (collision.collider.CompareTag("Obstacle"))
        {
            //障害物の衝突回数カウント
            GameManagerScript.instance.StageManager.CollisionObstacle();

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //はめ込む型にはまってUntaggedになってたら実行しない
        if (collision.CompareTag("Star") && gameObject.tag != "Untagged")
        {
            DestroyList.Add(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //はめ込む型にはまってUntaggedになってたら実行しない
        if (collision.CompareTag("Star") && gameObject.tag != "Untagged")
        {
            DestroyList.Remove(collision.gameObject);
        }
    }
}
