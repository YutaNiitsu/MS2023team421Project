using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetOrthographicProjection : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Camera mainCamera = GetComponent<Camera>();
        if (mainCamera != null)
        {
            // Orthographic�s��̃p�����[�^�[���w��
            float aspect = (float)Screen.height / (float)Screen.width;
            Vector2 hsize = new Vector2(mainCamera.orthographicSize * 2.0f, mainCamera.orthographicSize * 2.0f * aspect);
            float left = -hsize.x;
            float right = hsize.x;
            float bottom = -hsize.y;
            float top = hsize.y;
            float near = 0.3f;
            float far = 100f;

            // Orthographic�s����쐬
            Matrix4x4 orthoMatrix = Matrix4x4.Ortho(left, right, bottom, top, near, far);

            // �J������projectionMatrix��Orthographic�s����Z�b�g
            mainCamera.projectionMatrix = orthoMatrix;
        }
        else
        {
            Debug.LogError("Main camera not found.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
