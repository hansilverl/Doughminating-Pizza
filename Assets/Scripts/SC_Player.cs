using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(CharacterController))]
public class SC_Player : MonoBehaviour

{
    [Header("Pause Menu")]
    public GameObject pauseMenuUI;
    private bool isPaused = false;

    [Header("Player Settings")]
    public float moveSpeed = 5f;
    public float lookSensitivity = 2f;
    public Camera playerCamera;

    [Header("Interaction Settings")]
    public float interactionDistance = 3f;
    public TMP_Text interactionTextUI;
    public GameObject interactionPanel;
    public GameObject toastPanel;

    private PlayerControls _actions;
    private CharacterController controller;
    private Vector2 moveInput;
    private Vector2 lookInput;
    private float verticalLookRotation = 0f;
    private float verticalVelocity = 0f;
    public float gravity = -9.81f;

    private IInteractable currentInteractable;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        _actions = new PlayerControls();

        _actions.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        _actions.Player.Move.canceled += _ => moveInput = Vector2.zero;

        _actions.Player.Look.performed += ctx => lookInput = ctx.ReadValue<Vector2>();
        _actions.Player.Look.canceled += _ => lookInput = Vector2.zero;

        _actions.Player.Interact.performed += _ => HandleInteractionInput(); // 🔑 uses Input Action "Interact"

        _actions.Player.Pause.performed += _ => TogglePauseMenu();
        // _actions.Player.Pause.canceled += _ => TogglePauseMenu();
    }

    void OnEnable() => _actions.Enable();
    void OnDisable() => _actions.Disable();

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        HandleLook();
        HandleInteractionCheck();
        HandleMovement();
    }

    void HandleLook()
    {
        float mouseX = lookInput.x * lookSensitivity;
        float mouseY = lookInput.y * lookSensitivity;

        transform.Rotate(Vector3.up * mouseX);
        verticalLookRotation -= mouseY;
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, -90f, 90f);
        playerCamera.transform.localRotation = Quaternion.Euler(verticalLookRotation, 0f, 0f);
    }

    void HandleMovement()
    {
        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;
        move *= moveSpeed;

        // Apply gravity
        if (controller.isGrounded && verticalVelocity < 0)
            verticalVelocity = -2f;

        verticalVelocity += gravity * Time.deltaTime;
        move.y = verticalVelocity;

        controller.Move(move * Time.deltaTime);
    }

    void ToggleOutline(IInteractable target, bool state)
    {
        if (target == null) return;

        var mono = target as MonoBehaviour;
        if (mono == null) return;

        var highlight = mono.GetComponent<SimpleMaterialHighlighter>();
        if (highlight != null)
        {
            highlight.SetHighlight(state);
        }
    }


    void HandleInteractionCheck()
    {
        if (currentInteractable != null)
            ToggleOutline(currentInteractable, false);
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, interactionDistance))
        {
            IInteractable interactable = hit.collider.GetComponent<IInteractable>()
                                       ?? hit.collider.GetComponentInParent<IInteractable>();

            if (interactable != null)
            {
                currentInteractable = interactable;
                string interactionText = interactable.getInteractionText();

                ToggleOutline(currentInteractable, true);
                if (!string.IsNullOrEmpty(interactionText))
                {
                    interactionTextUI.text = interactionText;
                    interactionPanel.SetActive(true);
                }
                else
                {
                    interactionPanel.SetActive(false);
                }

                return;
            }
        }

        if (currentInteractable != null)
        {
            ToggleOutline(currentInteractable, false);
            currentInteractable = null;
        }
        interactionPanel.SetActive(false);
    }

    void HandleInteractionInput()
    {
        if (currentInteractable != null)
        {
            currentInteractable.Interact();
        }
    }

    void TogglePauseMenu()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0f : 1f;
        if (isPaused)
        {
            pauseMenuUI.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            _actions.Player.Look.Disable(); // Freeze mouse movement
            toastPanel?.SetActive(false); // Hide toast panel if it exists
        }
        else
        {
            pauseMenuUI.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            _actions.Player.Look.Enable();
        }
    }
}