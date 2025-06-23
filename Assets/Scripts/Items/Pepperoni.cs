using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pepperoni : Ingredient
{
    public override string getInteractionText() {
        return "Pick " + GetIngredientName();
    }
} 