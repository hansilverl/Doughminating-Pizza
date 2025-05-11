using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sauce : Ingredient
{
    // public override void Interact() {

    // }

    public override string getInteractionText() {
        return "Pick " + GetIngredientName();
    }
}
