using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetShieldScript : MonoBehaviour
{
    [Header("�V�[���h�̉摜")]
    public Sprite[] Sprites;
    private TargetScript Target;
    private CircleCollider2D Collider2D;
    public int HealthPoint { get; protected set; }   //�j�󂳂��܂ł̉�
    public int MaxHealthPoint { get; protected set; }
    private SpriteRenderer Sprite;

    // Start is called before the first frame update
    void Start()
    {
        HealthPoint = 1;
        Collider2D = transform.parent.gameObject.GetComponent<CircleCollider2D>();
        Sprite = GetComponent<SpriteRenderer>();
    }

    private void OnDestroy()
    {
        //�V�[���h�����鎞�̏���
        if (Collider2D != null)
            //�͂ߍ��ތ^�̃R���C�_�[��L���ɂ���
            Collider2D.enabled = true;


    }

    //�V�[���h�Ƀ_���[�W������
    public void AddDamage()
    {
        HealthPoint--;
        if (HealthPoint <= 0)
        {
            Destroy(gameObject, 0.5f);
        }
        else
        {
            int index = 4 * (HealthPoint - 1) / MaxHealthPoint;
            Sprite.sprite = Sprites[index];
        }
    }
    //�V�[���h�̐ݒ�
    public void Set(int shield_HP)
    {
        HealthPoint = shield_HP;
        MaxHealthPoint = shield_HP;
        Sprite = GetComponent<SpriteRenderer>();
        Sprite.sprite = Sprites[3];
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
    }

}
