using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_Barrier : MonoBehaviour
{
	void OnCollisionEnter2D(Collision2D collision2D)
	{
		// �Փ˂��������Player�^�O���t���Ă���Ƃ�
		if (collision2D.gameObject.tag == "Player")
		{
			// 0.2�b��ɏ�����
			Destroy(gameObject, 0.2f);
		}
	}
}
