using UnityEngine;
[ExecuteInEditMode]
public class SimplePostEffect : MonoBehaviour
{
    [SerializeField] private Material effectMaterial;
    private void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        // �|�X�g�G�t�F�N�g�������Ȃ��ꍇ
        if (effectMaterial == null)
        {
            Graphics.Blit(src, dest);
            return;
        }
        // �|�X�g�G�t�F�N�g��������ꍇ
        Graphics.Blit(src, dest, effectMaterial);
    }
}