using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject obj = collision.gameObject;

        // ����������
        if (obj.CompareTag("Star"))
        {
            // �ʒu���^�[�Q�b�g�ɍ��킹��
            Vector3 pos = gameObject.transform.position;
            obj.transform.position = pos;

            Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                // ���x���O�ɂ���
                rb.velocity = Vector3.zero;
            }

            // �������\���ɂ���
            gameObject.SetActive(false);
        }
    }
}
