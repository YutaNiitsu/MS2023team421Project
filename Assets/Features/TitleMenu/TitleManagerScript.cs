using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManagerScript : MonoBehaviour
{
    public TitleMenuScript TitleMenu;
    public TitleMenuScript StageSelectMenu;
    public GameObject Credit;

    // Start is called before the first frame update
    void Start()
    {
        SoundManager.instance.PlayBGM("BGM1");
        TitleMenu.Display(true);
        StageSelectMenu.Display(false);
        Credit.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //�V�[���؂�ւ�
    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
        SoundManager.instance.StopBGM("BGM1");
    }
    //�X�e�[�W�Z���N�g���j���[�̕\��
    public void DisplayStageSelectMenu()
    {
        TitleMenu.Display(false);
        StageSelectMenu.Display(true);
    }
    //�^�C�g���ɖ߂�
    public void Back()
    {
        TitleMenu.Display(true);
        StageSelectMenu.Display(false);
        Credit.SetActive(false);
    }
    //�N���W�b�g
    public void DisplayCredit()
    {
        TitleMenu.Display(false);
        StageSelectMenu.Display(false);
        Credit.SetActive(true);
    }
    //�Q�[���I��
    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
      Application.Quit();
#endif
    }
}
