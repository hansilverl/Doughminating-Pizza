using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sauce : Ingredient, IPickable
{
    // public override void Interact() {

    // }

    public override string getInteractionText() {
        return "Press 'E' to interact with " + GetIngredientName();
    }
}
