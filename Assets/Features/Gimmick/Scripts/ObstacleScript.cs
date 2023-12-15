using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleScript : MonoBehaviour
{
    [Header("消滅自のパーティクルのプレハブ")]
    public ParticleSystem Particle;
    public Sprite[] Sprites;

    private SpriteRenderer Sprite;
    // Start is called before the first frame update
    void Start()
    {
        Sprite = GetComponent<SpriteRenderer>();
        int rand = UnityEngine.Random.Range(0, Sprites.Length - 1);
        Sprite.sprite = Sprites[rand];
    }

    public void OnDestroy()
    {
        //パーティクル生成
        if (Particle != null)
        {
            ParticleSystem particle = Instantiate(Particle, gameObject.transform.position, new Quaternion());
            particle.Play();
            Destroy(particle.gameObject, 1.0f);
        }
    }
}
