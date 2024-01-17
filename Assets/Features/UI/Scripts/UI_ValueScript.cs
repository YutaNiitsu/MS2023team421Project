using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class UI_ValueScript : MonoBehaviour
{
    public GameObject NumberTextPrefab;
    List<TextMeshProUGUI> NumberTexts;

    // Start is called before the first frame update
    void Start()
    {
        TextMeshProUGUI text = Instantiate(NumberTextPrefab).GetComponent<TextMeshProUGUI>();
        NumberTexts = new List<TextMeshProUGUI>();

        text.gameObject.transform.parent = transform;
        SetNumberText(ref text, 0);
        NumberTexts.Add(text);

        //int[] numbers = new int[0];
        //CalcNumbers(ref numbers, 123);

        //img = Instantiate(NumberPrefab).GetComponent<Image>();
        //img.gameObject.transform.parent = transform;
        //Numbers.Add(img);
        //img = Instantiate(NumberPrefab).GetComponent<Image>();
        //img.gameObject.transform.parent = transform;
        //Numbers.Add(img);
        //img.material.SetFloat("_Number", 5);
        //int index = 0;
        //foreach (Image i in Numbers)
        //{
        //    i.material.SetFloat("_Number", numbers[index]);
        //    index++;
        //}

        SetValue(1234);
    }

    public void SetValue(int value)
    {
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
            for (int i = 0; i < tmp; i++)
            {
                TextMeshProUGUI text = Instantiate(NumberTextPrefab).GetComponent<TextMeshProUGUI>();
                text.gameObject.transform.parent = transform;
                NumberTexts.Add(text);
            }

            CalcNumbers(ref numbers, value);
        }
        else if (digit < NumberTexts.Count)
        {
            //åÖêîå∏Ç¡ÇΩ
            int tmp = NumberTexts.Count - digit;
            for (int i = 0; i < tmp; i++)
            {
                Destroy(NumberTexts[i]);
                NumberTexts.RemoveAt(i);
            }

            CalcNumbers(ref numbers, value);
        }
        for (int i = 0; i < NumberTexts.Count; i++)
        {
            NumberTexts[i].text = "<sprite=" + numbers[NumberTexts.Count - i- 1].ToString() + ">";
        }
    }

    void SetNumberText(ref TextMeshProUGUI text, int value)
    {
        text.text = "<sprite=" + value.ToString() + ">";
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
