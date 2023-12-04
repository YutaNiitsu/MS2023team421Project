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
    [Header("���A�������G���A�̃v���n�u")]
    public GameObject[] RareStarArea;
    [Header("�����Ȃ���Q���̃v���n�u")]
    public GameObject NormalObstacle;
    [Header("�_�[�N�z�[���̃v���n�u")]
    public GameObject DarkHoleObstacle;
    [Header("���[�v��Q���̃v���n�u")]
    public GameObject TeleportationObstacle;
    public TargetScript[] Targets { get; protected set; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    //�͂ߍ��ތ^��z�u
    //targets : �͂ߍ��ތ^�̃Z�[�u�f�[�^
    //range : ���̐����͈�
    //threshold : ���̖��x���ς��
    public void GenerateTargets(SaveConstellationData data)
    {
        if (Targets != null && Targets.Length > 0)
        {
            foreach (TargetScript i in Targets)
            {
                Destroy(i.gameObject);
            }
        }

        Targets = new TargetScript[data.targets.Length];
        // �����͂ߍ��ތ^����
        StageSetting setting = GameManagerScript.instance.Setting;
        //���ʃ|�C���g�A�V�[���h�ɂ���^�̗v�f�ԍ����X�g
        List<int> specialPoints = SelectRandomElements(setting.SpecialPointNumber, 0, Targets.Length - 1);
        List<int> shields = SelectRandomElements(setting.ShieldNumber, 0, Targets.Length - 1);


        int index = 0;
        foreach (ST_Constellation i in data.targets)
        {
            TargetScript obj = Instantiate(Target, i.position, Quaternion.identity).GetComponent<TargetScript>();

            //���ʃ|�C���g�A�V�[���h�ɂ���^�̗v�f�ԍ����X�g����index�Ɉ�v����v�f�ԍ�������������ݒ肷��
            bool isSpecialPoint = false;
            bool isShield = false;
            if (specialPoints.Contains(index))
                isSpecialPoint = true;
       
            if (shields.Contains(index))
                isShield = true;

            obj.Set(isSpecialPoint, isShield);
            Targets[index] = obj;
            index++;
        }

    }
    //�����ʒu������
    //private void SetRandomPositions(out Vector2[] positions)
    //{

    //}

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
        bool success = false;
        foreach (TargetScript i in Targets)
        {
            if (!i.Goaled)
            {
                //�����͂܂��Ă��Ȃ�������Ƃ΂�
                continue;
            }

            if (i.IsRareStarGoaledOnSpecialTarget())
            {
                success = true;
            }
            else
            {
                //�͂܂��Ă��鐯�����j�[�N�ȏ�̃��A���e�B�ł͂Ȃ������玸�s
                success = false;
                break;
            }
        }

        return success;
    }

    //�����_���ȗv�f���w�肳�ꂽ�������I��
    public List<int> SelectRandomElements(int count, int min, int max)
    {
        List<int> result = new List<int>();
        List<int> data = new List<int>();
        //min����max�܂ł̐����f�[�^�쐬
        for (int i = min; i < max + 1; i++)
        {
            data.Add(i);
        }

        for (int i = 0; i < count; i++)
        {
            if (data.Count == 0)
                break;

            int rand = UnityEngine.Random.Range(0, data.Count - 1);
            result.Add(data[rand]);
            data.RemoveAt(rand);
        }
        return result;
    }
}
