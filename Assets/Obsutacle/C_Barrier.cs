using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_Barrier : MonoBehaviour
{
	void OnCollisionEnter2D(Collision2D collision2D)
	{
		// 衝突した相手にPlayerタグが付いているとき
		if (collision2D.gameObject.tag == "Player")
		{
			// 0.2秒後に消える
			Destroy(gameObject, 0.2f);
		}
	}
}
