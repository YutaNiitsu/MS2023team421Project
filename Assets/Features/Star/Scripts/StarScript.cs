using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public enum StarRarity
{
    Normal,
    Rare,
    Unique,
    Legendary,
    [InspectorName("")]  StarRarityMax
}
public class StarScript : MonoBehaviour
{
   
    [Header("���̃��A���e�B")]
    public StarRarity Rarity;
    [Header("�ړ����̃p�[�e�B�N��")]
    public ParticleSystem MovingParticle;
    [Header("���Ŏ��̃p�[�e�B�N���̃v���n�u")]
    public ParticleSystem ExplosionParticle;

    public Rigidbody2D Rigidbody { get; protected set; }
    private bool IsPlaying;
    public bool IsMoving { get; protected set; }
    private CircleCollider2D Collider2D;

    //Start is called before the first frame update
    void Start()
    {
        //ParticleSystem.MainModule particle;
        //if (Particle != null)
        //{
        //    particle = Particle.main;
        //    particle.loop = false;
        //}
        IsPlaying = false;
        IsMoving = false;
        Rigidbody = gameObject.GetComponent<Rigidbody2D>();

        foreach (CircleCollider2D i in GetComponents<CircleCollider2D>())
        {
            //�g���K�[����Ȃ������Q��
            if (!i.isTrigger)
            {
                Collider2D = i;
            }
        }

        SetRarity();
    }

    // Update is called once per frame
    void Update()
    {
        //ParticleSystem.MainModule particle;
        //if (Particle != null) particle = Particle.main;
        if (Rigidbody != null)
        {
            if (Rigidbody.velocity.magnitude > 1.0f)
            {
                PlayParticle();
                IsMoving = true;
                //�����Ă��鎞�͓�����Q���ɏՓ�
                Collider2D.excludeLayers = LayerMask.GetMask("Nothing");
            }
            else
            {
                StopParticle();
                IsMoving = false;
                //�����Ă��Ȃ����͓�����Q���ɏՓ˂��Ȃ�
                Collider2D.excludeLayers = LayerMask.GetMask("MovableObstacle");
            }
        }
        
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 8)
        {
            if (ExplosionParticle != null)
            {
                ParticleSystem particle = Instantiate(ExplosionParticle, gameObject.transform.position, new Quaternion());
                particle.Play();
                Destroy(particle.gameObject, 1.0f);
            }
            Destroy(gameObject);
        }
    }

    public void PlayParticle()
    {
        if (MovingParticle != null && !IsPlaying)
        {
            IsPlaying = true;
            MovingParticle.Play();
            Debug.Log("PlayParticle");
        }
    }

    public void StopParticle()
    {
        if (MovingParticle != null && IsPlaying)
        {
            IsPlaying = false;
            MovingParticle.Stop();
            Debug.Log("StopParticle");
        }
    }

    public void SetRarity()
    {
        Rarity = GameManagerScript.instance.ProceduralGenerator.SetStarRarity(gameObject.transform.position);

        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        switch (Rarity)
        {
            case StarRarity.Normal:
                sprite.color = Color.white;
                break;
            case StarRarity.Rare:
                sprite.color = Color.green;
                break;
            case StarRarity.Unique:
                sprite.color = Color.blue;
                break;
            case StarRarity.Legendary:
                sprite.color = Color.yellow;
                break;
            default:
                break;
        }

    }
}
