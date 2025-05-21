using UnityEngine;
using System.Collections;

public class Oven : MonoBehaviour, IInteractable
{
    [Header("Cooking Settings")]
    [SerializeField] private Transform cookingSlot;
    [SerializeField] private float cookDuration = 5f;
    [SerializeField] private float burnDuration = 10f;

    [Header("Door Settings")]
    [SerializeField] private Transform ovenDoor;
    [SerializeField] private Vector3 doorOpenRotation = new Vector3(-90f, 0f, 0f); // Change as needed
    [SerializeField] private float doorAnimationTime = 0.5f;

    private GameObject currentItem;
    private float cookTimer;
    private bool isCooking;
    private bool isDoorOpen = false;

    private CookState currentState = CookState.Raw;

    public void Interact()
    {
        PlayerHand playerHand = GameObject.FindWithTag("Player").GetComponent<PlayerHand>();
        if (playerHand == null) return;

        // Open/close the door
        if (!isDoorOpen)
        {
            StartCoroutine(OpenDoor());
            return;
        }

        if (currentItem == null && playerHand.IsHoldingItem)
        {
            Pizza pizza = playerHand.HeldItem.GetComponent<Pizza>();
            if (pizza != null)
            {
                playerHand.Drop();
                currentItem = pizza.gameObject;
                currentItem.transform.SetParent(cookingSlot);
                currentItem.transform.localPosition = Vector3.zero;
                currentItem.transform.localRotation = Quaternion.identity;
                StartCoroutine(CookPizza(pizza));
            }
        }
        else if (currentItem != null && !playerHand.IsHoldingItem)
        {
            currentItem.transform.SetParent(null);
            playerHand.PickUp(currentItem);
            currentItem = null;
            isCooking = false;
            StopAllCoroutines();
        }
    }

    public string getInteractionText()
    {
        if (!isDoorOpen)
            return "Press 'E' to open oven door";

        if (currentItem == null)
            return "Press 'E' to place pizza in oven";
        else
            return "Press 'E' to take pizza out";
    }

    private IEnumerator CookPizza(Pizza pizza)
    {
        isCooking = true;
        cookTimer = 0f;

        while (isCooking)
        {
            cookTimer += Time.deltaTime;

            if (cookTimer >= burnDuration)
            {
                pizza.SetCookState(CookState.Burnt);
                currentState = CookState.Burnt;
                yield break;
            }
            else if (cookTimer >= cookDuration)
            {
                pizza.SetCookState(CookState.Cooked);
                currentState = CookState.Cooked;
            }

            yield return null;
        }
    }

    private IEnumerator OpenDoor()
    {
        Quaternion startRotation = ovenDoor.localRotation;
        Quaternion endRotation = Quaternion.Euler(doorOpenRotation);

        float elapsed = 0f;
        while (elapsed < doorAnimationTime)
        {
            ovenDoor.localRotation = Quaternion.Slerp(startRotation, endRotation, elapsed / doorAnimationTime);
            elapsed += Time.deltaTime;
            yield return null;
        }

        ovenDoor.localRotation = endRotation;
        isDoorOpen = true;
    }

    private IEnumerator CloseDoor()
    {
        Quaternion startRotation = ovenDoor.localRotation;
        Quaternion endRotation = Quaternion.identity;

        float elapsed = 0f;
        while (elapsed < doorAnimationTime)
        {
            ovenDoor.localRotation = Quaternion.Slerp(startRotation, endRotation, elapsed / doorAnimationTime);
            elapsed += Time.deltaTime;
            yield return null;
        }

        ovenDoor.localRotation = endRotation;
        isDoorOpen = false;
    }

    // Optionally call this to auto-close door
    private void OnTriggerExit(Collider other)
    {
        if (isDoorOpen)
        {
            StartCoroutine(CloseDoor());
        }
    }
}
