using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dough : Ingredient
{
    public override void Interact()
    {
        PlayerHand playerHand = GameObject.FindWithTag("Player").GetComponent<PlayerHand>();
        if(playerHand != null && playerHand.IsHoldingItem)
        {
            GameObject held = playerHand.HeldItem;
            Tool item = held.GetComponent<Tool>();
            if (item != null && item is RollingPin)
            {
                Debug.Log("Rolling out dough");
            }
        }
        else {
            base.Interact();
        }
    }

    public override string getInteractionText()
    {
        PlayerHand playerHand = GameObject.FindWithTag("Player").GetComponent<PlayerHand>();
        if(playerHand != null && playerHand.IsHoldingItem)
        {
            GameObject held = playerHand.HeldItem;
            Tool item = held.GetComponent<Tool>();
            if (item != null && item is RollingPin)
            {
                return "Roll out " + this.GetIngredientName();
            }
        }
        return "Pick " + this.GetIngredientName();
    }
}
