using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RareStarAreaScript : MonoBehaviour
{
    public GameObject RareStarAreaSprite;
    public GameObject Contents;
    private List<StarScript> Stars;
    // Start is called before the first frame update
    void Start()
    {
        //エリア表示用のスプライトを見えなくする
#if false
        RareStarAreaSprite.GetComponent<SpriteRenderer>().color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
#endif
        //Stars = new List<StarScript>();
        //int num = Contents.transform.childCount;
        //for (int i = 0; i < num; i++)
        //{
        //    if (Contents.transform.GetChild(i).gameObject.CompareTag("Star"))
        //    {
        //        Stars.Add(Contents.transform.GetChild(i).gameObject.GetComponent<StarScript>());
        //    }
        //}
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
