using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ingredient : MonoBehaviour, IInteractable, IPickable
{
    [SerializeField] protected string ingredientName;
    [SerializeField] protected Vector3 handPositionOffset;
    [SerializeField] protected Vector3 handRotationOffset;
    public abstract void Interact(); // must be implemented in subclasses

    public abstract string getInteractionText(); // returns the name of the ingredient
    public string GetIngredientName() => this.ingredientName; // returns the name of the ingredient
    public Vector3 GetHandPositionOffset() => this.handPositionOffset; // returns the hand position offset
    public Vector3 GetHandRotationOffset() => this.handRotationOffset; // returns the hand rotation offset
}
