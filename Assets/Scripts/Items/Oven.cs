using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Oven : MonoBehaviour, IInteractable
{
    [Header("UI Elements")]
    [SerializeField] private Image ovenTimerImage;

    [Header("Cooking Settings")]
    [SerializeField] private Vector3 pizzaPlacementPosition = new Vector3(-0.7f, 1.633f, 20.5f);
    [SerializeField] private float cookDuration = 5f;
    [SerializeField] private float burnDuration = 10f;



    private GameObject currentPizza;
    private float cookTimer;
    private bool isCooking;
    private CookState currentState = CookState.Raw;
    [SerializeField] private ParticleSystem smokeEffect;

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
            else
            {
                playerHand.InvalidAction("You can only place a pizza!", 2f);
            }
        }
        else if (currentPizza != null && playerHand.IsHoldingItem)
        {
            playerHand.InvalidAction("There's an item in the oven already!", 2f);
        }
        else if (currentPizza != null && !playerHand.IsHoldingItem)
        {
            currentPizza.transform.SetParent(null); // Just in case it ever was parented
            playerHand.PickUp(currentPizza);
            currentPizza = null;
            isCooking = false;
            StopAllCoroutines();

            if (ovenTimerImage != null)
            {
                ovenTimerImage.fillAmount = 0f;
                ovenTimerImage.color = Color.green;
                ovenTimerImage.transform.localRotation = Quaternion.identity;
                ovenTimerImage.gameObject.SetActive(false);
            }
            if (smokeEffect)
                smokeEffect.gameObject.SetActive(false);
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
        currentState = pizza.GetCookLevel();
        cookTimer = currentState == CookState.Raw ? 0f : currentState == CookState.Cooked ? cookDuration : burnDuration;

        if (ovenTimerImage != null)
        {
            ovenTimerImage.fillAmount = 1f;
            ovenTimerImage.gameObject.SetActive(true);
            ovenTimerImage.color = Color.green;
            ovenTimerImage.transform.localRotation = Quaternion.identity;
        }

        // Capture the pizza's original color
        Color originalColor = pizza.GetComponent<Renderer>().material.color;
        // burnt dough colour for the pizza
        Color burntColor = new Color32(128, 64, 26, 255); // Dark brown color for burnt pizza

        while (isCooking)
        {
            cookTimer += Time.deltaTime;

            // Phase 1: Raw -> Burnt (just darken over time)
            float t = Mathf.Clamp01(cookTimer / burnDuration);
            pizza.SetPizzaColor(Color.Lerp(originalColor, burntColor, t));

            if (ovenTimerImage != null)
            {
                ovenTimerImage.fillAmount = 1f - t;
                ovenTimerImage.color = Color.Lerp(Color.green, Color.red, t);
            }

            if (cookTimer >= burnDuration)
            {
                pizza.SetPizzaColor(burntColor);
                pizza.SetCookState(CookState.Burnt);
                currentState = CookState.Burnt;
                if (smokeEffect)
                {
                    smokeEffect.gameObject.SetActive(true);
                    smokeEffect.Play();
                }
                break;
            }
            else if (cookTimer >= cookDuration && currentState == CookState.Raw)
            {
                pizza.SetCookState(CookState.Cooked);
                currentState = CookState.Cooked;
            }
            yield return null;
        }

        // Clean up
        if (ovenTimerImage != null)
        {
            ovenTimerImage.fillAmount = 0f;
            ovenTimerImage.color = Color.green;
            ovenTimerImage.transform.localRotation = Quaternion.identity;
            ovenTimerImage.gameObject.SetActive(false);
        }
    }
}