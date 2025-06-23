using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pineapple : Ingredient
{
    public override string getInteractionText() {
        return "Pick " + GetIngredientName();
    }
}
