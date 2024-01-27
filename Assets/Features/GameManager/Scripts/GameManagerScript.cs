using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour
{
    //シングルトン
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

    //ゲーム開始時にステージマネージャーから参照
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
