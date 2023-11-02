using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
// �Q�[���S�̂𐧌䂷��
public class GameManagerScript : MonoBehaviour
{
    ProceduralGenerator proceduralGenerator;

    bool flag = true;
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
        if (flag)
        {
            flag = false;

            ST_Constellation[] constellations = new ST_Constellation[2];
            constellations[0].position = new Vector2(0, 0);
            constellations[1].position = new Vector2(1, 1);

            Line[] lines = new Line[2];
            lines[0].start = new Vector2(0, 0);
            lines[0].end = new Vector2(1, 1);
            lines[1].start = new Vector2(1, 0);
            lines[1].end = new Vector2(2, 2);

            // �e�X�g�f�[�^
            SaveConstellationData[] saveConstellationDatas = new SaveConstellationData[2];

            SaveConstellationData saveConstellationData = new SaveConstellationData();
            saveConstellationData.Create(1, 1, 1, 1, "a", constellations, lines);
            saveConstellationDatas[0] = saveConstellationData;

            saveConstellationData = new SaveConstellationData();
            saveConstellationData.Create(2, 2, 2, 2, "b", constellations, lines);
            saveConstellationDatas[1] = saveConstellationData;

            GetComponent<ConstellationSaveManager>().OnSaveNewData(saveConstellationDatas);

            SaveConstellationData[] data = GetComponent<ConstellationLoadManager>().LoadData();
        }
    }
}
