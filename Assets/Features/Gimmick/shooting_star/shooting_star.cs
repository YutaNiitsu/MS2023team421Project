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
        //弾の速さ
       float speed = 600;

        // 弾を発射する場所を取得
        Vector3 shootPos = this.transform.position;
        // 上で取得した場所に、"bullet"のPrefabを出現させる
        GameObject newStar = Instantiate(starObj, shootPos, transform.rotation);
        // 出現させたボールのforward(z軸方向)
        Vector3 direction = newStar.transform.right;
        // 弾の発射方向にnewBallのz方向(ローカル座標)を入れ、弾オブジェクトのrigidbodyに衝撃力を加える
       
            newStar.GetComponent<Rigidbody2D>().AddForce(direction * speed, ForceMode2D.Force);
        
       
        // 出現させたボールの名前を"bullet"に変更
        newStar.name = starObj.name;
        // 出現させたボールを0.8秒後に消す
        Destroy(newStar, 5f);


        //   newBall;
    }



}
