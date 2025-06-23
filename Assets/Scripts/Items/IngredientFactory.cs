using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientFactory : MonoBehaviour, IInteractable
{
    public static float planeOffset = 6.79f;
    [SerializeField] private GameObject prefab;

    public void Interact()
    {
        Debug.Log("Default Interact() called on IngredientFactory. This should not be called directly.");
        if (prefab == null)
        {
            return;
        }
        PlayerHand playerHand = GameObject.FindWithTag("Player").GetComponent<PlayerHand>();
        if(playerHand.IsHoldingItem) {
            playerHand.InvalidAction("You are already holding an item!", 2f);
            return;
        }


        // Vector3 spawnPosition = transform.position + transform.forward * 0.5f + Vector3.up * 0.3f;
        Vector3 spawnPosition = transform.position;
        GameObject spawned = Instantiate(prefab, spawnPosition, Quaternion.identity);

        Ingredient ingredient = spawned.GetComponent<Ingredient>();
        if (ingredient != null)
        {
            playerHand.PickUp(spawned);
        }

    }

    // public void Interact(Transform point) {
    //     if(prefab == null)
    //     {
    //         return;
    //     }
    //     Debug.Log("Spawning ingredient at: " + point.position);
    //     Vector3 spawnRotation = point.rotation.eulerAngles;
    //     Vector3 spawnPosition = point.position;
    //     Vector3 itemRotation = prefab.GetComponent<IPickable>().GetDefaultRotation();
    //     float itemOffset = prefab.GetComponent<IPickable>().GetCounterPositionOffset();
    //     Debug.Log("Item offset: " + itemOffset);
    //     GameObject spawned = Instantiate(prefab, Vector3.zero, Quaternion.Euler(spawnRotation.x + itemRotation.x, spawnRotation.y + itemRotation.y, spawnRotation.z + itemRotation.z), point);
    //     spawned.transform.localPosition = new Vector3(0f, itemOffset, 0f);
    //     Debug.Log("Spawned ingredient: " + spawned.name);
    // }

    public void Interact(Transform point) {
        if (prefab == null) {
            return;
        }

        Vector3 spawnRotation = point.rotation.eulerAngles;
        Vector3 itemRotation = prefab.GetComponent<IPickable>().GetDefaultRotation();
        float itemOffset = prefab.GetComponent<IPickable>().GetCounterPositionOffset();

        // 1. Instantiate WITHOUT parent
        GameObject spawned = Instantiate(prefab, point.position, Quaternion.Euler(
            spawnRotation.x + itemRotation.x,
            spawnRotation.y + itemRotation.y,
            spawnRotation.z + itemRotation.z
        ));
        
        // 2. THEN parent it safely using worldPositionStays=true
        spawned.transform.SetParent(point, true);

        // 3. AFTER parenting, adjust the local position
        spawned.transform.localPosition = new Vector3(
            spawned.transform.localPosition.x, 
            spawned.transform.localPosition.y + itemOffset + planeOffset, // Adjust only the y component
            spawned.transform.localPosition.z
        );
    }


    public string getInteractionText()
    {
        if (prefab == null)
            return "No ingredient assigned";

        Ingredient ingredient = prefab.GetComponent<Ingredient>();
        if (ingredient != null)
        {
            return "Grab " + ingredient.GetIngredientName();
        }
        return "Grab an ingredient";
    }
}
