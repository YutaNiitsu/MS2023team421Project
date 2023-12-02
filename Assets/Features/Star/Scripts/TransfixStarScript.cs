using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class TransfixStarScript : MonoBehaviour
{
    [Header("�Փ˃p�[�e�B�N���̃v���n�u")]
    public ParticleSystem Particle;

    private StarScript _StarScript;
    private CircleCollider2D Collider2D;
    // Start is called before the first frame update
    void Start()
    {
        _StarScript = GetComponent<StarScript>();
        foreach (CircleCollider2D i in GetComponents<CircleCollider2D>())
        {
            //�g���K�[����Ȃ��@���Q��
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
                //�����Ă��鎞�̓V�[���h�����Փ�
                Collider2D.includeLayers = LayerMask.GetMask("Shield");
                Collider2D.excludeLayers = LayerMask.GetMask("Default");
            }
            else
            {
                //�����Ă��Ȃ����͑S���Փ�
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
        //�͂ߍ��ތ^�ɂ͂܂���Untagged�ɂȂ��Ă�����s���Ȃ�
        if (collision.CompareTag("Star") && gameObject.tag != "Untagged")
        {
            //�Փ˃p�[�e�B�N������
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
            //�Փ˃p�[�e�B�N������
            if (Particle != null)
            {
                ParticleSystem particle = Instantiate(Particle, gameObject.transform.position, new Quaternion());
                particle.Play();
                Destroy(particle.gameObject, 1.0f);
            }
            //��Q���̔j��񐔃J�E���g
            GameManagerScript.instance.DestroyObstacle();
            Destroy(collision.gameObject);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        
    }
}
