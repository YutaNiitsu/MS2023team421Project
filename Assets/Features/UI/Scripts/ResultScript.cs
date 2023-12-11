using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;
using static MissionScript;

public class ResultScript : MonoBehaviour
{
    public Image ConstellationImage;
    public Text ScoreText;
    public GameObject ButtonObjects;
    public MissionResultManagerScript missionResult;
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
        missionResult.gameObject.SetActive(false);
        ConstellationImage.sprite = GameManagerScript.instance.StageManager.Setting.ConstellationImage;
        ConstellationImage.color = new Color(1.0f, 1.0f, 1.0f, ImageAlpha);
        StartCoroutine(FadeIn());
    }

    public void AddMissionResult(bool IsComp, MissionType type)
    {
        //çÄñ⁄í«â¡
        missionResult.AddContent(IsComp, type);
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
        ScoreText.text = GameManagerScript.instance.StageManager.Score.ToString();
        ButtonObjects.SetActive(true);
        missionResult.gameObject.SetActive(true);
        Debug.Log("ÉäÉUÉãÉgï\é¶äÆóπ");

    }
}
