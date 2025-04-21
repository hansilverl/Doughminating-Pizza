using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHand : MonoBehaviour
{
    [SerializeField] private Transform holdPoint;
    [SerializeField] private float floatHeight = 0.0001f; // Adjust this value to control the floating height
    [SerializeField] private float floatSpeed = 2f; // Adjust this value to control the floating speed
    private GameObject heldItem;
    public bool IsHoldingItem => heldItem != null;
    public GameObject HeldItem => heldItem;
    public void PickUp(GameObject item)
    {
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

    public void Drop() {
        if (heldItem != null)
        {
            heldItem.transform.SetParent(null);
            heldItem = null;
            Debug.Log("Dropped item from hand.");
        }
    }
    public void MoveItemUpDown() {
        if (heldItem != null)
        {
            Vector3 newPosition = heldItem.transform.localPosition;
            newPosition.y += Mathf.Sin(Time.time * floatSpeed) * floatHeight; // Adjust the multiplier for height
            heldItem.transform.localPosition = newPosition;
        }
    }



}
