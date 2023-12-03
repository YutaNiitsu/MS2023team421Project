using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetShieldScript : MonoBehaviour
{
    private TargetScript Target;
    private CircleCollider2D Collider2D;

    // Start is called before the first frame update
    void Start()
    {
        Collider2D = transform.parent.gameObject.GetComponent<CircleCollider2D>();
    }

    private void OnDestroy()
    {
        if (Collider2D != null)
            Collider2D.enabled = true;
    }
}
