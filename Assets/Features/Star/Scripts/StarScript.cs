using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum StarRarity
{
    Normal,
    Rare,
    Unique,
    Legendary,
    [InspectorName("")]  StarRarityMax
}
public class StarScript : MonoBehaviour
{
   
    [Header("êØÇÃÉåÉAÉäÉeÉB")]
    public StarRarity Rarity;
  
    //private Rigidbody2D rb;
    // Start is called before the first frame update
    //void Start()
    //{
    //    rb = gameObject.GetComponent<Rigidbody2D>();
    //    //AddForce(new Vector2(0, -1), 100);
    //}

    //// Update is called once per frame
    //void Update()
    //{

    //}

}
