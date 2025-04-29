using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tomato : Ingredient
{

    public override string getInteractionText()
    {
        return "Press 'E' to interact with " + GetIngredientName();
    }

    
}
