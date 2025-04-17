using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pineapple : Ingredient
{
    public override void Interact() {
        Debug.Log("Interacted with " + GetIngredientName());
    }

    public override string getInteractionText() {
        return "Press 'E' to interact with " + GetIngredientName();
    }

    // Awake happens immediately when the object is instantiated
    void Awake() {
        ingredientName = "Pineapple";
    }
}
