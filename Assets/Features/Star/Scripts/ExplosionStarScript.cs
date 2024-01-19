using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ExplosionStarScript : MonoBehaviour
{
    [Header("爆発のパーティクルのプレハブ")]
    public ParticleSystem Particle;

    private CircleCollider2D Collider2D;

    // Start is called before the first frame update
    void Start()
    {
        foreach (CircleCollider2D i in GetComponents<CircleCollider2D>())
        {
            //トリガーの方を参照
            if (i.isTrigger)
            {
                Collider2D = i;
            }
        }
        Collider2D.enabled = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //はめ込む型にはまってUntaggedになってたら実行しない
        if ((collision.collider.CompareTag("Star") || collision.collider.CompareTag("Obstacle")) && gameObject.tag != "Untagged")
        {
            //衝突パーティクル生成
            if (Particle != null)
            {
                ParticleSystem particle = Instantiate(Particle, gameObject.transform.position, new Quaternion());
                particle.Play();
                Destroy(particle.gameObject, 1.0f);
            }
            Collider2D.enabled = true;
            SoundManager.instance.PlaySE("Explosion");
            Destroy(gameObject, 0.1f);
        }
        
    }

    //爆破範囲内のオブジェクトを削除
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //はめ込む型にはまってUntaggedになってたら実行しない
        if ((collision.CompareTag("Star") || collision.CompareTag("Obstacle")) 
            && gameObject.tag != "Untagged" && collision.gameObject.GetComponent<ExplosionStarScript>() == null)
        {
            Destroy(collision.gameObject);
        }
    }
}
