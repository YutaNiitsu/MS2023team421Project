using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class ResultScript : MonoBehaviour
{
    public Image ConstellationImage;
    public Text ScoreText;
    public GameObject ButtonObjects;
    public float FadeSpeed;

    private float ImageAlpha;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void DisplayResult()
    {
        ImageAlpha = 0.0f;
        ScoreText.gameObject.SetActive(false);
        ButtonObjects.SetActive(false);
        ConstellationImage.sprite = GameManagerScript.instance.Setting.ConstellationImage;
        ConstellationImage.color = new Color(1.0f, 1.0f, 1.0f, ImageAlpha);
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        while (ImageAlpha < 1.0f)
        {
            ImageAlpha += FadeSpeed;
            ConstellationImage.color = new Color(1.0f, 1.0f, 1.0f, ImageAlpha);
            yield return new WaitForSeconds(0.02f);
        }

        yield return new WaitForSeconds(0.0f);
        ScoreText.gameObject.SetActive(true);
        ScoreText.text = GameManagerScript.instance.Score.ToString();
        ButtonObjects.SetActive(true);
        Debug.Log("ƒŠƒUƒ‹ƒg•\Ž¦Š®—¹");

    }
}
