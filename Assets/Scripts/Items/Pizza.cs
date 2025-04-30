using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pizza : Ingredient
{
    [SerializeField] private CookState CookLevel = CookState.Raw;
    // Time in seconds to cook the pizza

    [SerializeField] private bool hasSauce = false;
    [SerializeField] private bool hasCheese = false;
    [SerializeField] private HashSet<Ingredient> ingredients = new HashSet<Ingredient>();

    public override void Interact() {
        
    }

    public CookState GetCookState() => CookLevel;

    public void SetCookState(CookState state)
{
    this.CookLevel = state;
    Debug.Log($"Pizza is now {state}");
}


    public override string getInteractionText() {
        return "Press 'E' to interact with " + GetIngredientName();
    }

    public void AddIngredient(Ingredient ingredient) {
        if (ingredient is Sauce) {
            this.hasSauce = true;
        } else if (ingredient is Cheese) {
            this.hasCheese = true;
        } else {
            ingredients.Add(ingredient);
        }
    }
}
