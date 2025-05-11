using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pizza : Ingredient
{
    [SerializeField] private GameObject pizzaUI;
    [SerializeField] private CookState CookLevel = CookState.Raw;

    [SerializeField] private bool hasSauce = false;
    [SerializeField] private bool hasCheese = false;
    [SerializeField] private HashSet<Ingredient> ingredients = new HashSet<Ingredient>();

    void Start()
    {
        Debug.Log("Pizza created with cook level: " + CookLevel);

        // Initialize panel UI
        if (pizzaUI != null)
        {
            GameObject uiInstance = Instantiate(pizzaUI);
            uiInstance.transform.SetParent(this.transform);
            uiInstance.transform.localPosition = new Vector3(0, 0, 1f);
            pizzaUI = uiInstance;
        }
    }

    public CookState GetCookState() => CookLevel;

    public void SetCookState(CookState state)
    {
        this.CookLevel = state;
        Debug.Log($"Pizza is now {state}");
        updateObjectModel();
    }

    public override void Interact()
    {
        PlayerHand playerHand = GameObject.FindWithTag("Player").GetComponent<PlayerHand>();
        if (playerHand != null && playerHand.IsHoldingItem)
        {
            GameObject held = playerHand.HeldItem;
            Ingredient ingredient = held.GetComponent<Ingredient>();
            if (ingredient != null)
            {
                if (this.CookLevel != CookState.Raw) return; // Cannot add ingredients to a cooked pizza

                if (ingredient is Sauce)
                {
                    if (!hasSauce)
                    {
                        hasSauce = true;
                        playerHand.Remove();
                        AddIngredient(ingredient);
                    }
                    else
                    {
                        Debug.Log("Pizza already has sauce!");
                    }
                }
                else if (ingredient is Cheese)
                {
                    if (!hasCheese)
                    {
                        hasCheese = true;
                        playerHand.Remove();
                        AddIngredient(ingredient);
                    }
                    else
                    {
                        Debug.Log("Pizza already has cheese!");
                    }
                }
                else
                {
                    Debug.Log("Cannot add this ingredient to the pizza!");
                }

                updateObjectModel();
            }
        }
        else if (playerHand != null && !playerHand.IsHoldingItem)
        {
            base.Interact(); // Pick up the pizza
        }
    }

    public override string getInteractionText()
    {
        return "Press 'E' to interact with " + GetIngredientName();
    }

    public void AddIngredient(Ingredient ingredient)
    {
        ingredients.Add(ingredient);

        if (pizzaUI != null)
        {
            PizzaUIController uiController = pizzaUI.GetComponent<PizzaUIController>();
            if (uiController != null)
            {
                uiController.addIngredient(ingredient);
            }
        }
    }

    void updateObjectModel()
    {
        // Update visual state of the pizza (e.g., textures, models, materials)
    }

    void Cook()
    {
        if (this.CookLevel == CookState.Raw)
        {
            SetCookState(CookState.Cooked);
        }
        else if (this.CookLevel == CookState.Cooked)
        {
            SetCookState(CookState.Burnt);
        }
    }
}
