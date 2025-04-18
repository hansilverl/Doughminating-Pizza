using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ingredient : MonoBehaviour, IInteractable, IPickable
{
    [SerializeField] protected string ingredientName;
    public abstract void Interact(); // must be implemented in subclasses

    public abstract string getInteractionText(); // returns the name of the ingredient
    public string GetIngredientName() => ingredientName; // returns the name of the ingredient
}
