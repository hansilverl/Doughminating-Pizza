using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ingredient : IPickable, IInteractable
{
    [SerializeField] protected string ingredientName;
    
    public virtual void Interact() {
        PlayerHand playerHand = GameObject.FindWithTag("Player").GetComponent<PlayerHand>();
        if (playerHand != null && !playerHand.IsHoldingItem)
        {
            transform.SetParent(null);
            playerHand.PickUp(this.gameObject);
        }
    }

    public abstract string getInteractionText(); // returns the name of the ingredient
    public string GetIngredientName() => this.ingredientName; // returns the name of the ingredient
    public override Vector3 GetHandPositionOffset() => this.handPositionOffset; // returns the hand position offset
    public override Vector3 GetHandRotationOffset() => this.handRotationOffset; // returns the hand rotation offset
    public override Vector3 GetDefaultRotation() => this.defaultRotation; // returns the default rotation of the ingredient
    public override float GetCounterPositionOffset() => this.counterPositionOffset; // returns the counter position offset
}
