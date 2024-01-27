using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;

public static class DiscordUtils
{
    [Serializable]
    private sealed class JsonData
    {
        public string content;
    }

    public static IEnumerator SendMessage
    (
        string url,
        string message,
        Action onSuccess = null,
        Action<string> onError = null
    )
    {
        var header = new Dictionary<string, string>
        {
            { "Content-Type", "application/json; charset=UTF-8" },
        };

        var jsonData = new JsonData
        {
            content = message,
        };

        var json = JsonUtility.ToJson(jsonData);
        var postData = Encoding.Default.GetBytes(json);
        var www = new WWW(url, postData, header);

        yield return www;

        var error = www.error;

        if (!string.IsNullOrEmpty(error))
        {
            onError?.Invoke(error);
            yield break;
        }

        onSuccess?.Invoke();
    }
}