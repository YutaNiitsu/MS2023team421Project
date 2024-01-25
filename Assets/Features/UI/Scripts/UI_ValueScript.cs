using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class UI_ValueScript : MonoBehaviour
{
    public float FontSize;
    public int Value;
    public GameObject NumberTextPrefab;
    private List<TextMeshProUGUI> NumberTexts;

    // Start is called before the first frame update
    void Start()
    {
       
        SetValue(Value);
    }

    public void SetValue(int value)
    {
        if (NumberTexts == null)
            NumberTexts = new List<TextMeshProUGUI>();

        //åÖêîí≤Ç◊ÇÈ
        int digit = value.ToString().Length;
        int[] numbers = new int[0];
        if (digit == NumberTexts.Count)
        {
            //åÖêîìØÇ∂
            CalcNumbers(ref numbers, value);

        }
        else if (digit > NumberTexts.Count)
        {
            //åÖêîëùÇ¶ÇΩ
            int tmp = digit - NumberTexts.Count;
            CreateText(ref NumberTexts, tmp);
            CalcNumbers(ref numbers, value);
        }
        else if (digit < NumberTexts.Count)
        {
            //åÖêîå∏Ç¡ÇΩ
            int tmp = NumberTexts.Count - digit;
            for (int i = 0; i < NumberTexts.Count; i++)
            {
                Destroy(NumberTexts[i].gameObject);
                
            }
            transform.DetachChildren();
            NumberTexts.Clear();
            CreateText(ref NumberTexts, digit);
            CalcNumbers(ref numbers, value);
        }
        for (int i = 0; i < NumberTexts.Count; i++)
        {
            NumberTexts[i].text = "<sprite=" + numbers[NumberTexts.Count - i- 1].ToString() + ">";
        }
    }

    void CreateText(ref List<TextMeshProUGUI> texts, int addNum)
    {
        for (int i = 0; i < addNum; i++)
        {
            TextMeshProUGUI text = Instantiate(NumberTextPrefab).GetComponent<TextMeshProUGUI>();
            text.fontSize = FontSize;
            text.gameObject.transform.parent = transform;
            texts.Add(text);
        }
    }

    void CalcNumbers(ref int[] numbers, int value)
    {
        int digitnum = value.ToString().Length;
        numbers = new int[digitnum];
        for (int i = 0; i < digitnum; i++)
        {
            numbers[i] = value % 10;
            value /= 10;
        }
        
    }
}
