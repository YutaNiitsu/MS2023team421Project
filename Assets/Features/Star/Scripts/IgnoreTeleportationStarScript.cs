using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class IgnoreTeleportationStarScript : MonoBehaviour
{
    [Header("���Ŏ��̃p�[�e�B�N���̃v���n�u")]
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
            SoundManager.instance.PlaySE("Explosion");
            //GameManagerScript.instance.CollisionObstacle();
            //�Փ˃p�[�e�B�N������
            if (Particle != null)
            {
                ParticleSystem particle = Instantiate(Particle, gameObject.transform.position, new Quaternion());
                particle.Play();
                Destroy(particle.gameObject, 1.0f);
            }
            Destroy(gameObject);
            //Destroy(collision.collider.gameObject);

        }
        if (collision.collider.CompareTag("Obstacle"))
        {
            //��Q���̏Փˉ񐔃J�E���g
            GameManagerScript.instance.StageManager.CollisionObstacle();

        }
    }
}
