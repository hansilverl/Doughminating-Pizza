using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pizza : Ingredient
{
    [SerializeField] private GameObject pizzaUI;
    [SerializeField] private CookState CookLevel = CookState.Raw;
    // Time in seconds to cook the pizza

    [SerializeField] private bool hasSauce = false;
    [SerializeField] private bool hasCheese = false;
    [SerializeField] private bool hasBacon = false;
    [SerializeField] private bool hasPineapple = false;
    [SerializeField] private bool hasPepperoni = false;
    [SerializeField] private HashSet<Ingredient> ingredients = new HashSet<Ingredient>();

    // Public properties to access ingredient states
    public bool HasSauce => hasSauce;
    public bool HasCheese => hasCheese;
    public bool HasBacon => hasBacon;
    public bool HasPineapple => hasPineapple;
    public bool HasPepperoni => hasPepperoni;

    void Start()
    {
        Debug.Log("Pizza created with cook level: " + CookLevel);

        // // Initialize panel UI
        if (pizzaUI != null)
        {
            GameObject uiInstance = Instantiate(pizzaUI);
            uiInstance.transform.SetParent(this.transform); // Parent to pizza in world space
            uiInstance.transform.localPosition = new Vector3(0, 0, 1) * 1f; // Offset above the pizza
            pizzaUI = uiInstance;
        }
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

                bool ingredientAdded = false;
                if (ingredient is Sauce && !hasSauce)
                {
                    hasSauce = true;
                    ingredientAdded = true;
                }
                else if (ingredient is Cheese && !hasCheese)
                {
                    hasCheese = true;
                    ingredientAdded = true;
                }
                else if (ingredient is Bacon && !hasBacon)
                {
                    hasBacon = true;
                    ingredientAdded = true;
                }
                else if (ingredient is Pineapple && !hasPineapple)
                {
                    hasPineapple = true;
                    ingredientAdded = true;
                }
                else if (ingredient is Pepperoni && !hasPepperoni)
                {
                    hasPepperoni = true;
                    ingredientAdded = true;
                }

                if (ingredientAdded)
                {
                    playerHand.Remove();
                    AddIngredient(ingredient);
                }
                else
                {
                    Debug.Log($"Cannot add {ingredient.GetType().Name} to the pizza!");
                }
            }
        }
        else if (playerHand != null && !playerHand.IsHoldingItem)
        {
            base.Interact(); // Call the base interact method to pick up the pizza
        }
    }

    public void Cook()
    {
        if (this.CookLevel == CookState.Raw)
        {
            this.CookLevel = CookState.Cooked;
        }
        else if (this.CookLevel == CookState.Cooked)
        {
            this.CookLevel = CookState.Burnt;
        }
    }

    // Method to set the cook state from Oven class
    public void SetCookState(CookState newState)
    {
        CookLevel = newState;
        Debug.Log($"Pizza cook state changed to: {newState}");
    }

    public override string getInteractionText()
    {
        return "Pick " + GetIngredientName();
    }

    public void AddIngredient(Ingredient ingredient)
    {
        pizzaUI.GetComponent<PizzaUIController>().addIngredient(ingredient);
    }
}
