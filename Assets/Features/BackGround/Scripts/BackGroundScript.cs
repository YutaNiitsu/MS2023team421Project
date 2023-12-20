using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackGroundScript : MonoBehaviour
{
    public float ScrollSpeed;
    private Material BG_Material;
    private GameObject Camera;

    // Start is called before the first frame update
    void Start()
    {
        if (GetComponent<SpriteRenderer>() is SpriteRenderer i)
        {
            BG_Material = i.material;
        }
        if (GameObject.FindGameObjectWithTag("MainCamera") is GameObject camra)
        {
            Camera = camra;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //カメラに追従
        Vector3 pos = Camera.transform.position;
        pos.z = 1.0f;
        gameObject.transform.position = pos;
        //UVをスクロール
        var x = Camera.transform.position.x * ScrollSpeed * 0.01f;
        var y = Camera.transform.position.y * ScrollSpeed * 0.01f;
        var offset = new Vector2(x, y);
        BG_Material.SetTextureOffset("_MainTex", offset);
    }
}
