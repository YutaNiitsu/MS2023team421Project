using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetInCreateModeScript : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public int Index{ get; protected set; }
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetIsSelected(bool enable)
    {
        if (enable)
        {
            spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
        }
        else
        {
            spriteRenderer.color = new Color(1f, 0f, 0f, 1f);
        }
    }
    public void SetIndex(int index)
    {
        Index = index;
    }
}
