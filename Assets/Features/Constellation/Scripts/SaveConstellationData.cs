using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[System.Serializable]
public class SaveConstellationData
{
    // §ì‚µ‚½”NŒ“ú
    public int year;
    public int month;
    public int day;
    // –¼‘O
    public string name;
    // ¯‚ğ‚Í‚ß‚ŞŒ^‚ÌˆÊ’u
    public Vector2[] tagerPos;
}