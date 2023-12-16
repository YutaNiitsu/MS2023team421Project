using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class TutorialManagerScript : StageManagerScript
{
    public TutorialUIScript TutorialUI;
   
    public Action ClickStarFunc { get; set; }
    public Action DischargeFunc { get; set; }
    public Action PutOnTaregetFunc { get; set; }

    public bool EnableAddScore { get; set; }
    public override void StageManagerStart()
    {
        Initialize();
        EnableAddScore = false;
        UIManager.SetUIActive(false, false, false, false, false, false);
        TutorialUI.SetAllDisable();
        TutorialUI.Buttons.SetActive(false);
        Instantiate(TutorialUI.OperatingTutorialPrefab);
    }

    public override void ClickStar()
    {
        ClickStarFunc();
    }

    public override void Discharge(Rigidbody2D rb)
    {
        DischargeFunc();
        if (EnableAddScore)
            base.Discharge(rb);
    }
    public override void PutOnTareget()
    {
        PutOnTaregetFunc();
    }

    public override void AddScore(StarRarity starRarity, bool isSpecialPoint)
    {
        if (EnableAddScore)
            base.AddScore(starRarity, isSpecialPoint);
    }

    public override void StageComplete()
    {
        TutorialUI.SetAllDisable();
        base.StageComplete();
    }
}
