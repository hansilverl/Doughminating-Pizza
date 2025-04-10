using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    // Movement speeds
    public float walkingSpeed = 7f;
    public float runningSpeed = 11f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;

    // Camera settings
    public Camera playerCamera;
    public float lookSpeed = 1.0f;
    public float lookXLimit = 45.0f;

    CharacterController characterController;
    Vector3 moveDirection = Vector3.zero;
    float rotationX = 0;

    [HideInInspector]
    public bool canMove = true;

    void Start()
    {
        characterController = GetComponent<CharacterController>();

        // Lock the cursor to the center of the screen and make it invisible
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // Get input for horizontal and vertical movement
        float inputX = Input.GetAxis("Horizontal");
        float inputZ = Input.GetAxis("Vertical");

        // Calculate the movement direction relative to the player's orientation
        Vector3 forward = transform.forward;
        Vector3 right = transform.right;
        Vector3 direction = (forward * inputZ + right * inputX).normalized;

        // Check if the player is running
        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float speed = isRunning ? runningSpeed : walkingSpeed;

        // Set horizontal movement speed
        moveDirection.x = direction.x * speed;
        moveDirection.z = direction.z * speed;

        // Handle jumping and gravity
        if (characterController.isGrounded)
        {
            // On the ground: allow jump or reset vertical speed
            moveDirection.y = Input.GetButton("Jump") ? jumpSpeed : 0f;
        }
        else
        {
            // Apply gravity when not grounded
            moveDirection.y -= gravity * Time.deltaTime;
        }

        // Move the player using CharacterController, multiplying by deltaTime for frame rate independence
        characterController.Move(moveDirection * Time.deltaTime);

        // Handle camera and player rotation if movement is allowed
        if (canMove)
        {
            HandleCameraLook();
        }
    }

    // Method to handle camera look and horizontal player rotation
    void HandleCameraLook()
    {
        // Update vertical rotation based on mouse movement
        rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
        // Clamp the vertical rotation to prevent over-rotation
        rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
        // Apply the vertical rotation to the camera's local rotation
        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
        // Rotate the player horizontally based on mouse movement
        transform.Rotate(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
    }
}
