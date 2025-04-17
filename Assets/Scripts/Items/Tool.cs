using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tool : MonoBehaviour, IInteractable
{
    [SerializeField] private string toolName;

    public abstract void Interact(); // must be implemented in subclasses
    public abstract string getInteractionText(); // returns the name of the ingredient
    public string GetToolName() => toolName; // returns the name of the tool
    
}
