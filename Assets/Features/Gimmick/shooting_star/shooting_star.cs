using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shooting_star : MonoBehaviour
{

    [SerializeField]
    GameObject starObj;

    float count = 0.0f;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        count += Time.deltaTime;
       if (count>2.0f)
        {
            star();
            count = 0.0f;
        }


    }



    private void star()
    {
        //�e�̑���
       float speed = 600;

        // �e�𔭎˂���ꏊ���擾
        Vector3 shootPos = this.transform.position;
        // ��Ŏ擾�����ꏊ�ɁA"bullet"��Prefab���o��������
        GameObject newStar = Instantiate(starObj, shootPos, transform.rotation);
        // �o���������{�[����forward(z������)
        Vector3 direction = newStar.transform.right;
        // �e�̔��˕�����newBall��z����(���[�J�����W)�����A�e�I�u�W�F�N�g��rigidbody�ɏՌ��͂�������
       
            newStar.GetComponent<Rigidbody2D>().AddForce(direction * speed, ForceMode2D.Force);
        
       
        // �o���������{�[���̖��O��"bullet"�ɕύX
        newStar.name = starObj.name;
        // �o���������{�[����0.8�b��ɏ���
        Destroy(newStar, 5f);


        //   newBall;
    }



}
