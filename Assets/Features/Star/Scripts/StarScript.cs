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
   
    [Header("星のレアリティ")]
    public StarRarity Rarity;
    [Header("星のレアリティごとのパーティクルのプレハブ")]
    public GameObject RareParticle;
    public GameObject UniqueParticle;
    public GameObject LegendaryParticle;
    [Header("移動中のパーティクル")]
    public ParticleSystem MovingParticle;
    [Header("消滅自のパーティクルのプレハブ")]
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
            //トリガーじゃない方を参照
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
                //動いている時は動く障害物に衝突
                Collider2D.excludeLayers = LayerMask.GetMask("Nothing");
            }
            else
            {
                StopParticle();
                IsMoving = false;
                //動いていない時は動く障害物に衝突しない
                Collider2D.excludeLayers = LayerMask.GetMask("MovableObstacle");
            }
        }
        
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        //動く障害物に衝突
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
        Rarity = GameManagerScript.instance.StageManager.ProceduralGenerator.SetStarRarity(gameObject.transform.position);
        GameObject obj;
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();

        //パーティクル生成して子にする
        switch (Rarity)
        {
            case StarRarity.Normal:
                //sprite.color = Color.white;
                break;
            case StarRarity.Rare:
                obj = Instantiate(RareParticle, gameObject.transform.position, new Quaternion());
                obj.transform.parent = gameObject.transform;
                //sprite.color = Color.green;
                break;
            case StarRarity.Unique:
                obj = Instantiate(UniqueParticle, gameObject.transform.position, new Quaternion());
                obj.transform.parent = gameObject.transform;
                sprite.color = Color.blue;
                break;
            case StarRarity.Legendary:
                obj = Instantiate(LegendaryParticle, gameObject.transform.position, new Quaternion());
                obj.transform.parent = gameObject.transform;
                sprite.color = Color.yellow;
                break;
            default:
                break;
        }

    }
}
