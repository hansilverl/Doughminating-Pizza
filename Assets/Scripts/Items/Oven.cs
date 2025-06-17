using UnityEngine;
using System.Collections;

public class Oven : MonoBehaviour, IInteractable
{
    [Header("Cooking Settings")]
    [SerializeField] private Vector3 pizzaPlacementPosition = new Vector3(-0.7f, 1.633f, 20.5f);
    [SerializeField] private float cookDuration = 5f;
    [SerializeField] private float burnDuration = 10f;

    private GameObject currentPizza;
    private float cookTimer;
    private bool isCooking;
    private CookState currentState = CookState.Raw;

    public void Interact()
    {
        PlayerHand playerHand = GameObject.FindWithTag("Player")?.GetComponent<PlayerHand>();
        if (playerHand == null) return;

        if (currentPizza == null && playerHand.IsHoldingItem)
        {
            Pizza pizza = playerHand.HeldItem.GetComponent<Pizza>();
            if (pizza != null)
            {
                playerHand.Drop();
                currentPizza = pizza.gameObject;

                // Place it in the world — don't parent or use local position
                currentPizza.transform.position = pizzaPlacementPosition;
                Vector3 defaultRotation = currentPizza.GetComponent<IPickable>()?.GetDefaultRotation() ?? Vector3.zero;
                currentPizza.transform.rotation = Quaternion.Euler(defaultRotation.x, defaultRotation.y, defaultRotation.z);

                StartCoroutine(CookPizza(pizza));
            }
        }
        else if (currentPizza != null && !playerHand.IsHoldingItem)
        {
            currentPizza.transform.SetParent(null); // Just in case it ever was parented
            playerHand.PickUp(currentPizza);
            currentPizza = null;
            isCooking = false;
            StopAllCoroutines();
        }
    }

    public string getInteractionText()
    {
        PlayerHand playerHand = GameObject.FindWithTag("Player")?.GetComponent<PlayerHand>();

        if (currentPizza == null && playerHand != null && playerHand.IsHoldingItem)
        {
            if (playerHand.HeldItem.GetComponent<Pizza>() != null)
                return "Press 'E' to place pizza in oven";
        }
        else if (currentPizza != null)
        {
            return "Press 'E' to remove pizza";
        }
        return "";
    }

    private IEnumerator CookPizza(Pizza pizza)
    {
        isCooking = true;
        cookTimer = 0f;
        currentState = CookState.Raw;

        while (isCooking)
        {
            Debug.Log($"Cooking pizza: {cookTimer}/{cookDuration} seconds");
            cookTimer += Time.deltaTime;

            if (cookTimer >= burnDuration)
            {
                pizza.SetCookState(CookState.Burnt);
                currentState = CookState.Burnt;
                break;
            }
            else if (cookTimer >= cookDuration && currentState == CookState.Raw)
            {
                pizza.SetCookState(CookState.Cooked);
                currentState = CookState.Cooked;
            }

            yield return null;
        }
    }
}
