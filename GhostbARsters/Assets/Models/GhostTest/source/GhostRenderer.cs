using UnityEngine;
using System.Collections;

public class GhostRenderer : MonoBehaviour
{
    Material[] dissolveMaterials;

    public void Dissolve(float duration)
    {
        SkinnedMeshRenderer renderer = GetComponentInChildren<SkinnedMeshRenderer>();
        if (renderer != null)
        {
            // Klone alle Materialien individuell
            Material[] originalMats = renderer.materials;
            dissolveMaterials = new Material[originalMats.Length];

            for (int i = 0; i < originalMats.Length; i++)
            {
                dissolveMaterials[i] = new Material(originalMats[i]);
            }

            // Setze die neuen Materials im Renderer
            renderer.materials = dissolveMaterials;
        }

        StartCoroutine(DissolveAndDestroy(duration));
    }

    IEnumerator DissolveAndDestroy(float duration)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float dissolve = Mathf.Lerp(0f, 1f, elapsed / duration);

            foreach (Material mat in dissolveMaterials)
            {
                if (mat != null)
                    mat.SetFloat("_DissolveAmount", dissolve);
            }

            elapsed += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }
}
