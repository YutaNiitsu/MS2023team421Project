using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetScript : MonoBehaviour
{
    private GameManagerScript GameManager;
    private bool Goal;
    //���ʃ|�C���g�Ɏw�肳��Ă��邩�ǂ���
    private bool IsSpecialPoint;
    // Start is called before the first frame update
    void Start()
    {
        Goal = false;
    }

    private void Update()
    {
        //�e�X�g�p
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Goal = true;
            GameManager.AddScore(StarRarity.Normal, false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject obj = collision.gameObject;

        // ����������
        if (obj.CompareTag("Star"))
        {
            obj.tag = "Untagged";
            Goal = true;
            // �ʒu���^�[�Q�b�g�ɍ��킹��
            Vector3 pos = gameObject.transform.position;
            obj.transform.position = pos;

            Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                // ���x���O�ɂ���
                rb.velocity = Vector3.zero;
            }

            //�X�R�A���Z
            StarRarity rare = obj.GetComponent<StarScript>().Rarity;
            GameManager.AddScore(rare, IsSpecialPoint);

            
            // �������\���ɂ���
            gameObject.SetActive(false);
        }
    }

    //�������̐ݒ�
    //gameManager : �Q�[���}�l�[�W���[���Q�Ƃ���
    //isSpecialPoint : ���ʃ|�C���g�ɂ��邩�ǂ���
    public void Set(GameManagerScript gameManager, bool isSpecialPoint)
    {
        GameManager = gameManager;
        IsSpecialPoint = isSpecialPoint;
    }

    public bool IsGoal()
    {
        return Goal;
    }
    public bool GetIsSpecialPoint()
    {
        return IsSpecialPoint;
    }
}
