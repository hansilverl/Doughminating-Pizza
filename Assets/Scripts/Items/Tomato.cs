using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tomato : Ingredient
{
    public override void Interact()
    {
        // Check if the ingredient is already chopped or choppable
        Choppable chopped = GetComponent<Choppable>();

        PlayerHand playerHand = GameObject.FindWithTag("Player").GetComponent<PlayerHand>();

        if (playerHand != null && !playerHand.IsHoldingItem)
        {
            // playerHand.PickUp(this);
        }
    }

    public override string getInteractionText()
    {
        return "Press 'E' to interact with " + GetIngredientName();
    }

    
}
