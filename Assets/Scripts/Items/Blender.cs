using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blender : Tool
{
    public override void Interact()
    {
        PlayerHand playerHand = GameObject.FindWithTag("Player").GetComponent<PlayerHand>();
        if (playerHand != null && playerHand.IsHoldingItem)
        {
            GameObject held = playerHand.HeldItem;
            Ingredient ingredient = held.GetComponent<Ingredient>();
            if (ingredient != null)
            {
                // Perform blending logic here
                Debug.Log("Blending " + ingredient.GetIngredientName() + " with " + GetToolName());
                // You can add more logic to handle the blending process
                playerHand.Remove();

                //Reuse IngredientFactory
                IngredientFactory factory = GetComponent<IngredientFactory>();
                if (factory != null)
                {
                    factory.Interact(); // Triggers the sauce spawn!
                }

            }
        }
    }

    public override string getInteractionText()
    {
        PlayerHand playerHand = GameObject.FindWithTag("Player").GetComponent<PlayerHand>();
        if (playerHand != null && playerHand.IsHoldingItem)
        {
            GameObject held = playerHand.HeldItem;
            Ingredient ingredient = held.GetComponent<Ingredient>();
            if (ingredient != null)
            {
                return "Press 'E' to blend " + ingredient.GetIngredientName();
            }
        }

        return "Press 'E' to blend with " + GetToolName();
    }
}
