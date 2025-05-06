using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VectorGraphics;

public class PizzaUIController : MonoBehaviour
{
    private UnityEngine.UI.Image cookCircle;
    private SVGImage sauce;
    private SVGImage cheese;
    // Start is called before the first frame update
    void Start()
    {
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
        Debug.Log("PizzaUIController started");
    }

    // Update is called once per frame
    void Update()
    {

    }
}
