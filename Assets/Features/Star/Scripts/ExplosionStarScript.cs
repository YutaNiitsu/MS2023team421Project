using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionStarScript : MonoBehaviour
{
    [Header("�����̃p�[�e�B�N���̃v���n�u")]
    public ParticleSystem Particle;

    private List<GameObject> DestroyList;

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
            foreach (GameObject i in DestroyList)
            {
                Destroy(i);
            }
        }
        if (collision.collider.CompareTag("Obstacle"))
        {
            //��Q���̏Փˉ񐔃J�E���g
            GameManagerScript.instance.StageManager.CollisionObstacle();

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //�͂ߍ��ތ^�ɂ͂܂���Untagged�ɂȂ��Ă�����s���Ȃ�
        if (collision.CompareTag("Star") && gameObject.tag != "Untagged")
        {
            DestroyList.Add(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //�͂ߍ��ތ^�ɂ͂܂���Untagged�ɂȂ��Ă�����s���Ȃ�
        if (collision.CompareTag("Star") && gameObject.tag != "Untagged")
        {
            DestroyList.Remove(collision.gameObject);
        }
    }
}
