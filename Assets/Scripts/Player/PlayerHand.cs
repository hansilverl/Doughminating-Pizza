using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHand : MonoBehaviour
{
    [SerializeField] private Transform holdPoint;
    private GameObject heldItem;
    public bool IsHoldingItem => heldItem != null;

    public void PickUp(IPickable item)
    {
        Debug.Log("Picked up item: " + item);
        if (heldItem != null)
        {
            Debug.Log("Already holding an item. Cannot pick up another.");
            return;
        }
        if (item is GameObject)
        {
            Debug.Log("Item is a valid pickable object.");
            // heldItem = item as GameObject;
            // heldItem.transform.SetParent(holdPoint);
            // heldItem.transform.localPosition = Vector3.zero;
            // heldItem.transform.localRotation = Quaternion.identity;
            Debug.Log("Held item: " + heldItem.name);
        }
        else
        {
            Debug.Log("Item is not a valid pickable object.");
        }

    }



}
