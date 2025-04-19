using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientFactory : MonoBehaviour, IInteractable
{
    private Ingredient ingredient;
    public void Interact() {
        Debug.Log("Ingredient: " + ingredient.GetIngredientName() + " is being interacted with");
    }

    public  string getInteractionText() {
        return "Press 'E' to grab a " + ingredient.GetIngredientName();
    }

    void Awake() {
        this.ingredient = GetComponent<Ingredient>();
        if (ingredient == null) {
            Debug.LogError("IngredientFactory: No ingredient found on this object");
        }
    }
}
