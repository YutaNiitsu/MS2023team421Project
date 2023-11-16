using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class DarkHole : MonoBehaviour
{


    GameObject Player;



    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {

        if (GameOver.isGameOver)
        {
            SceneManager.LoadScene("game_over");
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        //
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("è¡Ç¶Ç‹Ç∑");
            Player.SetActive(false);
            GameOver.isGameOver = true;
        }
    }



}
