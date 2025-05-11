using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class Counter : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            Vector3 hitCoordinates = hitInfo.point;
            PlayerHand playerHand = GameObject.FindWithTag("Player").GetComponent<PlayerHand>();
            if (playerHand.IsHoldingItem)
            {
                GameObject item = playerHand.HeldItem;
                if (item != null)
                {
                    GameObject counter = gameObject;
                    Vector3 counterRotation = counter.transform.rotation.eulerAngles;
                    float counterOffset = item.GetComponent<IPickable>().GetCounterPositionOffset();
                    Vector3 defaultRotation = item.GetComponent<IPickable>().GetDefaultRotation();
                    playerHand.Drop();
                    item.transform.position = hitCoordinates;
                    item.transform.rotation = Quaternion.Euler(
                        counterRotation.x + defaultRotation.x,
                        counterRotation.y + defaultRotation.y,
                        counterRotation.z + defaultRotation.z
                    );
                    item.transform.SetParent(counter.transform);
                    item.transform.localPosition = new Vector3(
                        item.transform.localPosition.x,
                        item.transform.localPosition.y + counterOffset,
                        item.transform.localPosition.z
                    );
                }
            }
        }
    }


    private void Place(GameObject item, Vector3 hitCoordinates)
    {
        // Logic to place the item on the counter
        // This could involve setting the item's position and rotation to match the counter's surface
        // For example:
        item.transform.position = new Vector3(hitCoordinates.x, hitCoordinates.y, hitCoordinates.z); // Adjust the height as needed
    }

    public string getInteractionText()
    {
        if (GameObject.FindWithTag("Player").GetComponent<PlayerHand>().IsHoldingItem)
        {
            return "Place item";
        }
        return "";
    }
}
