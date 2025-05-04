using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollingPin : Tool
{
    
    public override string getInteractionText()
    {
        return "Pick " + GetToolName();
    }
}
