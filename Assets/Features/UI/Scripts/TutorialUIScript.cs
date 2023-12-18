using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialUIScript : MonoBehaviour
{
    public GameObject[] OperatingTutorial;
    public GameObject[] StarTypeTutorial;
    public GameObject[] ObstacleTypeTutorial;
    public GameObject Buttons;
    public GameObject OperatingTutorialPrefab;
    public GameObject StarTypeTutorialPrefab;
    public GameObject ObstacleTypeTutorialPrefab;
    // Start is called before the first frame update
    void Start()
    {

    }

    public void SetAllDisable()
    {
        foreach (GameObject i in OperatingTutorial)
        {
            i.SetActive(false);
        }
        foreach (GameObject i in StarTypeTutorial)
        {
            i.SetActive(false);
        }
        foreach (GameObject i in ObstacleTypeTutorial)
        {
            i.SetActive(false);
        }
        Buttons.SetActive(false);
    }

    public void SetActiveOperatingTutorial(int index)
    {
        foreach (GameObject i in OperatingTutorial)
        {
            i.SetActive(false);
        }
        OperatingTutorial[index].SetActive(true);
    }
    public void SetActiveStarTypeTutorial(int index)
    {
        foreach (GameObject i in StarTypeTutorial)
        {
            i.SetActive(false);
        }
        StarTypeTutorial[index].SetActive(true);
    }
    public void SetActiveObstacleTutorial(int index)
    {
        foreach (GameObject i in ObstacleTypeTutorial)
        {
            i.SetActive(false);
        }
        ObstacleTypeTutorial[index].SetActive(true);
    }

    //ƒ{ƒ^ƒ“
    public void SelectTutorialMode(int mode)
    {
        GameObject[] objs = new GameObject[3] { OperatingTutorialPrefab, StarTypeTutorialPrefab, ObstacleTypeTutorialPrefab };
        Instantiate(objs[mode]);
    }
}
