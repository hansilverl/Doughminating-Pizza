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
            if (ingredient != null && ingredient is Tomato)
            {
                // blending tomato logic
                playerHand.Remove();

                //Reuse IngredientFactory
                IngredientFactory factory = GetComponent<IngredientFactory>();
                if (factory != null)
                {
                    factory.Interact(); // Triggers the sauce spawn!
                    AudioSource audio = GetComponent<AudioSource>();
                    if (audio != null)
                    {
                        audio.Play(); // Play the blending sound
                    }
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
            if (ingredient != null && ingredient is Tomato)
            {
                return "Press 'E' to blend " + ingredient.GetIngredientName();
            }
        }

        return "";
    }
}
