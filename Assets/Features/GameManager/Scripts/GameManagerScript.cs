using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// �Q�[���S�̂𐧌䂷��
public class GameManagerScript : MonoBehaviour
{
    ProceduralGenerator proceduralGenerator;
    // Start is called before the first frame update
    void Start()
    {
        proceduralGenerator = GetComponent<ProceduralGenerator>();
        // �I�u�W�F�N�g��z�u
        proceduralGenerator.Generate();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
