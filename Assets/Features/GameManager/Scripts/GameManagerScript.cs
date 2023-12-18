using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
