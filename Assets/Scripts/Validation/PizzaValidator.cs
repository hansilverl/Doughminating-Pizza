using System;
using System.Collections.Generic;

/// <summary>
/// Helper class for pizza ingredient validation
/// </summary>
public static class PizzaValidator
{
    private static readonly Dictionary<Type, Func<Pizza, bool>> IngredientCheckers = new Dictionary<Type, Func<Pizza, bool>>
    {
        { typeof(Sauce), pizza => pizza.HasSauce },
        { typeof(Cheese), pizza => pizza.HasCheese },
        { typeof(Bacon), pizza => pizza.HasBacon },
        { typeof(Pineapple), pizza => pizza.HasPineapple },
        { typeof(Pepperoni), pizza => pizza.HasPepperoni }
    };

    /// <summary>
    /// Validates if a pizza has all required ingredients and no unwanted ones
    /// </summary>
    /// <param name="pizza">The pizza to validate</param>
    /// <param name="wantedIngredients">List of required ingredient types</param>
    /// <returns>True if the pizza matches the requirements exactly</returns>
    public static bool ValidatePizza(Pizza pizza, List<Type> wantedIngredients)
    {
        if (pizza == null || wantedIngredients == null)
            return false;

        // Check if pizza has all required ingredients
        foreach (Type ingredientType in wantedIngredients)
        {
            if (IngredientCheckers.TryGetValue(ingredientType, out var checker))
            {
                if (!checker(pizza))
                    return false;
            }
        }

        // Check that pizza doesn't have any unwanted ingredients
        foreach (var kvp in IngredientCheckers)
        {
            Type ingredientType = kvp.Key;
            var checker = kvp.Value;
            
            // If the ingredient is not wanted but present on the pizza, it's invalid
            if (!wantedIngredients.Contains(ingredientType) && checker(pizza))
                return false;
        }

        return true;
    }
}
