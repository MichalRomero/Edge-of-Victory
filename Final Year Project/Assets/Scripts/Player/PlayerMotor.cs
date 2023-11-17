using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool isGrounded;
    private float baseSpeed = 5f; // Base walking speed
    private float crouchSpeed = 3f; // Reduced speed when crouching
    private float sprintSpeed = 8f; // Increased speed when sprinting
    private float gravity = -20f; // Gravitational force
    private float jumpHeight = 2f; // Height the player can jump
    private bool lerpCrouch; // Flag for crouch transition
    private bool crouching; // Is player crouching?
    private bool sprinting; // Is player sprinting?
    private float crouchTimer; // Timer for crouch transition

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        isGrounded = controller.isGrounded;
        HandleCrouch();
    }

    public void ProcessMove(Vector2 input)
    {
        // Adjust speed based on player state
        float currentSpeed = sprinting ? sprintSpeed : (crouching ? crouchSpeed : baseSpeed);
        Vector3 moveDirection = transform.TransformDirection(new Vector3(input.x, 0, input.y)) * currentSpeed;

        // Handle gravity
        if (isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = -2f;
        }
        else
        {
            playerVelocity.y += gravity * Time.deltaTime;
        }

        // Apply movement and gravity
        controller.Move((moveDirection + playerVelocity) * Time.deltaTime);
    }

    public void Jump()
    {
        if (isGrounded && !crouching) // Prevent jumping while crouching
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    private void HandleCrouch()
    {
        if (!lerpCrouch) return;

        crouchTimer += Time.deltaTime;
        float targetHeight = crouching ? 1f : 2f;
        controller.height = Mathf.Lerp(controller.height, targetHeight, crouchTimer);

        if (crouchTimer >= 1)
        {
            lerpCrouch = false;
            crouchTimer = 0f;
        }
    }

    public void Crouch()
    {
        // If sprinting, switch to crouch
        if (sprinting)
        {
            sprinting = false;
            crouching = true;
        }
        else
        {
            crouching = !crouching;
        }

        crouchTimer = 0;
        lerpCrouch = true;
    }

    public void Sprint()
    {
        // If crouching, switch to sprint
        if (crouching)
        {
            crouching = false;
            sprinting = true;
        }
        else
        {
            sprinting = !sprinting;
        }
    }
}
