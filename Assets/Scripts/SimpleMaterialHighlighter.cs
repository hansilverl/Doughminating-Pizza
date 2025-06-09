using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;

public class SimpleMaterialHighlighter : MonoBehaviour
{
    [Tooltip("Material used when object is highlighted")]
    public Material highlightMaterial;

    private Renderer[] renderers;
    private Material[][] originalMaterials;
    private bool isHighlighted = false;

    void Awake()
    {
        renderers = GetComponentsInChildren<Renderer>(true);
        originalMaterials = new Material[renderers.Length][];

        for (int i = 0; i < renderers.Length; i++)
        {
            originalMaterials[i] = renderers[i].materials;
        }
    }

    public void SetHighlight(bool state)
    {
        if (state == isHighlighted) return; // Skip if already in desired state
        isHighlighted = state;

        for (int i = 0; i < renderers.Length; i++)
        {
            if (state)
            {
                Material[] newMats = new Material[renderers[i].materials.Length];
                for (int j = 0; j < newMats.Length; j++)
                    newMats[j] = highlightMaterial;
                renderers[i].materials = newMats;
            }
            else
            {
                renderers[i].materials = originalMaterials[i];
            }
        }
    }
}
