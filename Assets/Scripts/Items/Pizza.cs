using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pizza : Ingredient
{
    [SerializeField] private GameObject pizzaUI;
    [SerializeField] private CookState CookLevel = CookState.Raw;

    [SerializeField] private bool hasSauce = false;
    [SerializeField] private bool hasCheese = false;
    [SerializeField] private bool hasBacon = false;
    [SerializeField] private bool hasPineapple = false;
    [SerializeField] private bool hasPepperoni = false;
    [SerializeField] private HashSet<Ingredient> ingredients = new HashSet<Ingredient>();

    [Header("Visual Ingredient Prefabs")]
    [SerializeField] private GameObject saucePrefab;
    [SerializeField] private GameObject cheesePrefab;
    [SerializeField] private GameObject baconPrefab;
    [SerializeField] private GameObject pineapplePrefab;
    [SerializeField] private GameObject pepperoniPrefab;

    // Store references to spawned visual ingredients
    private GameObject visualSauce;
    private GameObject visualCheese;
    private GameObject visualBacon;
    private GameObject visualPineapple;
    private GameObject visualPepperoni;

    // Public properties to access ingredient states
    public bool HasSauce => hasSauce;
    public bool HasCheese => hasCheese;
    public bool HasBacon => hasBacon;
    public bool HasPineapple => hasPineapple;
    public bool HasPepperoni => hasPepperoni;
    public CookState GetCookLevel() => CookLevel;

    void Start()
    {
        Debug.Log("Pizza created with cook level: " + CookLevel);

        // Initialize panel UI
        if (pizzaUI != null)
        {
            GameObject uiInstance = Instantiate(pizzaUI);
            uiInstance.transform.SetParent(this.transform);
            uiInstance.transform.localPosition = new Vector3(0, 0, 1) * 1f;
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
                if (this.CookLevel != CookState.Raw)
                {
                    playerHand.InvalidAction("Pizza is " + this.CookLevel.ToString().ToLower() + "! Can't add!", 2f);
                    return;
                }

                bool ingredientAdded = false;
                if (ingredient is Sauce && !hasSauce)
                {
                    hasSauce = true;
                    ingredientAdded = true;
                    SpawnVisualIngredient(IngredientType.Sauce);
                }
                else if (ingredient is Cheese && !hasCheese)
                {
                    hasCheese = true;
                    ingredientAdded = true;
                    SpawnVisualIngredient(IngredientType.Cheese);
                }
                else if (ingredient is Bacon && !hasBacon)
                {
                    hasBacon = true;
                    ingredientAdded = true;
                    SpawnVisualIngredient(IngredientType.Bacon);
                }
                else if (ingredient is Pineapple && !hasPineapple)
                {
                    hasPineapple = true;
                    ingredientAdded = true;
                    SpawnVisualIngredient(IngredientType.Pineapple);
                }
                else if (ingredient is Pepperoni && !hasPepperoni)
                {
                    hasPepperoni = true;
                    ingredientAdded = true;
                    SpawnVisualIngredient(IngredientType.Pepperoni);
                }

                if (ingredientAdded)
                {
                    playerHand.Remove();
                    AddIngredient(ingredient);
                }
                else
                {
                    playerHand.InvalidAction("You can't add " + ingredient.GetIngredientName() + " to the pizza!", 2f);
                }
            }

            Tool tool = held.GetComponent<Tool>();
            if (tool != null)
            {
                playerHand.InvalidAction("You can't use " + tool.GetToolName() + " on the pizza!", 2f);
            }
        }
        else if (playerHand != null && !playerHand.IsHoldingItem)
        {
            base.Interact(); // Call the base interact method to pick up the pizza
        }
    }

    
    private void SpawnVisualIngredient(IngredientType ingredientType)
{
    GameObject prefab = null;
    Vector3 relativeOffset = Vector3.zero;
    Quaternion worldRotation = Quaternion.identity;
    Vector3 localScale = Vector3.one;
    
    // Height adjustment to ensure ingredients appear properly on top of the pizza
    float heightAdjustment = 0.02f; // Adjust this value as needed to move ingredients up
    
    switch (ingredientType)
    {
        case IngredientType.Sauce:
            prefab = saucePrefab;
            relativeOffset = new Vector3(0f, 0.0124f, 0.003f);
            worldRotation = Quaternion.Euler(-90f, 0f, 0f);
            localScale = new Vector3(1.472f, 1.55f, 1.472f);
            heightAdjustment = 0.005f; // Sauce needs less height
            break;
        case IngredientType.Cheese:
            prefab = cheesePrefab;
            relativeOffset = new Vector3(-0.001f, 0.024f, 0f);
            worldRotation = Quaternion.Euler(0f, 0f, 0f);
            localScale = new Vector3(0.2f, 0.1f, 0.2f);
            heightAdjustment = 0.01f;
            break;
        case IngredientType.Bacon:
            prefab = baconPrefab;
            relativeOffset = new Vector3(-0.013f, 0.02f, -0.02f);
            worldRotation = Quaternion.Euler(0f, 0f, 0f);
            localScale = new Vector3(0.19f, 0.23f, 0.2f);
            heightAdjustment = 0.03f;
            break;
        case IngredientType.Pineapple:
            prefab = pineapplePrefab;
            relativeOffset = new Vector3(-0.009f, 0.0471f, -0.004f);
            worldRotation = Quaternion.Euler(0f, 0f, 0f);
            localScale = new Vector3(0.15f, 0.15f, 0.15f);
            heightAdjustment = 0.03f;
            break;
        case IngredientType.Pepperoni:
            prefab = pepperoniPrefab;
            relativeOffset = new Vector3(0.0025f, 0.033f, 0.002f);
            worldRotation = Quaternion.Euler(0f, 0f, 0f);
            localScale = new Vector3(0.18f, 0.18f, 0.18f);
            heightAdjustment = 0.03f;
            break;
    }

    if (prefab != null)
    {
        // First get the local space position based on our pizza's rotation
        Vector3 localPosition = transform.InverseTransformPoint(transform.position + relativeOffset);
        
        // Apply height adjustment in the pizza's local up direction
        // Since pizza is rotated -90 on X, its "up" is actually world Z
        localPosition.z += heightAdjustment;
        
        // Now transform back to world space
        Vector3 worldPosition = transform.TransformPoint(localPosition);
        
        // Instantiate and set up the ingredient
        GameObject visualIngredient = Instantiate(prefab, worldPosition, worldRotation);
        visualIngredient.transform.localScale = localScale;
        visualIngredient.transform.SetParent(this.transform, true);

        switch (ingredientType)
        {
            case IngredientType.Sauce: visualSauce = visualIngredient; break;
            case IngredientType.Cheese: visualCheese = visualIngredient; break;
            case IngredientType.Bacon: visualBacon = visualIngredient; break;
            case IngredientType.Pineapple: visualPineapple = visualIngredient; break;
            case IngredientType.Pepperoni: visualPepperoni = visualIngredient; break;
        }

        Debug.Log($"Spawned visual {ingredientType} at world position: {worldPosition}, world rotation: {worldRotation.eulerAngles}");
    }
    else
    {
        Debug.LogWarning($"No prefab assigned for {ingredientType}!");
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

        pizzaUI.GetComponent<PizzaUIController>().setCookLevel(this.CookLevel);
    }

    public void SetCookState(CookState newState)
    {
        CookLevel = newState;
        Debug.Log($"Pizza cook state changed to: {newState}");

        if (pizzaUI != null)
        {
            pizzaUI.GetComponent<PizzaUIController>().setCookLevel(this.CookLevel);
        }
    }

    public override string getInteractionText()
    {
        PlayerHand playerHand = GameObject.FindWithTag("Player").GetComponent<PlayerHand>();
        if (!playerHand.IsHoldingItem)
        {
            return "Pick up the pizza";
        }
        else
        {
            if (playerHand.HeldItem.GetComponent<Ingredient>() != null)
            {
                Ingredient heldIngredient = playerHand.HeldItem.GetComponent<Ingredient>();
                return "Add " + heldIngredient.GetIngredientName() + " to the pizza";
            }
            if (playerHand.HeldItem.GetComponent<Tool>() != null)
            {
                return "Use " + playerHand.HeldItem.GetComponent<Tool>().GetToolName() + " on the pizza";
            }
        }
        return "Pick " + GetIngredientName();
    }

    public void AddIngredient(Ingredient ingredient)
    {
        pizzaUI.GetComponent<PizzaUIController>().addIngredient(ingredient);
    }

    // Enum to identify ingredient types
    private enum IngredientType
    {
        Sauce,
        Cheese,
        Bacon,
        Pineapple,
        Pepperoni
    }
}