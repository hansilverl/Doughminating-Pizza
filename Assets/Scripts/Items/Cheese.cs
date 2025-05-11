using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cheese : Ingredient
{
    public override void Interact() {
        base.Interact();
    }

    public override string getInteractionText() {
        return "Pick " + GetIngredientName();
    }
}
