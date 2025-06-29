using UnityEngine;

[System.Serializable]
public struct IngredientVisualConfig
{
    public GameObject prefab;
    public Vector3 relativeOffset;
    public Quaternion worldRotation;
    public Vector3 localScale;
    public float heightAdjustment;
    
    public IngredientVisualConfig(GameObject prefab, Vector3 relativeOffset, Quaternion worldRotation, 
                                  Vector3 localScale, float heightAdjustment)
    {
        this.prefab = prefab;
        this.relativeOffset = relativeOffset;
        this.worldRotation = worldRotation;
        this.localScale = localScale;
        this.heightAdjustment = heightAdjustment;
    }
}
