using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IPickable : MonoBehaviour
{
    [SerializeField] protected Vector3 handPositionOffset;
    [SerializeField] protected Vector3 handRotationOffset;
    [SerializeField] protected Vector3 defaultRotation;
    [SerializeField] protected float counterPositionOffset;
    public abstract Vector3 GetHandPositionOffset();
    public abstract Vector3 GetHandRotationOffset();
    public abstract Vector3 GetDefaultRotation(); // returns the default rotation of the ingredient
    public abstract float GetCounterPositionOffset(); // returns the counter position offset
}
