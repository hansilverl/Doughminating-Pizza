using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ingredient : MonoBehaviour, IInteractable, IPickable
{
    [SerializeField] protected string ingredientName;
    [SerializeField] protected Vector3 holdPositionOffset;
    public abstract void Interact(); // must be implemented in subclasses

    public abstract string getInteractionText(); // returns the name of the ingredient
    public string GetIngredientName() => ingredientName; // returns the name of the ingredient
    public Vector3 GetHoldPositionOffset() => holdPositionOffset; // returns the hold position offset
}
