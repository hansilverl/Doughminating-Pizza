using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tomato : Ingredient
{   
    public override void Interact()
    {
        // Check if the ingredient is already chopped or choppable
        Choppable chopped = GetComponent<Choppable>();
        Debug.Log("Interacted with " + ingredientName);
    }

    public override string getInteractionText()
    {
        return "Press 'E' to interact with " + ingredientName;
    }

    // Awake happens immediately when the object is instantiated
    void Awake()
    {
        ingredientName = "Tomato";
    }
}
