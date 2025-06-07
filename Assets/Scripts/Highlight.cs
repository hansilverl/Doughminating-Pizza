using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Highlight : MonoBehaviour
{
    public Renderer[] renderers;
    private Color[] originalColors;
    // Set highlightColor with 80% transparency (alpha = 0.2)
    public Color highlightColor = Color.green;

    void Awake()
    {
        // Recursively get all renderers in this object and children
        renderers = GetComponentsInChildren<Renderer>(includeInactive: true);

        originalColors = new Color[renderers.Length];
        for (int i = 0; i < renderers.Length; i++)
        {
            if (renderers[i].material.HasProperty("_Color"))
                originalColors[i] = renderers[i].material.color;
        }
    }

    public void SetHighlight(bool state)
    {
        for (int i = 0; i < renderers.Length; i++)
        {
            if (renderers[i].material.HasProperty("_Color"))
            {
                renderers[i].material.color = state ? highlightColor : originalColors[i];
            }
        }
    }
    void Start()
    {

    }

    void Update()
    {

    }
}
