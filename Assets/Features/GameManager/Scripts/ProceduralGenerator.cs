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
    public List<TargetScript> Targets { get; protected set; }

    // Start is called before the first frame update
    void Start()
    {
        
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
            obj.Set(false);
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
                    float lenSq = Vector2.Dot(pos, pos);
                    float len = Vector2.Distance(pos, new Vector2(0.0f, 0.0f));
                    float rand = UnityEngine.Random.Range(0.0f, 1.0f);
                    if (lenSq > Math.Pow(200, 2) && rand > (400.0f - (len - 200.0f) * 0.3f) / 400.0f)
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

    //�S�Ă̂͂ߍ��ތ^�ɐ����͂܂��Ă��邩
    public bool IsAllGoaled()
    {
        bool success = true;
        foreach (TargetScript i in Targets)
        {
            if (!i.Goaled)
            {
                //�����͂܂��Ă��Ȃ����̂��������玸�s
                success = false;
                break;
            }
        }

        return success;
    }

    //���ʃ|�C���g�Ɏw�肳��Ă���͂ߍ��ތ^�S�ĂɃ��j�[�N�ȏ�̃��A���e�B�̐����͂܂��Ă��邩�ǂ���
    public bool IsRareStarGoaledOnSpecialTargetAll()
    {
        bool success = true;
        foreach (TargetScript i in Targets)
        {
            //���ʃ|�C���g�Ɏw�肳��Ă���
            if (i.IsSpecialPoint)
            {
                //���j�[�N�ȏ�̃��A���e�B�̐����͂܂��Ă��Ȃ�����
                if (i.StarGoaled != null && (int)i.StarGoaled.Rarity < 2)
                {
                    success = false;
                }
            }
        }

        return success;
    }
}
