using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
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
    public GameObject NormalStar;
    public GameObject BouncingStar;
    public GameObject TransfixStar;
    public GameObject IgnoreTeleportationStar;
    public GameObject ExplosionStar;
    [Header("��Q���̃v���n�u")]
    public GameObject Obstacle;
    public GameObject Teleportation;
    public GameObject DarkHole;
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
    public void CreateTargets(SaveConstellationData data)
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
        StageSetting setting = GameManagerScript.instance.StageManager.Setting;
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

            obj.Set(isSpecialPoint, isShield, setting.ShieldHealthPoint);
            Targets[index] = obj;
            index++;
        }

    }
    //�����ʒu������
    private void SetRandomPositions(out Vector2[] positions, Vector2 stageSize, float threshold)
    {
        positions = new Vector2[0];
        //���ɐݒu���ꂽ�����Q��
        List<GameObject> stars = new List<GameObject>();
        GameObject.FindGameObjectsWithTag("Star", stars);
        //���ɐݒu���ꂽ��Q�����Q��
        List<GameObject> obstacles = new List<GameObject>();
        GameObject.FindGameObjectsWithTag("Obstacle", obstacles);
        int index = 0;
        //�X�N���[���̃T�C�Y�����[���h���̃T�C�Y�ɕϊ�
        Vector3 worldScreen = Camera.main.ScreenToWorldPoint(new Vector3(Screen.currentResolution.width, Screen.currentResolution.height, 0.0f));
        
        for (int y = 0; y < (int)stageSize.y; y += 2)
        {
            for (int x = 0; x < (int)stageSize.x; x += 2)
            {
                Vector2 pos = new Vector2(x - stageSize.x / 2, y - stageSize.y / 2);
                bool success = true;
                foreach (GameObject j in stars)
                {
                    Vector2 dis = new Vector2(j.transform.position.x, j.transform.position.y) + pos;
                    float lenSq = Vector2.Dot(dis, dis);
                    //���ɒu����Ă鐯�Ƌ����߂������玸�s
                    if (lenSq < 9.0f)
                        success = false;
                }
                foreach (GameObject j in obstacles)
                {
                    Vector2 dis = new Vector2(j.transform.position.x, j.transform.position.y) + pos;
                    float lenSq = Vector2.Dot(dis, dis);
                    //���ɒu����Ă��Q���Ƌ����߂������玸�s
                    if (lenSq < 9.0f)
                        success = false;
                }

                //���s������Ƃ΂�
                if (!success)
                    continue;

                if (pos.x > -worldScreen.x && pos.x < worldScreen.x
                    && pos.y > -worldScreen.y && pos.y < worldScreen.y)
                    // �J�����X�N���[���ɂ͐������Ȃ�
                    continue;

                float noise = Mathf.PerlinNoise((float)x * 0.7f, (float)y * 0.7f);
                
                if (noise > threshold)
                {
                    //臒l���傫������
                    Array.Resize(ref positions, positions.Length + 1);
                    positions[index] = pos;
                    index++;
                }
            }
        }
    }

    //���Ə�Q���𐶐�
    //range : �����͈�
    //threshold : 臒l
    public void CreateStarObstacle(Vector2 stageSize, float threshold)
    {
        StageSetting setting = GameManagerScript.instance.StageManager.Setting;
        Vector2[] positions;
        //���̐���
        GameObject[] stars = new GameObject[5] { NormalStar, BouncingStar, TransfixStar, IgnoreTeleportationStar, ExplosionStar };
        //�����ʒu������
        SetRandomPositions(out positions, stageSize, threshold);
        //���ɉ�����������Ă��邩�ǂ����̔��ʗp
        bool[] determination = new bool[positions.Length];
        int index = 0;
        //���[�v�̐���
        foreach (Vector2 i in positions)
        {
            float max = setting.ProbabilityObstacle + setting.ProbabilityDarkHole + setting.ProbabilityTeleportation * 0.5f + setting.ProbabilityStar;
            float rand = UnityEngine.Random.Range(0.0f, max);
            if (!determination[index])
            {
                float min = setting.ProbabilityObstacle + setting.ProbabilityDarkHole + setting.ProbabilityStar;
                //���[�v����
                if (setting.ProbabilityTeleportation > 0.0f
                    && rand > min && max >= rand)
                {
                    //���[�v��Q���̐���
                    //2�����ʒu���߂�
                    List<int> indexs = SelectRandomElements(2, 0, positions.Length - 1);
                    //�����u����ĂȂ������琶��
                    if (!determination[indexs[0]] && !determination[indexs[1]])
                    {
                        GameObject obj = Instantiate(Teleportation, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
                        Color color = new Color();
                        color.r = UnityEngine.Random.Range(0f, 1f);
                        color.g = UnityEngine.Random.Range(0f, 1f);
                        color.b = UnityEngine.Random.Range(0f, 1f);
                        color.a = 1.0f;
                        obj.GetComponent<TeleportationScript>().Set(positions[indexs[0]], positions[indexs[1]], color);
                        determination[indexs[0]] = true;
                        determination[indexs[1]] = true;
                    }
                }

            }
            index++;
        }

        index = 0;
        //���Ə�Q���i���[�v�ȊO�j�̐���
        foreach (Vector2 i in positions)
        {
            float max = setting.ProbabilityObstacle + setting.ProbabilityDarkHole + setting.ProbabilityTeleportation + setting.ProbabilityStar;
            float rand = UnityEngine.Random.Range(0.0f, max);
            if (!determination[index])
            {
                float min = 0.0f;
                max = setting.ProbabilityStar;
                //���̐���
                if (setting.ProbabilityStar > 0.0f
                    && max >= rand)
                {
                    CreateStars(new Vector3(i.x, i.y, 0.0f));
                    determination[index] = true;
                }
                min += setting.ProbabilityStar;
                max += setting.ProbabilityObstacle;
                //�����Ȃ���Q���̐���
                if (setting.ProbabilityObstacle > 0.0f
                    && rand > min && max >= rand)
                {
                    Instantiate(Obstacle, new Vector3(i.x, i.y, 0.0f), Quaternion.identity);
                    determination[index] = true;
                }
                min += setting.ProbabilityObstacle;
                max += setting.ProbabilityDarkHole;
                //�_�[�N�z�[������
                if (setting.ProbabilityDarkHole > 0.0f 
                    && rand > min && max >= rand)
                {
                    Instantiate(DarkHole, new Vector3(i.x, i.y, 0.0f), Quaternion.identity);
                    determination[index] = true;
                }
                //min += setting.ProbabilityDarkHole;
                //max += setting.ProbabilityTeleportation;
                ////���[�v����
                //if (setting.ProbabilityTeleportation > 0.0f
                //    && rand > min && max >= rand)
                //{
                //    //���[�v��Q���̐���
                //    //2�����ʒu���߂�
                //    List<int> indexs = SelectRandomElements(2, 0, positions.Length - 1);
                //    //�����u����ĂȂ������琶��
                //    if (!determination[indexs[0]] && !determination[indexs[1]])
                //    {
                //        GameObject obj = Instantiate(Teleportation, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
                //        Color color = new Color();
                //        color.r = UnityEngine.Random.Range(0f, 1f);
                //        color.g = UnityEngine.Random.Range(0f, 1f);
                //        color.b = UnityEngine.Random.Range(0f, 1f);
                //        color.a = 1.0f;
                //        obj.GetComponent<TeleportationScript>().Set(positions[indexs[0]], positions[indexs[1]], color);
                //        determination[indexs[0]] = true;
                //        determination[indexs[1]] = true;
                //    }
                //}
                
            }
            index++;
        }
    }
    //���̐���
    void CreateStars(Vector3 pos)
    {
        StageSetting setting = GameManagerScript.instance.StageManager.Setting;
        float max = setting.ProbabilityNormalStar + setting.ProbabilityBouncingStar + setting.ProbabilityIgnoreTeleportationStar
            + setting.ProbabilityTransfixStar + setting.ProbabilityExplosionStar;
        float rand = UnityEngine.Random.Range(0.0f, max);
        float min = 0.0f;
        max = setting.ProbabilityNormalStar;
        //�m�[�}���̐���
        if (setting.ProbabilityNormalStar > 0.0f
            && max >= rand)
        {
            int type = UnityEngine.Random.Range(0, 4);
            Instantiate(NormalStar, pos, Quaternion.identity);
        }
        min += setting.ProbabilityNormalStar;
        max += setting.ProbabilityBouncingStar;
        //�o�E���X�̐���
        if (setting.ProbabilityBouncingStar > 0.0f
            && rand > min && max >= rand)
        {
            Instantiate(BouncingStar, pos, Quaternion.identity);
        }
        min += setting.ProbabilityBouncingStar;
        max += setting.ProbabilityIgnoreTeleportationStar;
        //���[�v��������
        if (setting.ProbabilityIgnoreTeleportationStar > 0.0f
            && rand > min && max >= rand)
        {
            Instantiate(IgnoreTeleportationStar, pos, Quaternion.identity);
        }
        min += setting.ProbabilityIgnoreTeleportationStar;
        max += setting.ProbabilityTransfixStar;
        //�ђʐ���
        if (setting.ProbabilityTransfixStar > 0.0f
            && rand > min && max >= rand)
        {
            Instantiate(TransfixStar, pos, Quaternion.identity);
        }
        min += setting.ProbabilityTransfixStar;
        max += setting.ProbabilityExplosionStar;
        //��������
        if (setting.ProbabilityExplosionStar > 0.0f
            && rand > min && max >= rand)
        {
            Instantiate(ExplosionStar, pos, Quaternion.identity);
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

    public StarRarity SetStarRarity(Vector3 pos)
    {
        StarRarity result = StarRarity.Unique;
        StageSetting setting = GameManagerScript.instance.StageManager.Setting;
        Vector2 stageSize = setting.StageSize;
        float[] p = new float[4]{
        setting.NormalPoint,
        setting.RarePoint,
        setting.UniquePoint,
        setting.LegendaryPoint
        };
        //���S����̋�����2��
        float lenSq = Vector2.Dot(pos, pos);
        float t = lenSq / ((float)Math.Pow(stageSize.x * 0.5f, 2.0f) * (float)Math.Pow(stageSize.y * 0.5f, 2.0f));

        result = (StarRarity)SelectRandomExp(p,t);

        return result;
    }

    //p : x�l�̔z��
    //t : �ɑ�l�̈ʒu�����炷
    int SelectRandomExp(float[] p, float t)
    {
        int result = 0;
        foreach (float x in p)
        {
            float y = (float)Math.Exp(-Math.Pow((x * 2.0f - t) * 1.0f, 2));
            float rand = UnityEngine.Random.Range(0.1f, 1.0f);
            if (y >= rand)
                break;
            result++;
        }
        if (result == 4)
            result = 0;

        return result;
    }
}
