using UnityEngine;

public class FlashlightMaskController : MonoBehaviour
{
    public Transform lightPos;
    public Material darknessMaterial;
    public Camera mainCamera;

    void Update()
    {
        if (lightPos != null && darknessMaterial != null)
        {
            Vector3 viewPos = mainCamera.WorldToViewportPoint(lightPos.position);
            darknessMaterial.SetVector("_HolePosition", new Vector4(viewPos.x, viewPos.y, 0, 0));
        }
    }
}
