using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetScript : MonoBehaviour
{
    [Header("�X�v���C�g")]
    public Sprite normalSprite;
    public Sprite SpecialPointSprite;
    [Header("�V�[���h")]
    public GameObject Shield;
    [Header("�^�ɂ͂܂������̃G�t�F�N�g�̃v���n�u")]
    public GameObject CombinationParticle;
    //�������łɂ͂܂��Ă��邩�ǂ���
    public bool Goaled { get; protected set; }
    //�͂܂��Ă��鐯���Q�Ƃ���
    public StarScript StarGoaled { get; protected set; }
    //���ʃ|�C���g�Ɏw�肳��Ă��邩�ǂ���
    public bool IsSpecialPoint { get; protected set; }
    //�V�[���h���邩�ǂ���
    public bool IsShield { get; protected set; }
    private CircleCollider2D Collider2D;

    // Start is called before the first frame update
    void Start()
    {
        Goaled = false;
        StarGoaled = null;
        Collider2D = GetComponent<CircleCollider2D>();
        //������Q���ɏՓ˂��Ȃ�
        Collider2D.excludeLayers = LayerMask.GetMask("MovableObstacle");
    }

    private void Update()
    {
        //�e�X�g�p
        if (Input.GetKeyDown(KeyCode.F1))
        {
            Goaled = true;
            GameManagerScript.instance.StageManager.AddScore(StarRarity.Normal, false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject obj = collision.gameObject;

        // ����������
        
        if (obj.CompareTag("Star"))
        {
            obj.tag = "Untagged";
            Goaled = true;
            // �ʒu���^�[�Q�b�g�ɍ��킹��
            Vector3 pos = gameObject.transform.position;
            obj.transform.position = pos;

            Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                // ���x���O�ɂ���
                rb.velocity = Vector3.zero;
                //�Փ˂��Ă������Ȃ��悤�ɂ���
                rb.bodyType = RigidbodyType2D.Static;
                //������Q���ɏՓ˂��Ȃ�
                collision.excludeLayers = LayerMask.GetMask("MovableObstacle");
            }

            StarGoaled = obj.GetComponent<StarScript>();

            //�X�R�A���Z
            StarRarity rare = StarGoaled.Rarity;

            GameManagerScript.instance.StageManager.AddScore(rare, IsSpecialPoint);
            GameManagerScript.instance.StageManager.PutOnTareget();

            //�G�t�F�N�g����
            Instantiate(CombinationParticle, transform.position, new Quaternion());

            // �������\���ɂ���
            gameObject.SetActive(false);
        }
    }

    //�������̐ݒ�
    //isSpecialPoint : ���ʃ|�C���g�ɂ��邩�ǂ���
    //isShield : �V�[���h���邩�ǂ���
    public void Set(bool isSpecialPoint, bool isShield, int shieldHP)
    {
        IsSpecialPoint = isSpecialPoint;
        IsShield = isShield;

        //���ʃ|�C���g
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        if (isSpecialPoint)
        {
            sprite.sprite = SpecialPointSprite;
        }
        else
        {
            sprite.sprite = normalSprite;
        }

        //�V�[���h
        Collider2D = GetComponent<CircleCollider2D>();
        if (isShield)
        {
            //�V�[���h���͂�
            Shield.SetActive(true);
            Shield.GetComponent<TargetShieldScript>().Set(shieldHP);
            Collider2D.enabled = false;


        }
        else
        {
            Shield.SetActive(false);
            Collider2D.enabled = true;

        }
        
    }

    //���ʃ|�C���g�Ɏw�肳��Ă��鎞�A���j�[�N�ȏ�̃��A���e�B�̐����͂܂��Ă��邩�ǂ���
    public bool IsRareStarGoaledOnSpecialTarget()
    {
        if (IsSpecialPoint && Goaled 
            && StarGoaled != null &&(int)StarGoaled.Rarity >= 2)
            return true;

        return false;
    }
}
