using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
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
    private void Update()
    {
        //if (gameObject == EventSystem.current.currentSelectedGameObject)
        //{
        //    Hover();
        //}
        //else
        //{
        //    UnHover();
        //}
    }

    //ボタンにホバーした時
    public void Hover()
    {
        if (Rect != null)
            Rect.localScale = new Vector3(HoveredScale, HoveredScale, 1.0f);
    }
    //ホバーされなくなった時
    public void UnHover()
    {
        if (Rect != null)
            Rect.localScale = new Vector3(1.0f, 1.0f, 1.0f);
    }

    private void OnApplicationFocus(bool focus)
    {
        
    }
}
