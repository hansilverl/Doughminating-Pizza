using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerInteraction : MonoBehaviour
{
    // SerializeField allows you to modify the variable in the Unity Inspector
    // without making it public
    [SerializeField] private float interactionDistance = 3f; // Distance within which the player can interact with objects
    [SerializeField] private Camera playerCamera;
    [SerializeField] private TMP_Text interactionTextUI;// Text displayed when the player is close to an interactable object 
    [SerializeField] private GameObject interactionPanel; // Panel that contains the interaction text

    private IInteractable currentInteractable; // Reference to the current interactable object
    // Start is called before the first frame update
    void Start()
    {
        playerCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        HandleInteractionCheck();
        HandleInteractionInput();
    }

    void HandleInteractionCheck()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactionDistance))
        {
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();

            if (interactable != null)
            {
                currentInteractable = interactable;
                interactionTextUI.text = interactable.getInteractionText();
                interactionPanel.SetActive(true);
                return;
            }
        }

        currentInteractable = null;
        interactionPanel.SetActive(false);
    }

    void HandleInteractionInput()
    {
        if (currentInteractable != null && Input.GetKeyDown(KeyCode.E))
        {
            currentInteractable.Interact();
        }
    }
}
