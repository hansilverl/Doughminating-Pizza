using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkingSpeed = 7f;
    public float runningSpeed = 11f;
    public float jumpForce = 8f;
    public float groundCheckDistance = 1.1f;

    [Header("Camera Settings")]
    public Camera playerCamera;
    public float mouseSensitivityX = 15f;
    public float mouseSensitivityY = 15f;
    public float minY = -60f;
    public float maxY = 60f;
    public bool invertY = false;


    private float rotationY = 0f;
    private Rigidbody rb;
    private bool isGrounded;

    [HideInInspector]
    public bool canMove = true;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; // Prevent physics from rotating the player
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (canMove)
        {
            HandleMouseLook();
        }

        // Jump input is handled in Update for better responsiveness
        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
        }
    }

    void FixedUpdate()
    {
        if (!canMove) return;

        // Using Input.GetAxisRaw for more immediate response
        float inputX = Input.GetAxisRaw("Horizontal");
        float inputZ = Input.GetAxisRaw("Vertical");

        Vector3 cameraForward = playerCamera.transform.forward;
        Vector3 cameraRight = playerCamera.transform.right;

        // Flatten the vectors to avoid moving up/down
        cameraForward.y = 0f;
        cameraRight.y = 0f;
        cameraForward.Normalize();
        cameraRight.Normalize();

        Vector3 moveDirection = (cameraForward * inputZ + cameraRight * inputX).normalized;

        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float speed = isRunning ? runningSpeed : walkingSpeed;

        Vector3 targetVelocity = moveDirection * speed;
        Vector3 velocityChange = targetVelocity - new Vector3(rb.velocity.x, 0, rb.velocity.z);

        if(speed != 0)
        {
            rb.AddForce(velocityChange, ForceMode.VelocityChange);
            // Move the item in the player's hand up and down
            PlayerHand playerHand = GetComponentInChildren<PlayerHand>();
            if (playerHand != null)
            {
                playerHand.MoveItemUpDown();
            }
        }

        // rb.AddForce(velocityChange, ForceMode.VelocityChange);
    }

    void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivityX * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivityY * Time.deltaTime;


        
        // Rotate body on horizontal axis
        transform.Rotate(Vector3.up * mouseX);

        // Adjust vertical camera rotation
        rotationY += (invertY ? mouseY : -mouseY);
        rotationY = Mathf.Clamp(rotationY, minY, maxY);

        playerCamera.transform.localRotation = Quaternion.Euler(rotationY, 0f, 0f);


        // // Horizontal rotation (Y-axis)
        // transform.Rotate(0, mouseX, 0);

        // // Vertical rotation (X-axis)
        // rotationY += mouseY;
        // rotationY = Mathf.Clamp(rotationY, minY, maxY);

        // // Apply vertical rotation to the camera only
        // playerCamera.transform.localRotation = Quaternion.Euler(-rotationY, 0, 0);
    }

    bool IsGrounded()
    {
        Vector3 origin = transform.position + Vector3.up * 0.1f; // raise it slightly above the base
        return Physics.Raycast(origin, Vector3.down, groundCheckDistance);
    }
}
