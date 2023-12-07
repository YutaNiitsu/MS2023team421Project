using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetShieldScript : MonoBehaviour
{
    private TargetScript Target;
    private CircleCollider2D Collider2D;
    private int HealthPoint;   //�j�󂳂��܂ł̉�

    // Start is called before the first frame update
    void Start()
    {
        HealthPoint = 1;
        Collider2D = transform.parent.gameObject.GetComponent<CircleCollider2D>();
    }

    private void OnDestroy()
    {
        //�V�[���h�����鎞�̏���
        if (Collider2D != null)
            Collider2D.enabled = true;


    }

    //�V�[���h�Ƀ_���[�W������
    public void AddDamage()
    {
        HealthPoint--;
        Destroy(gameObject, 0.5f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
    }

}
