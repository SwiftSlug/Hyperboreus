using UnityEngine;

public class CameraBlur : MonoBehaviour
{
    public Material BlurMaterial;

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, BlurMaterial);
    }
}
