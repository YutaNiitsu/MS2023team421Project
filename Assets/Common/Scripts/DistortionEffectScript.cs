using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistortionEffectScript : MonoBehaviour
{
    [Header("˜c‚Ý‰Q‚Ì–§“x")]
    [SerializeField, Range(0f, 1f)]
    public float Density;
    private SpriteRenderer Sprite;
    // Start is called before the first frame update
    void Start()
    {
        Sprite = GetComponent<SpriteRenderer>();
        Sprite.material.SetFloat("_density", Density);
    }

    public void SetDensity(float value)
    {
        Sprite.material.SetFloat("_density", value);
    }
}
