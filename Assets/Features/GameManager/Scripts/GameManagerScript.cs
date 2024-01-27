using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour
{
    //�V���O���g��
    public static GameManagerScript instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public StageManagerScript StageManager { get; protected set; }

    //�Q�[���J�n���ɃX�e�[�W�}�l�[�W���[����Q��
    public void Set(StageManagerScript stageManager)
    {
        StageManager = stageManager;
    }

    private void Update()
    {
        SceneManager.sceneLoaded += (Scene scene, LoadSceneMode type) => {
            if (scene.name == "errorScene")
            {
                Destroy(gameObject);
            }
        };
    }
}
