using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapScript : MonoBehaviour
{
    //public GameObject ConstellationLocationLocationPoint;
    public RectTransform CameraLocationPoint;
    private Transform CameraTransform;
    private Vector2 MapSize;
    private Vector2 StageSize;

    // Start is called before the first frame update
    void Start()
    {
        CameraTransform = GameObject.FindGameObjectWithTag("MainCamera").transform;
        StageSize = GameManagerScript.instance.StageManager.Setting.StageSize;
        MapSize = gameObject.GetComponent<RectTransform>().sizeDelta;
    }

    // Update is called once per frame
    void Update()
    {
        if (StageSize.y > 0.0f)
        {
            Vector3 cameraPos = new Vector3(CameraTransform.position.x * MapSize.x / StageSize.x,
           CameraTransform.position.y * MapSize.y / StageSize.y, 0.0f);
            CameraLocationPoint.localPosition = cameraPos;
        }
       
    }
}
