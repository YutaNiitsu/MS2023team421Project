using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleScript : MonoBehaviour
{
    [Header("消滅自のパーティクルのプレハブ")]
    public ParticleSystem Particle;

    // Start is called before the first frame update
    void Start()
    {
        
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
