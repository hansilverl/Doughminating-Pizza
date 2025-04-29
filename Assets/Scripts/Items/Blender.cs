using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blender : Tool
{
    private bool isBlending = false;
    [SerializeField] private Transform spawnPoint;
    public override void Interact()
    {
        if(isBlending) return; // Prevent multiple interactions at once
        PlayerHand playerHand = GameObject.FindWithTag("Player").GetComponent<PlayerHand>();
        if (playerHand != null && playerHand.IsHoldingItem)
        {
            GameObject held = playerHand.HeldItem;
            Ingredient ingredient = held.GetComponent<Ingredient>();
            if (ingredient != null && ingredient is Tomato)
            {
                isBlending = true; // Set the flag to true
                StartCoroutine(Blend(playerHand, ingredient));


                // blending tomato logic

                // playerHand.Remove();

                // //Reuse IngredientFactory
                // IngredientFactory factory = GetComponent<IngredientFactory>();
                // if (factory != null)
                // {
                //     factory.Interact(spawnPoint); // Triggers the sauce spawn!
                //     AudioSource audio = GetComponent<AudioSource>();
                //     if (audio != null)
                //     {
                //         audio.Play(); // Play the blending sound
                //     }
                // }

            }
        }
    }

    private IEnumerator Blend(PlayerHand playerHand, Ingredient ingredient)
    {
        // Logic for blending the ingredient
        // This could involve changing the ingredient's state or spawning a new blended item
        // For example, you might want to create a new GameObject for the blended item and set its properties
        // You can also play a blending animation or sound here if needed
        playerHand.Remove();
        AudioSource audio = GetComponent<AudioSource>();
        if (audio != null)
        {
            audio.Play(); // Play the blending sound
            yield return new WaitForSeconds(audio.clip.length);
        } else yield return null;

        // Spawn the blended item (e.g., Tomato Sauce)
        IngredientFactory factory = GetComponent<IngredientFactory>();
        if (factory != null)
        {
            factory.Interact(spawnPoint); // Triggers the sauce spawn!
        }
        isBlending = false; // Reset the flag after blending is done
    }

    public override string getInteractionText()
    {
        PlayerHand playerHand = GameObject.FindWithTag("Player").GetComponent<PlayerHand>();
        if (playerHand != null && playerHand.IsHoldingItem)
        {
            GameObject held = playerHand.HeldItem;
            Ingredient ingredient = held.GetComponent<Ingredient>();
            if (ingredient != null && ingredient is Tomato)
            {
                return "Press 'E' to blend " + ingredient.GetIngredientName();
            }
        }

        return "";
    }
}
