using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackGroundScript : MonoBehaviour
{
    private Material BG_Material;
    private GameObject Camera;
    private Vector2 Offset;

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

        Offset = new Vector2();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CameraScroll(float scrollSpeed)
    {
        Vector3 pos = Camera.transform.position;
        pos.z = 1.0f;
        gameObject.transform.position = pos;
        //UVÇÉXÉNÉçÅ[Éã
        var x = pos.x * scrollSpeed * 0.01f;
        var y = pos.y * scrollSpeed * 0.01f;
        var offset = new Vector2(x, y);
        BG_Material.SetTextureOffset("_MainTex", offset);
    }

    public void Scroll(Vector2 vel)
    {
        Offset += vel * 0.0001f;
        BG_Material.SetTextureOffset("_MainTex", Offset);
    }
}
