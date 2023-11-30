using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class TeleportationGateScript : MonoBehaviour
{
    public TeleportationGateScript OtherGate; //��������̃Q�[�g
    public bool IsExit { get; set; } //�o�����ǂ���
    private CircleCollider2D Collider2D;
    // Start is called before the first frame update
    void Start()
    {
        IsExit = false;
        Collider2D = GetComponent<CircleCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Star") && !IsExit)
        {
            OtherGate.IsExit = true;
            //�o���ɏd�Ȃ�Ȃ��悤�ɂ���
            Vector2 vel = collision.gameObject.GetComponent<Rigidbody2D>().velocity;
            vel = vel.normalized;
            Vector2 offset = vel * Collider2D.radius * 2;
            collision.transform.position = OtherGate.transform.position + new Vector3(offset.x, offset.y, 0.0f);
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Star") && IsExit)
        {
            IsExit = false;
        }
    }
}
