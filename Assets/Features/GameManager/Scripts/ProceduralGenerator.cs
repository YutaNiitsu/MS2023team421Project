using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;
using static UnityEditor.PlayerSettings;
// �Q�[���J�n���ɐ��Ƃ�����͂ߍ��ޏꏊ��z�u����V�X�e��
public class ProceduralGenerator : MonoBehaviour
{
    public GameObject Target;
    public GameObject[] Star;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // �I�u�W�F�N�g��z�u
    //targets : �͂ߍ��ތ^�̃Z�[�u�f�[�^
    //range : ���̐����͈�
    //threshold : ���̖��x���ς��
    public void Generate(ST_Constellation[] targets, Vector2 range, float threshold)
    {
        // �����͂ߍ��ތ^����
        foreach (ST_Constellation i in targets)
        {
            Instantiate(Target, i.position, Quaternion.identity);
        }
        //�X�N���[���̃T�C�Y�����[���h���̃T�C�Y�ɕϊ�
        Vector3 worldScreen = Camera.main.ScreenToWorldPoint(new Vector3(Screen.currentResolution.width, Screen.currentResolution.height, 0.0f));
        // ���𐶐�
        for (int y = 0; y < (int)range.y; y+=2)
        {
            for (int x = 0; x < (int)range.x; x+=2)
            {
                Vector2 pos = new Vector2(x - range.x / 2, y - range.y / 2);
                if (pos.x > -worldScreen.x && pos.x < worldScreen.x
                    && pos.y > -worldScreen.y && pos.y < worldScreen.y)
                    // �X�N���[���ɂ͐������Ȃ�
                    continue;
                float noise = Mathf.PerlinNoise((float)x * 0.7f, (float)y * 0.7f);
                //臒l���傫�������琶��
                if (noise > threshold) Instantiate(Star[0], new Vector3(pos.x, pos.y, 0.0f), Quaternion.identity);
            }
        }
       
        
    }
}
