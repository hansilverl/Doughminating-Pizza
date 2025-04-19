using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trash : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        PlayerHand playerHand = GameObject.FindWithTag("Player").GetComponent<PlayerHand>();
        if (playerHand != null && playerHand.IsHoldingItem)
        {
            GameObject held = playerHand.HeldItem;
            Ingredient ingredient = held.GetComponent<Ingredient>();
            if (ingredient != null)
            {
                // Perform trashing logic here
                Debug.Log("Trashing " + ingredient.GetIngredientName());
                playerHand.Remove(); // Remove the item from the player's hand
            }
        }
    }

    public string getInteractionText()
    {
        return "Press 'E' to trash item";
    }
}
