using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientFactory : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject prefab;

    public void Interact()
    {
        if (prefab == null)
        {
            Debug.LogError("No ingredient prefab assigned!");
            return;
        }
        PlayerHand playerHand = GameObject.FindWithTag("Player").GetComponent<PlayerHand>();
        if(playerHand.IsHoldingItem) {
            Debug.Log("Player is already holding an item. Cannot spawn another.");
            return;
        }

        Vector3 spawnPosition = transform.position + transform.forward * 0.5f + Vector3.up * 0.3f;
        GameObject spawned = Instantiate(prefab, spawnPosition, Quaternion.identity);

        Ingredient ingredient = spawned.GetComponent<Ingredient>();
        if (ingredient != null)
        {
            Debug.Log("Spawned Ingredient: " + ingredient.GetIngredientName());
            playerHand.PickUp(spawned);
        }
        else
        {
            Debug.LogWarning("Spawned object doesn't have an Ingredient component.");
        }

    }

    public string getInteractionText()
    {
        if (prefab == null)
            return "No ingredient assigned";

        Ingredient ingredient = prefab.GetComponent<Ingredient>();
        if (ingredient != null)
        {
            return "Press 'E' to grab a " + ingredient.GetIngredientName();
        }
        return "Press 'E' to grab an ingredient";
    }
}
