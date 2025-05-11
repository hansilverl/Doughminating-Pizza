using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VectorGraphics;

public class PizzaUIController : MonoBehaviour
{

    private Transform player;
    private Transform panel;
    private UnityEngine.UI.Image cookCircle;
    private SVGImage sauce;
    private SVGImage cheese;
    private SVGImage bacon;
    private SVGImage pineapple;
    private SVGImage pepperoni;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        panel = transform.Find("Panel");

        Transform cookCircleTransform = transform.Find("Panel/CookCircle");
        Transform sauceTransform = transform.Find("Panel/Sauce");
        if (sauceTransform != null)
        {
            sauce = sauceTransform.GetComponent<SVGImage>();
            if (sauce != null)
            {
                sauce.color = new Color(sauce.color.r, sauce.color.g, sauce.color.b, 0f); // Keep current color and make fully transparent

            }
        }
        Transform cheeseTransform = transform.Find("Panel/Cheese");
        if (cheeseTransform != null)
        {
            cheese = cheeseTransform.GetComponent<SVGImage>();
            if (cheese != null)
            {
                cheese.color = new Color(cheese.color.r, cheese.color.g, cheese.color.b, 0f); // Keep current color and make fully transparent
            }
        }
        
        Transform baconTransform = transform.Find("Panel/Bacon");
        if (baconTransform != null)
        {
            bacon = baconTransform.GetComponent<SVGImage>();
            if (bacon != null)
            {
                bacon.color = new Color(bacon.color.r, bacon.color.g, bacon.color.b, 0f); // Keep current color and make fully transparent
            }
        }
        Transform pineappleTransform = transform.Find("Panel/Pineapple");
        if (pineappleTransform != null)
        {
            pineapple = pineappleTransform.GetComponent<SVGImage>();
            if (pineapple != null)
            {
                pineapple.color = new Color(pineapple.color.r, pineapple.color.g, pineapple.color.b, 0f); // Keep current color and make fully transparent
            }
        }
        
        Transform pepperoniTransform = transform.Find("Panel/Pepperoni");
        if (pepperoniTransform != null)
        {
            pepperoni = pepperoniTransform.GetComponent<SVGImage>();
            if (pepperoni != null)
            {
                pepperoni.color = new Color(pepperoni.color.r, pepperoni.color.g, pepperoni.color.b, 0f); // Keep current color and make fully transparent
            }
        }
        Debug.Log("PizzaUIController started");
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null && panel != null)
        {
            // Face away from the player only on Y axis (e.g., horizontal rotation)
            Vector3 lookDirection = panel.position - player.position;
            lookDirection.y = 0; // Remove vertical tilt if desired
            panel.forward = lookDirection.normalized;
        }
    }

    public void addIngredient(Ingredient ingredient)
    {
        if (ingredient is Sauce)
        {
            if (sauce != null)
            {
                sauce.color = new Color(sauce.color.r, sauce.color.g, sauce.color.b, 1f); // Make fully opaque
            }
        }
        else if (ingredient is Cheese)
        {
            if (cheese != null)
            {
                cheese.color = new Color(cheese.color.r, cheese.color.g, cheese.color.b, 1f); // Make fully opaque
            }
        }
    }
}
