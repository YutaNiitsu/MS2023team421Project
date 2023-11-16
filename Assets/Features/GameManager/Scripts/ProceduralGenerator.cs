using System;
using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.PlayerSettings;
// �Q�[���J�n���ɐ��Ƃ�����͂ߍ��ޏꏊ��z�u����V�X�e��
public class ProceduralGenerator : MonoBehaviour
{
    [Header("�����͂ߍ��ތ^�̃v���n�u")]
    public GameObject Target;
    [Header("���̃v���n�u")]
    public GameObject[] Star;
    [Header("���A���̃v���n�u")]
    public GameObject[] RareStarArea;
    private List<TargetScript> Targets;
    private GameManagerScript GameManager;
    // Start is called before the first frame update
    void Start()
    {
        GameManager = gameObject.GetComponent<GameManagerScript>();
    }

    // �I�u�W�F�N�g��z�u
    //targets : �͂ߍ��ތ^�̃Z�[�u�f�[�^
    //range : ���̐����͈�
    //threshold : ���̖��x���ς��
    public void GenerateTargets(ST_Constellation[] targets)
    {
        if (Targets != null && Targets.Count > 0)
        {
            foreach (TargetScript i in Targets)
            {
                Destroy(i.gameObject);
            }
        }

        Targets = new List<TargetScript>();
        // �����͂ߍ��ތ^����
        foreach (ST_Constellation i in targets)
        {
            TargetScript obj = Instantiate(Target, i.position, Quaternion.identity).GetComponent<TargetScript>();
            obj.Set(GameManager, false);
            Targets.Add(obj);
        }

    }

    //���𐶐�
    //range : �����͈�
    //threshold : 臒l
    public void GenerateStars(Vector2 range, float threshold)
    {
        //�X�N���[���̃T�C�Y�����[���h���̃T�C�Y�ɕϊ�
        Vector3 worldScreen = Camera.main.ScreenToWorldPoint(new Vector3(Screen.currentResolution.width, Screen.currentResolution.height, 0.0f));
        // ���𐶐�
        for (int y = 0; y < (int)range.y; y += 2)
        {
            for (int x = 0; x < (int)range.x; x += 2)
            {
                Vector2 pos = new Vector2(x - range.x / 2, y - range.y / 2);
                if (pos.x > -worldScreen.x && pos.x < worldScreen.x
                    && pos.y > -worldScreen.y && pos.y < worldScreen.y)
                    // �J�����X�N���[���ɂ͐������Ȃ�
                    continue;

                float noise = Mathf.PerlinNoise((float)x * 0.7f, (float)y * 0.7f);
                //臒l���傫�������琶��
                if (noise > threshold)
                {
                    float len = Vector2.Dot(pos, pos);
                    float rand = UnityEngine.Random.Range(0.0f, 1.0f);
                    if (len > Math.Pow(300, 2) && rand > 0.5)
                    {
                        Instantiate(RareStarArea[0], new Vector3(pos.x, pos.y, 0.0f), Quaternion.identity);
                    }
                    else
                    {
                        Instantiate(Star[0], new Vector3(pos.x, pos.y, 0.0f), Quaternion.identity);
                    }
                    
                }
            }
        }


    }

    public List<TargetScript> GetTargets()
    {
        return Targets;
    }
}
