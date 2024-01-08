using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuButtonScript : MonoBehaviour
{
    public Button ButtonObject { get; protected set; }
    [Header("ホバー時のボタンのサイズスケール")]
    public float HoveredScale;
    private RectTransform Rect;

    // Start is called before the first frame update
    void Start()
    {
        ButtonObject = transform.GetChild(0).gameObject.GetComponent<Button>();
        Rect = ButtonObject.GetComponent<RectTransform>();
    }

    //ボタンにホバーした時
    public void Hover()
    {
        Rect.localScale = new Vector3(HoveredScale, HoveredScale, 1.0f);
    }
    //ホバーされなくなった時
    public void UnHover()
    {
        Rect.localScale = new Vector3(1.0f, 1.0f, 1.0f);
    }
}
