using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ingredient : MonoBehaviour, IInteractable, IPickable
{
    [SerializeField] protected string ingredientName;
    [SerializeField] protected Vector3 handPositionOffset;
    [SerializeField] protected Vector3 handRotationOffset;
    [SerializeField] protected Vector3 defaultRotation;
    [SerializeField] protected float counterPositionOffset;
    
    public virtual void Interact() {
        PlayerHand playerHand = GameObject.FindWithTag("Player").GetComponent<PlayerHand>();
        if (playerHand != null && !playerHand.IsHoldingItem)
        {
            transform.SetParent(null);
            playerHand.PickUp(this.gameObject);
            Debug.Log("Picked up " + ingredientName);
        }
    }

    public abstract string getInteractionText(); // returns the name of the ingredient
    public string GetIngredientName() => this.ingredientName; // returns the name of the ingredient
    public Vector3 GetHandPositionOffset() => this.handPositionOffset; // returns the hand position offset
    public Vector3 GetHandRotationOffset() => this.handRotationOffset; // returns the hand rotation offset
    public Vector3 GetDefaultRotation() => this.defaultRotation; // returns the default rotation of the ingredient
    public float GetCounterPositionOffset() => this.counterPositionOffset; // returns the counter position offset
}
