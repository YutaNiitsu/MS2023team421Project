using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ErrorReportScript : MonoBehaviour
{
    private bool OccurredException;
    private bool once;

    // Start is called before the first frame update
    void Start()
    {
        OccurredException = false;
        once = true;
    }

    private void Update()
    {
        try
        {
            if (OccurredException)
            {
                throw new Exception();
                
            }
        }
        catch(Exception ex)
        {
            //エラー発生
            ShowDialog();
#if !UNITY_EDITOR
            StartCoroutine(SendToMessage());
#endif
        }
    }
    
    void OnEnable()
    {
        if (!OccurredException)
            Application.logMessageReceived += HandleLog;
    }

    void OnDisable()
    {
        if (!OccurredException)
            Application.logMessageReceived -= HandleLog;
    }

    void HandleLog(string logString, string stackTrace, LogType type)
    {
        switch (type)
        {
            case LogType.Log:
                break;
            case LogType.Warning:
                break;
            case LogType.Assert:
                break;
            case LogType.Error:
                
                break;
            case LogType.Exception:
                OccurredException = true;
                break;
        }
    }

    static void ShowDialog()
    {
        GameManagerScript.instance.StageManager.UIManager.ErrorDialog.SetActive(true);

//        bool Ok = EditorUtility.DisplayDialog("Title", "予期せぬエラーが発生しました\r\n再起動してください", "OK");
        
//        if (Ok)
//        {
//#if UNITY_EDITOR
//            UnityEditor.EditorApplication.isPlaying = false;
//#else
//      Application.Quit();
//#endif
//        }
    }

    private IEnumerator SendToMessage()
    {
        if (once)
        {
            once = false;
            yield return DiscordUtils.SendMessage
       (
           url: "https://discord.com/api/webhooks/1200815206977257572/7GGAmQch9kQg-f-1qyL7uqVVORLrmwVYQ9FLtLl40qJ453Vr7BgFerVD34ME5iBSWuaF",
           message: "エラーが発生しました",
           onSuccess: () => Debug.Log("成功"),
           onError: error => Debug.LogError("失敗：" + error)
       );
            
        }
        yield return 0;
    }
}
