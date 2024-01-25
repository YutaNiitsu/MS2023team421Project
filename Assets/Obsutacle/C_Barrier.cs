using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_Barrier : MonoBehaviour
{
	void OnCollisionEnter2D(Collision2D collision2D)
	{
		// Õ“Ë‚µ‚½‘Šè‚ÉPlayerƒ^ƒO‚ª•t‚¢‚Ä‚¢‚é‚Æ‚«
		if (collision2D.gameObject.tag == "Player")
		{
			// 0.2•bŒã‚ÉÁ‚¦‚é
			Destroy(gameObject, 0.2f);
		}
	}
}
