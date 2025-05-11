using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pineapple : Ingredient
{
    public override string getInteractionText() {
        return "Pick " + GetIngredientName();
    }

    // Awake happens immediately when the object is instantiated
    void Awake() {
        ingredientName = "Pineapple";
    }
}
