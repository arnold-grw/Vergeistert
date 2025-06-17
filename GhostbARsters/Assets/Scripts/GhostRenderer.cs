using UnityEngine;

public class GhostRenderer : MonoBehaviour
{
    private Material[] dissolveMaterials;

    void Awake()
    {
        SkinnedMeshRenderer renderer = GetComponentInChildren<SkinnedMeshRenderer>();
        if (renderer != null)
        {
            Material[] originalMats = renderer.materials;
            dissolveMaterials = new Material[originalMats.Length];

            for (int i = 0; i < originalMats.Length; i++)
            {
                dissolveMaterials[i] = new Material(originalMats[i]);
            }

            renderer.materials = dissolveMaterials;
        }

        SetDissolveAmount(1.0f);
    }

    public void SetDissolveAmount(float dissolve)
    {
        if (dissolveMaterials == null) return;

        foreach (Material mat in dissolveMaterials)
        {
            if (mat != null)
            {
                mat.SetFloat("_DissolveAmount", dissolve);
            }
        }
    }
}
