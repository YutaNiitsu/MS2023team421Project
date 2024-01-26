using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleManagerScript : MonoBehaviour
{
    public TitleMenuScript TitleMenu;
    public TitleMenuScript StageSelectMenu;
    public GameObject Credit;
    public Vector2 BG_ScrollVelocity;
    private BackGroundScript BackGround;

    // Start is called before the first frame update
    void Start()
    {
        SoundManager.instance.PlayBGM("BGM1");
        TitleMenu.Display(true);
        StageSelectMenu.Display(false);
        Credit.SetActive(false);
        BackGround = GameObject.FindGameObjectWithTag("BackGround").GetComponent<BackGroundScript>();
    }

    // Update is called once per frame
    void Update()
    {
        BackGround.Scroll(BG_ScrollVelocity);
    }

    //シーン切り替え
    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
        SoundManager.instance.StopBGM("BGM1");
    }
    //ステージセレクトメニューの表示
    public void DisplayStageSelectMenu()
    {
        TitleMenu.Display(false);
        StageSelectMenu.Display(true);
    }
    //タイトルに戻る
    public void Back()
    {
        TitleMenu.Display(true);
        StageSelectMenu.Display(false);
        Credit.SetActive(false);
    }
    //クレジット
    public void DisplayCredit()
    {
        TitleMenu.Display(false);
        StageSelectMenu.Display(false);
        Credit.SetActive(true);
        EventSystem.current.SetSelectedGameObject(Credit.transform.GetChild(0).GetChild(0).gameObject);
    }
    //ゲーム終了
    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
      Application.Quit();
#endif
    }
}
