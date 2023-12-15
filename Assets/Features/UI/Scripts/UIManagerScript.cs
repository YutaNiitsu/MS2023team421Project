using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManagerScript : MonoBehaviour
{
    public GameObject HUD;
    public GameObject MiniMap;
    public GameObject WarningMark;
    public GameObject Result;
    public GameObject Pause;
    public GameObject GameOver;

    private StageManagerScript StageManager;

    // Start is called before the first frame update
    void Start()
    {
        HUD.SetActive(true);
        MiniMap.SetActive(true);
        WarningMark.SetActive(true);
        Result.SetActive(false);
        Pause.SetActive(false);
        GameOver.SetActive(false);

        StageManager = GameManagerScript.instance.StageManager;
    }

    //リザルト表示
    public void DisplayResult()
    {
        
        HUD.SetActive(false);
        MiniMap.SetActive(false);
        WarningMark.SetActive(false);
        Result.SetActive(true);
        Result.GetComponent<ResultScript>().DisplayResult();
    }

    public void DisplayPauseMenu()
    {
        HUD.SetActive(false);
        MiniMap.SetActive(false);
        WarningMark.SetActive(false);
        Result.SetActive(false);
        Pause.SetActive(true);
    }

    public void HiddenPauseMenu()
    {
        HUD.SetActive(true);
        MiniMap.SetActive(true);
        WarningMark.SetActive(true);
        Result.SetActive(false);
        Pause.SetActive(false);
    }

    public void DisplayGameOver()
    {
        HUD.SetActive(false);
        MiniMap.SetActive(false);
        WarningMark.SetActive(false);
        GameOver.SetActive(true);
    }


    //ボタンアクション
    //次のシーンへ移行
    public void NextScene()
    {
        SoundManager.instance.PlaySE("Select");
        SceneManager.LoadScene(StageManager.NextSceneName);
    }

    public void Retry()
    {
        SoundManager.instance.PlaySE("Select");
        string sceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(sceneName);
    }

    public void TitleScene()
    {
        SoundManager.instance.PlaySE("Select");
        SceneManager.LoadScene("Title");
    }

    public void PauseGame()
    {
        SoundManager.instance.PlaySE("Select");
        if (Time.timeScale != 0)
        {
            StageManager.UIManager.DisplayPauseMenu();
            Time.timeScale = 0;
        }
        else
        {
            StageManager.UIManager.HiddenPauseMenu();
            Time.timeScale = 1;
        }
    }
    public void ResumeGame()
    {
        SoundManager.instance.PlaySE("Select");
        StageManager.UIManager.HiddenPauseMenu();
        Time.timeScale = 1;
    }
}
