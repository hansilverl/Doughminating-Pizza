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
    [SerializeField] private HashSet<Ingredient> ingredients = new HashSet<Ingredient>();

    void Start()
    {
        Debug.Log("Pizza created with cook level: " + CookLevel);

        // // Initialize panel UI
        if (pizzaUI != null)
        {
            GameObject uiInstance = Instantiate(pizzaUI);
            uiInstance.transform.SetParent(this.transform); // Parent to pizza in world space
            uiInstance.transform.localPosition = new Vector3(0, 0, 1) * 1f; // Offset above the pizza
        }
        // {
        //     uiInstance.transform.SetParent(this.transform, false); // Parent to pizza in world space
        //     pizzaUI = uiInstance;

        //     // Fix local transform to avoid scale/rotation issues
        //     RectTransform rt = pizzaUI.GetComponent<RectTransform>();
        //     if (rt != null)
        //     {
        //         rt.localScale = Vector3.one;
        //         rt.localRotation = Quaternion.identity;
        //         rt.localPosition = Vector3.up * 2f; // Offset above the pizza
        //     }
        //     else
        //     {
        //         // Fallback if it's not a UI element
        //         pizzaUI.transform.localPosition = Vector3.up * 2f;
        //         pizzaUI.transform.localRotation = Quaternion.identity;
        //         pizzaUI.transform.localScale = Vector3.one;
        //     }
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
                this.updateObjectModel();
            }
        }
        else if (playerHand != null && !playerHand.IsHoldingItem)
        {
            base.Interact(); // Call the base interact method to pick up the pizza
        }
    }

    void updateObjectModel()
    {
        // Update the object model based on the current state of the pizza
        // This could involve changing the appearance of the pizza based on its ingredients and cook level
        // For example, you might want to change the texture or model of the pizza to reflect its state
    }

    void Cook()
    {
        if (this.CookLevel == CookState.Raw)
        {
            this.CookLevel = CookState.Cooked;
        }
        else if (this.CookLevel == CookState.Cooked)
        {
            this.CookLevel = CookState.Burnt;
        }
        updateObjectModel();
    }

    public override string getInteractionText()
    {
        return "Pick " + GetIngredientName();
    }

    public void AddIngredient(Ingredient ingredient)
    {
        if (ingredient is Sauce)
        {
            this.hasSauce = true;
        }
        else if (ingredient is Cheese)
        {
            this.hasCheese = true;
        }
        else
        {
            ingredients.Add(ingredient);
        }
    }
}
