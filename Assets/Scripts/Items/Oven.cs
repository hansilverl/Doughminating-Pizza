using UnityEngine;
using System.Collections;

public class Oven : MonoBehaviour, IInteractable
{
    // serializeField is used to expose private variables in the Unity Inspector
    [SerializeField] private Transform cookingSlot; // Where the pizza will sit
    [SerializeField] private float cookDuration = 5f;     // Time to Cooked (5 seconds)
    [SerializeField] private float burnDuration = 10f;    // Time time to Burnt (10 seconds)

    private GameObject currentItem;
    private float cookTimer;
    private bool isCooking;

    private CookState currentState = CookState.Raw;

    public void Interact()
    {
        PlayerHand playerHand = GameObject.FindWithTag("Player").GetComponent<PlayerHand>();

        if (playerHand == null) return;

        if (currentItem == null && playerHand.IsHoldingItem)
        {
            // Place pizza in oven
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
            // Take pizza out of oven
            currentItem.transform.SetParent(null);
            playerHand.PickUp(currentItem);
            currentItem = null;
            isCooking = false;
            StopAllCoroutines();
        }
    }

    public string getInteractionText()
    {
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
}
