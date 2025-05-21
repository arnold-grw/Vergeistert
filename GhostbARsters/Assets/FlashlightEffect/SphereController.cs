using UnityEngine;

public class SphereController : MonoBehaviour
{
    public Transform lightSource;
    public Material darknessMaterial;
    public Camera mainCamera;
    public float current_size = 0.2f;
    public float light_size = 0.2f;

    void Update()
    {
        current_size = Mathf.Lerp(current_size, light_size, Time.deltaTime * 10f);
        darknessMaterial.SetFloat("_Smoothness", current_size);

        if (lightSource != null && darknessMaterial != null && mainCamera != null)
        {
            // Richtung von Kamera zur Lichtquelle
            Vector3 dir = (lightSource.position - mainCamera.transform.position).normalized;

            darknessMaterial.SetVector("_LightDir", dir);
        }
    }

    public void setSize(float size)
    {
        light_size = size;
    }
}
