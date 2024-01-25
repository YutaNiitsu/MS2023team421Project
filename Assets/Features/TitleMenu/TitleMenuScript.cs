using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TitleMenuScript : MonoBehaviour
{
    [Header("最初にフォーカスするボタン")]
    [SerializeField] private GameObject FirstSelect;
    private DrawConstellationLine DrawLine;
    private MenuButtonScript[] Buttons;
    private ST_Constellation[] Targets;
    private Line[] Lines;

    private void Awake()
    {
        DrawLine = GetComponent<DrawConstellationLine>();
        RectTransform rect = GetComponent<RectTransform>();

        int buttonNum = transform.childCount;
        Buttons = new MenuButtonScript[buttonNum];
        Targets = new ST_Constellation[buttonNum];
        Lines = new Line[buttonNum - 1];

        for (int i = 0; i < buttonNum; i++)
        {
            Buttons[i] = transform.GetChild(i).gameObject.GetComponent<MenuButtonScript>();
            Vector3 screenPos = Buttons[i].gameObject.GetComponent<RectTransform>().position;
            screenPos.z = 5.0f;
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
            Targets[i].position = worldPos;
            Targets[i].Key = i;

            Buttons[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < buttonNum - 1; i++)
        {
            Lines[i].start = Targets[i].position;
            Lines[i].end = Targets[i + 1].position;
            Lines[i].startTargetKey = Targets[i].Key;
            Lines[i].endTargetKey = Targets[i + 1].Key;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (FirstSelect != null) 
            EventSystem.current.SetSelectedGameObject(FirstSelect);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Display(bool enable)
    {
        //表示
        if (enable)
        {
            if (FirstSelect != null)
                EventSystem.current.SetSelectedGameObject(FirstSelect);
            DrawLine.DrawLine(Targets, Lines);
            gameObject.SetActive(true);
            StartCoroutine(DisplayCoroutine());
        }
        //非表示
        else
        {
            if (FirstSelect != null)
                EventSystem.current.SetSelectedGameObject(null);
            DrawLine.DeleteLine();
            foreach (MenuButtonScript i in Buttons)
            {
                i.gameObject.SetActive(false);
            }
            gameObject.SetActive(false);
            
        }
    }

    //表示
    IEnumerator DisplayCoroutine()
    {

        yield return new WaitForSeconds(0);

        foreach (MenuButtonScript i in Buttons)
        {
            i.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.05f);
        }
    }
    //非表示
    IEnumerator NonDisplayCoroutine()
    {


        yield return new WaitForSeconds(0);

        foreach (MenuButtonScript i in Buttons)
        {
            i.gameObject.SetActive(false);
            //yield return new WaitForSeconds(0.1f);
        }
    }
}
