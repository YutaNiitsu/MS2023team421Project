using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionStarScript : MonoBehaviour
{
    [Header("�����̃p�[�e�B�N���̃v���n�u")]
    public ParticleSystem Particle;
    [Header("�����͈�")]
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
            //�Փ˃p�[�e�B�N������
            if (Particle != null)
            {
                ParticleSystem particle = Instantiate(Particle, gameObject.transform.position, new Quaternion());
                particle.Play();
                Destroy(particle.gameObject, 1.0f);
            }

        }
        if (collision.collider.CompareTag("Obstacle"))
        {
            //��Q���̏Փˉ񐔃J�E���g
            GameManagerScript.instance.CollisionObstacle();

        }
    }
}
