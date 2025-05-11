using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tomato : Ingredient
{

    public override string getInteractionText()
    {
        return "Pick " + GetIngredientName();
    }

    
}
