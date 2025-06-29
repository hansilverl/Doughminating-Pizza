using UnityEngine;
using Unity.VectorGraphics;
using System.Collections.Generic;

/// <summary>
/// Helper class to manage UI ingredient configurations
/// </summary>
public static class UIIngredientManager
{
    /// <summary>
    /// Updates the visibility of an ingredient UI element
    /// </summary>
    /// <param name="uiElement">The UI element to update</param>
    /// <param name="isVisible">Whether the element should be visible</param>
    public static void SetIngredientVisibility(SVGImage uiElement, bool isVisible)
    {
        if (uiElement == null) return;
        
        Color currentColor = uiElement.color;
        uiElement.color = new Color(currentColor.r, currentColor.g, currentColor.b, isVisible ? 1f : 0f);
    }

    /// <summary>
    /// Initializes an ingredient UI element to be transparent
    /// </summary>
    /// <param name="uiElement">The UI element to initialize</param>
    public static void InitializeIngredientUI(SVGImage uiElement)
    {
        SetIngredientVisibility(uiElement, false);
    }

    /// <summary>
    /// Gets the cook level color for the pizza
    /// </summary>
    /// <param name="cookState">The cooking state</param>
    /// <returns>The color representing the cook state</returns>
    public static Color32 GetCookLevelColor(CookState cookState)
    {
        return cookState switch
        {
            CookState.Raw => new Color32(0xF5, 0xE1, 0xA4, 0xFF),    // Peach color
            CookState.Cooked => new Color32(0xBF, 0x90, 0x01, 0xFF), // Golden brown
            CookState.Burnt => new Color32(0x4D, 0x3D, 0x0c, 0xFF),  // Dark brown
            _ => Color.white
        };
    }
}
