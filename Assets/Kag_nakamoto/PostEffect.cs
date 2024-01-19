using UnityEngine;
[ExecuteInEditMode]
public class SimplePostEffect : MonoBehaviour
{
    [SerializeField] private Material effectMaterial;
    private void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        // ポストエフェクトをかけない場合
        if (effectMaterial == null)
        {
            Graphics.Blit(src, dest);
            return;
        }
        // ポストエフェクトをかける場合
        Graphics.Blit(src, dest, effectMaterial);
    }
}