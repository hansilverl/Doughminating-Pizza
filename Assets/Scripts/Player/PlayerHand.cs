using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHand : MonoBehaviour
{
    [SerializeField] private Transform holdPoint;
    private GameObject heldItem;
    public bool IsHoldingItem => heldItem != null;
    public GameObject HeldItem => heldItem;
    public void PickUp(GameObject item)
    {
        Debug.Log("Picked up item: " + item);
        if (item.CompareTag("Tool") || item.CompareTag("Ingredient"))
        {
            if (heldItem != null)
            {
            Debug.Log("Already holding an item. Cannot pick up another.");
            return;
            }

            heldItem = item;
            heldItem.transform.SetParent(holdPoint);
            heldItem.transform.localPosition = item.GetComponent<Ingredient>().GetHandPositionOffset();
            heldItem.transform.localRotation = Quaternion.Euler(item.GetComponent<Ingredient>().GetHandRotationOffset());
            Debug.Log("Held item: " + heldItem.name);
        }
        else Debug.Log("Item is not pickable");
        // if (heldItem != null)
        // {
        //     Debug.Log("Already holding an item. Cannot pick up another.");
        //     return;
        // }
        // if (item is GameObject)
        // {
        //     Debug.Log("Item is a valid pickable object.");
        //     // heldItem = item as GameObject;
        //     // heldItem.transform.SetParent(holdPoint);
        //     // heldItem.transform.localPosition = Vector3.zero;
        //     // heldItem.transform.localRotation = Quaternion.identity;
        //     Debug.Log("Held item: " + heldItem.name);
        // }
        // else
        // {
        //     Debug.Log("Item is not a valid pickable object.");
        // }

    }
    public void Remove() {
        if (heldItem != null)
        {
            Destroy(heldItem);
            heldItem.transform.SetParent(null);
            heldItem = null;
            Debug.Log("Removed item from hand.");
        }
    }
    public void MoveItemUpDown() {
        if (heldItem != null)
        {
            Vector3 newPosition = heldItem.transform.localPosition;
            newPosition.y += Mathf.Sin(Time.time) * 0.0001f; // Adjust the multiplier for height
            heldItem.transform.localPosition = newPosition;
        }
    }



}
