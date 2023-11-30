using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class TransfixStarScript : MonoBehaviour
{
    [Header("�Փ˃p�[�e�B�N���̃v���n�u")]
    public ParticleSystem Particle;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //�͂ߍ��ތ^�ɂ͂܂���Untagged�ɂȂ��Ă�����s���Ȃ�
        if (collision.collider.CompareTag("Star") && gameObject.tag != "Untagged")
        {
            //GameManagerScript.instance.CollisionObstacle();
            //�Փ˃p�[�e�B�N������
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
            //�Փ˃p�[�e�B�N������
            if (Particle != null)
            {
                ParticleSystem particle = Instantiate(Particle, gameObject.transform.position, new Quaternion());
                particle.Play();
                Destroy(particle.gameObject, 1.0f);
            }
            //��Q���̔j��񐔃J�E���g
            GameManagerScript.instance.DestroyObstacle();
            Destroy(gameObject);
        }
    }
}
