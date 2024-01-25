using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_Meteor : MonoBehaviour
{
    [SerializeField]
    float speed = 0.01f;
    public float Speed
    {
        get
        {
            return speed;
        }
        set
        {
            speed = value;
        }
    }
    void Update()
    {
        transform.position = new Vector2(transform.position.x * Time.deltaTime, transform.position.y - speed);
    }
    // �T�C�Y�𒲐�
    public void SetWall(Vector2 size)
    {
        transform.localScale = new Vector3(size.x, size.y, 1);
    }
    // ��ʊO�ɏo����j���i���e�X�g�v���C���ɃV�[���r���[�ɉf���Ă���Ɣj������Ȃ��̂Œ��Ӂj
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
