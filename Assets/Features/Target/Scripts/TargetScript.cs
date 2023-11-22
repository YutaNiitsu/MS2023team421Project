using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetScript : MonoBehaviour
{
    //�������łɂ͂܂��Ă��邩�ǂ���
    public bool Goaled { get; protected set; }
    //�͂܂��Ă��鐯���Q�Ƃ���
    public StarScript StarGoaled { get; protected set; }
    //���ʃ|�C���g�Ɏw�肳��Ă��邩�ǂ���
    public bool IsSpecialPoint { get; protected set; }
    // Start is called before the first frame update
    void Start()
    {
        Goaled = false;
        StarGoaled = null;
    }

    private void Update()
    {
        //�e�X�g�p
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Goaled = true;
            GameManagerScript.instance.AddScore(StarRarity.Normal, false);
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
            }

            StarGoaled = obj.GetComponent<StarScript>();

            //�X�R�A���Z
            StarRarity rare = StarGoaled.Rarity;

            GameManagerScript.instance.AddScore(rare, IsSpecialPoint);
            
            // �������\���ɂ���
            gameObject.SetActive(false);
        }
    }

    //�������̐ݒ�
    //isSpecialPoint : ���ʃ|�C���g�ɂ��邩�ǂ���
    public void Set(bool isSpecialPoint)
    {
        IsSpecialPoint = isSpecialPoint;
    }

    //���ʃ|�C���g�Ɏw�肳��Ă��鎞�A���j�[�N�ȏ�̃��A���e�B�̐����͂܂��Ă��邩�ǂ���
    public bool IsRareStarGoaledOnSpecialTarget()
    {
        if (IsSpecialPoint && Goaled && (int)StarGoaled.Rarity >= 2)
            return true;

        return false;
    }
}
