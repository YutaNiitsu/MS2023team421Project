using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManagerScript : MonoBehaviour
{
    public GameObject HUD;
    public GameObject MiniMap;
    public GameObject WarningMark;
    public GameObject Result;
    public GameObject Pause;
    public GameObject GameOver;
    public Button PauseFocusButton;
    public Button ResultFocusButton;
    public Button GameOverFocusButton;

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
        ResultFocusButton.Select();
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
        GameOverFocusButton.Select();
        ResultFocusButton.Select();
        HUD.SetActive(false);
        MiniMap.SetActive(false);
        WarningMark.SetActive(false);
        GameOver.SetActive(true);
    }

    public void SetUIActive(bool hud, bool miniMap, bool warningMark, bool result, bool pause, bool gameOver)
    {
        HUD.SetActive(hud);
        MiniMap.SetActive(miniMap);
        WarningMark.SetActive(warningMark);
        Result.SetActive(result);
        Pause.SetActive(pause);
        GameOver.SetActive(gameOver);
    }

    //ボタンアクション
    //次のシーンへ移行
    public void NextScene()
    {
        SoundManager.instance.StopBGM(GameManagerScript.instance.StageManager.BGM_Name);
        SoundManager.instance.PlaySE("Select");
        SceneManager.LoadScene(StageManager.NextSceneName);
        Time.timeScale = 1;
    }

    public void Retry()
    {
        SoundManager.instance.StopBGM(GameManagerScript.instance.StageManager.BGM_Name);
        SoundManager.instance.PlaySE("Select");
        string sceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(sceneName); 
        Time.timeScale = 1;
    }

    public void TitleScene()
    {
        SoundManager.instance.StopBGM(GameManagerScript.instance.StageManager.BGM_Name);
        SoundManager.instance.PlaySE("Select");
        SceneManager.LoadScene("Title");
        Time.timeScale = 1;
    }

    public void PauseGame()
    {

        SoundManager.instance.PlaySE("Select");
        if (Time.timeScale != 0)
        {
            PauseFocusButton.Select();
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
