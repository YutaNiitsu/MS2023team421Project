using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.VFX;
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
    [Header("移動中のパーティクルの色")]
    public UnityEngine.Color MovingParticleColor;
    [Header("移動中の軌跡の色")]
    public UnityEngine.Color TrailColor1;
    public UnityEngine.Color TrailColor2;
    [Header("チャージ中のパーティクル")]
    public ParticleSystem ChargingParticle;
    public ParticleSystemRenderer ChargingParticleRenderer;
    [Header("チャージ中の稲妻の密度")]
    public float ChargingParticleDensity = 0.5f;

    public Rigidbody2D Rigidbody { get; protected set; }
    private bool IsPlaying;
    public bool IsMoving { get; protected set; }
    private CircleCollider2D Collider2D;
    private VisualEffect TrailHeadEffect;
    private TrailRenderer TrailEffect;
    private bool IsCharging;

    //Start is called before the first frame update
    void Start()
    {
        //スプライトとエフェクト類の描画順設定
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        sr.sortingOrder = 1;
        TrailEffect = MovingParticle.gameObject.GetComponent<TrailRenderer>();
        TrailEffect.sortingOrder = 0;
        ChargingParticleRenderer.sortingOrder = 2;

        TrailHeadEffect = MovingParticle.gameObject.GetComponent<VisualEffect>();
        TrailHeadEffect.Stop();
        //TrailHeadEffect.SetFloat("Size", 0);

        //軌跡の色設定
        TrailEffect.material.SetColor("_Color01", TrailColor1);
        TrailEffect.material.SetColor("_Color02", TrailColor2);
        TrailHeadEffect.SetVector4("Color", TrailColor2);

        ChargingParticle.Stop();

        IsPlaying = false;
        IsMoving = false;
        IsCharging = false;

        Rigidbody = gameObject.GetComponent<Rigidbody2D>();

        foreach (CircleCollider2D i in GetComponents<CircleCollider2D>())
        {
            //トリガーじゃない方を参照
            if (!i.isTrigger)
            {
                Collider2D = i;
            }
        }
        var main = MovingParticle.main;
        main.startColor = new ParticleSystem.MinMaxGradient(MovingParticleColor);
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
            TrailEffect.enabled = true;
            TrailHeadEffect.Play();
            TrailHeadEffect.SetFloat("Size", 5);
            ChargingParticle.Stop();
            IsCharging = false;
        }
    }

    public void StopParticle()
    {
        if (MovingParticle != null && IsPlaying)
        {
            IsPlaying = false;
            MovingParticle.Stop();
            TrailEffect.enabled = false;
            
            if (!IsCharging)
                TrailHeadEffect.Stop();

            //Debug.Log("StopParticle");
        }
    }

    public void SetRarity()
    {
        if (GameManagerScript.instance.StageManager.ProceduralGenerator != null) 
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
                //sprite.color = UnityEngine.Color.blue;
                break;
            case StarRarity.Legendary:
                obj = Instantiate(LegendaryParticle, gameObject.transform.position, new Quaternion());
                obj.transform.parent = gameObject.transform;
                //sprite.color = UnityEngine.Color.yellow;
                break;
            default:
                break;
        }

    }

    //発射チャージ中処理
    public void Charging(float power)
    {
        if (!IsCharging)
        {
            TrailHeadEffect.Play();
            ChargingParticle.Play();
        }
        IsCharging = true;
        TrailHeadEffect.SetFloat("Size", power);
        ParticleSystem.EmissionModule em = ChargingParticle.emission;
        em.rateOverTime = (int)power * power * ChargingParticleDensity;
        Debug.Log(power);
    }
}
