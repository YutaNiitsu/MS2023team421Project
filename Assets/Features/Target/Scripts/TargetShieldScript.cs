using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetShieldScript : MonoBehaviour
{
    private TargetScript Target;
    private CircleCollider2D Collider2D;
    private int HealthPoint;   //破壊されるまでの回数
    private int MaxHealthPoint;
    private Coroutine _Coroutine;
    private Animator ShieldAnimation;

    // Start is called before the first frame update
    void Start()
    {
        //HealthPoint = 1;
        Collider2D = transform.parent.gameObject.GetComponent<CircleCollider2D>();
        ShieldAnimation = gameObject.GetComponent<Animator>();
    }

    private void OnDestroy()
    {
        //シールド消える時の処理
        if (Collider2D != null)
            Collider2D.enabled = true;


    }

    //シールドにダメージ加える
    public void AddDamage()
    {
        HealthPoint--;
        ShieldAnimation.SetInteger("HP", HealthPoint);
        if (HealthPoint <= 0)
        {
            Destroy(gameObject, 0.5f);
        }
        
    }

    public void Set(int shieldHP)
    {
        HealthPoint = shieldHP;
        MaxHealthPoint = shieldHP;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Star"))
        {
            SoundManager.instance.PlaySE("HitShield");
            AddDamage();
        }
    }
}
