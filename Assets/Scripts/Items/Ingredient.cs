using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ingredient : MonoBehaviour, IInteractable
{
    [SerializeField] protected string ingredientName;
    public abstract void Interact(); // must be implemented in subclasses

    public abstract string getInteractionText(); // returns the name of the ingredient
}
