using UnityEngine;

/// <summary>
/// Base class for simple ingredients that only need basic pick-up functionality
/// </summary>
public abstract class SimpleIngredient : Ingredient
{
    public override string getInteractionText()
    {
        return "Pick " + GetIngredientName();
    }
}
