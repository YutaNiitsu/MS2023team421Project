using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;
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
    public void Generate()
    {
        // �����͂ߍ��ތ^����
        Instantiate(Target, new Vector3(5.0f, 0.0f, 0.0f), Quaternion.identity);

        // ���𐶐�
        Instantiate(Star[0], new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
    }
}
