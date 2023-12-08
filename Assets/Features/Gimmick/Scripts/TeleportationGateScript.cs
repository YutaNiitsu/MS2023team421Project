using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;
using static UnityEngine.ParticleSystem;

public class TeleportationGateScript : MonoBehaviour
{
    public TeleportationGateScript OtherGate; //��������̃Q�[�g
    public bool IsExit { get; set; } //�o�����ǂ���
    private CircleCollider2D Collider2D;
    private int Frame;

    // Start is called before the first frame update
    void Start()
    {
        IsExit = false;
        Frame = 0;
        Collider2D = GetComponent<CircleCollider2D>();
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Star") && !IsExit)
        {
            OtherGate.IsExit = true;
            //�o���ɏd�Ȃ�Ȃ��悤�ɂ���
            Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
            Vector2 vel = rb.velocity;
            vel = vel.normalized;
            Vector2 offset = vel * Collider2D.radius * 2;
            Vector3 endPos = OtherGate.transform.position + new Vector3(offset.x, offset.y, 0.0f);
            GameObject camera = GameObject.FindGameObjectWithTag("MainCamera");
            //�o���Ɉړ�
            StartCoroutine(StartTeleportation(collision.gameObject, camera, endPos, rb));
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Star") && IsExit)
        {
            IsExit = false;
        }
    }

    IEnumerator StartTeleportation(GameObject star, GameObject camera, Vector3 exitPos, Rigidbody2D rb)
    {
        Vector2 vel = new Vector2(rb.velocity.x, rb.velocity.y);
        star.transform.position = gameObject.transform.position;
        rb.velocity = Vector2.zero;
        //���鎞
        while (Frame < 30)
        {
            yield return new WaitForSeconds(1.0f / 60.0f);
            Frame++;
        }
        star.SetActive(false);
        Frame = 0;
        //�o���܂ŃJ�����ړ�
        while (Frame < 30)
        {
            Vector3 pos = new Vector3(exitPos.x, exitPos.y, camera.transform.position.z);
            camera.transform.position = Vector3.Lerp(camera.transform.position, pos, (float)Frame / 30.0f);
            yield return new WaitForSeconds(1.0f / 60.0f);
            Frame++;
        }

        //�o��Ƃ�
        Frame = 0;
        star.SetActive(true);
        star.transform.position = exitPos;
        while (Frame < 30)
        {
            yield return new WaitForSeconds(1.0f / 60.0f);
            Frame++;
        }
        yield return new WaitForSeconds(0);
        rb.velocity = vel;
    }
}
