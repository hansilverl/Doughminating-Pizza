using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : Tool
{   public override void Interact()
    {
        // Implement interaction logic here
        Debug.Log("Interacting with " + toolName);
    }

    public override string getInteractionText()
    {
        return "Use " + toolName; // Returns the interaction text
    }
}
