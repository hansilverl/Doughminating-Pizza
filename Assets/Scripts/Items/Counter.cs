using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Counter : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        Debug.Log("Interacting with Counter");
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            Vector3 hitCoordinates = hitInfo.point;
            Debug.Log($"Raycast hit coordinates: {hitCoordinates}");
            PlayerHand playerHand = GameObject.FindWithTag("Player").GetComponent<PlayerHand>();
            if (playerHand.IsHoldingItem)
            {
                GameObject item = playerHand.HeldItem;
                if (item != null)
                {
                    // Place the item on the counter
                    // item.transform.position = hitCoordinates;
                    // item.transform.SetParent(transform); // Set the parent to the counter
                    // playerHand.DropItem(); // Drop the item from the player's hand
                    Debug.Log("Item placed on counter");
                    GameObject counter = gameObject;
                    Vector3 counterRotation = counter.transform.rotation.eulerAngles;
                    playerHand.Drop();
                    item.transform.position = new Vector3(hitCoordinates.x, hitCoordinates.y, hitCoordinates.z); // Adjust the height as needed
                    item.transform.rotation = Quaternion.Euler(counterRotation.x, counterRotation.y, counterRotation.z);
                    item.transform.SetParent(counter.transform); // Set the parent to the counter
                    Debug.Log("Counter: " + counter.name);

                }
            }
        }
    }

    public string getInteractionText()
    {
        if (GameObject.FindWithTag("Player").GetComponent<PlayerHand>().IsHoldingItem)
        {
            return "Press 'E' to place item on counter";
        }
        return "";
    }
}
