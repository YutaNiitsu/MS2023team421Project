using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngineInternal;

public class MovableObstacleManagerScript : MonoBehaviour
{
    [Header("������Q���̃v���n�u")]
    public GameObject MovableObstacle;
    [Header("������Q���̃X�s�[�h")]
    public float Speed;
    [Header("������Q���̐�")]
    public int Number;
    [Header("������Q���̌Q�̂̑傫��")]
    public float Size;
    [Header("�x���\�����鎞��")]
    public float WarningTime;
    [Header("�x���\���_�ŊԊu")]
    public float Interval;

    private GameObject MainCamera;
    private Vector3[] SpawnPos;   //�o���ʒu�̌��
    private WarningMarkScript WarningMark;
    private Coroutine _Coroutine;
    // Start is called before the first frame update
    void Start()
    {
        MainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        SpawnPos = new Vector3[8];
        SpawnPos[0] = new Vector3(1.0f, 0.0f, 0.0f);
        SpawnPos[1] = new Vector3(1.0f, 1.0f, 0.0f);
        SpawnPos[2] = new Vector3(0.0f, 1.0f, 0.0f);
        SpawnPos[3] = new Vector3(-1.0f, 1.0f, 0.0f);
        SpawnPos[4] = new Vector3(-1.0f, 0.0f, 0.0f);
        SpawnPos[5] = new Vector3(-1.0f, -1.0f, 0.0f);
        SpawnPos[6] = new Vector3(0.0f, -1.0f, 0.0f);
        SpawnPos[7] = new Vector3(1.0f, -1.0f, 0.0f);
        WarningMark = GameObject.Find("WarningImage").GetComponent<WarningMarkScript>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //��Q������
    //8��������I��
    public void Create(int dir)
    {
        if (dir < 0 || dir > 7)
            return;

        if (_Coroutine != null) StopCoroutine(_Coroutine);
        _Coroutine = StartCoroutine(CreateCoroutine(dir));
       
    }

    IEnumerator CreateCoroutine(int dir)
    {
        Vector3 screenSize = new Vector3(Screen.currentResolution.width, Screen.currentResolution.height, 0.0f);

        //�����ʒu
        Vector3 popPos = Vector3.Scale(SpawnPos[dir], screenSize) * 0.45f;
       
        //�x���\��
        WarningMark.StartWarning(popPos, Interval);
        yield return new WaitForSeconds(WarningTime);
        WarningMark.StopWarning();

        Vector3 cameraPos = MainCamera.transform.position;
        cameraPos.z = 0.0f;
        ////�X�N���[���̃T�C�Y�����[���h���̃T�C�Y�ɕϊ�
        //Vector3 worldScreen = Camera.main.ScreenToWorldPoint(screenSize);
        //����
        popPos = Vector3.Scale(SpawnPos[dir], screenSize * 0.01f) + cameraPos;
        //�ړ�����
        Vector3 vel = -SpawnPos[dir] * Speed;

        for (int i = 0; i < Number; i++)
        {
            float x = UnityEngine.Random.Range(-1.0f, 1.0f);
            float y = UnityEngine.Random.Range(-1.0f, 1.0f);
            popPos += new Vector3(x, y, 0.0f) * Size;
            GameObject obj = Instantiate(MovableObstacle, popPos, new Quaternion());
            obj.GetComponent<Rigidbody2D>().velocity = vel;
        }
    }
}
