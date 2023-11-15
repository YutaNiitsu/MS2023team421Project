using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetScript : MonoBehaviour
{
    private GameManagerScript GameManager;
    private bool Goal;
    // Start is called before the first frame update
    void Start()
    {
        Goal = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Goal = true;
            GameManager.AddScore(1);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject obj = collision.gameObject;

        // ����������
        if (obj.CompareTag("Star"))
        {
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
            int rare = obj.GetComponent<StarScript>().Rarity;
            GameManager.AddScore(rare);

            
            // �������\���ɂ���
            gameObject.SetActive(false);
        }
    }

    public void Set(GameManagerScript gameManager)
    {
        GameManager = gameManager;
    }

    public bool IsGoal()
    {
        return Goal;
    }
}
