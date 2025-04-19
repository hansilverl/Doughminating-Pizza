using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blender : Tool
{
    public override void Interact() {
        Debug.Log("Interacted with " + GetToolName());
    }

    public override string getInteractionText() {
        PlayerHand playerHand = GameObject.FindWithTag("Player").GetComponent<PlayerHand>();
        if (playerHand != null && playerHand.IsHoldingItem)
        {
            GameObject held = playerHand.HeldItem;
            Ingredient ingredient = held.GetComponent<Ingredient>();
            if (ingredient != null)
            {
                return "Press 'E' to blend " + ingredient.GetIngredientName() + " with " + GetToolName();
            }
        }

        return "Press 'E' to blend with " + GetToolName();
    }
}
