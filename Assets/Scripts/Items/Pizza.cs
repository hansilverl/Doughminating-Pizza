using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pizza : Ingredient
{
    [SerializeField] private GameObject pizzaUI;
    [SerializeField] private Vector3 pizzaUIdirection = new Vector3(0, 0, 1f);
    [SerializeField] private CookState CookLevel = CookState.Raw;
    // Time in seconds to cook the pizza

    [SerializeField] private bool hasSauce = false;
    [SerializeField] private bool hasCheese = false;
    [SerializeField] private HashSet<Ingredient> ingredients = new HashSet<Ingredient>();

    [SerializeField] private GameObject meshRaw;
    [SerializeField] private GameObject meshCooked;
    [SerializeField] private GameObject meshBurnt;

    void Start()
    {
        Debug.Log("Pizza created with cook level: " + CookLevel);

        // // Initialize panel UI
        if (pizzaUI != null)
        {
            GameObject uiInstance = Instantiate(pizzaUI);
            uiInstance.transform.SetParent(this.transform); // Parent to pizza in world space
            uiInstance.transform.localPosition = this.pizzaUIdirection * 1f; // Offset above the pizza
            pizzaUI = uiInstance;
        }
    }

    private void UpdatePizzaVisual()
    {
        meshRaw.SetActive(false);
        meshCooked.SetActive(false);
        meshBurnt.SetActive(false);

        switch (CookLevel)
        {
            case CookState.Raw:
                meshRaw.SetActive(true);
                break;
            case CookState.Cooked:
                meshCooked.SetActive(true);
                break;
            case CookState.Burnt:
                meshBurnt.SetActive(true);
                break;
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

                if (ingredient is Sauce)
                {
                    if (!hasSauce)
                    {
                        hasSauce = true;
                        playerHand.Remove();
                        AddIngredient(ingredient);
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
        if (hasSauce && hasCheese)
        {
            Debug.Log("Pizza has cheese and sauce, updating model...");
            UpdatePizzaVisual();
            //     GameObject newModel = Resources.Load<GameObject>("Pizza_Uncooked");

            //     if (newModel != null)
            //     {
            //         Transform parent = this.transform.parent;
            //         Vector3 position = this.transform.position;

            //         GameObject instance = Instantiate(newModel, position, Quaternion.identity);
            //         if (parent != null) instance.transform.SetParent(parent);

            //         Pizza newPizzaScript = instance.AddComponent<Pizza>();
            //         if (this.hasCheese)
            //         {
            //             newPizzaScript.AddIngredient(new Cheese());
            //         }
            //         if (this.hasSauce)
            //         {
            //             newPizzaScript.AddIngredient(new Sauce());
            //         }
            //         foreach (Ingredient ingredient in ingredients)
            //         {
            //             newPizzaScript.AddIngredient(ingredient);
            //         }
            //         newPizzaScript.CookLevel = this.CookLevel;

            //         // // Destroy the old UI to avoid ghosting
            //         // if (pizzaUI != null)
            //         // {
            //         //     Destroy(pizzaUI);
            //         //     pizzaUI = null;
            //         // }

            //         // // Instantiate new UI
            //         // GameObject pizzaUIPrefab = Resources.Load<GameObject>("PizzaUI");
            //         // if (pizzaUIPrefab != null)
            //         // {
            //         //     GameObject newUI = Instantiate(pizzaUIPrefab, instance.transform);
            //         //     newUI.transform.localPosition = new Vector3(0, 0, 1f);
            //         //     newPizzaScript.pizzaUI = newUI;
            //         // }
            //         // else
            //         // {
            //         //     Debug.LogWarning("Pizza UI prefab not found.");
            //         // }

            //         Destroy(this.gameObject);
            //     }
            //     else
            //     {
            //         Debug.LogError("Failed to load the new pizza model!");
            //     }
            // }
        }
        UpdatePizzaVisual();
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
        pizzaUI.GetComponent<PizzaUIController>().addIngredient(ingredient);
    }
}
