using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    [Header("�X�^�[�g�{�^�����������Ɉړ�����Scene�̖��O")]
    public string StartSceneName;

    // Start is called before the first frame update
    void Start()
    {
        SoundManager.instance.PlayBGM("BGM1");
    }

    public void SwitchScene()
    {
        SceneManager.LoadScene(StartSceneName, LoadSceneMode.Single);
        SoundManager.instance.StopBGM("BGM1");
    }
}
