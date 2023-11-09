using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SelectButtonScript : MonoBehaviour
{
    //ボタンのテキスト
    public Text ButtonText;
    private Button _Button;
    private RectTransform rectTransform;

    // Start is called before the first frame update
    void Start()
    {
        _Button = GetComponent<Button>();
        rectTransform = GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(0f, 0f);
    }

    public void SetText(string text, int fontSize)
    {
        ButtonText.text = text;
        ButtonText.fontSize = fontSize;
    }
    public void SetSize(float width, float height)
    {
        rectTransform.sizeDelta = new Vector2(width, height);
        
    }

    public void AddClickAction(UnityAction call)
    {
        GetComponent<Button>().onClick.AddListener(call);
    }
}
