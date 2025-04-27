using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollingPin : Tool
{
    
    public override string getInteractionText()
    {
        return "Press 'E' to pick up " + GetToolName();
    }
}
