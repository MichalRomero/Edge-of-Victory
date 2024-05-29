using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerMotor : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 playerVelocity;
    public Camera cam;

    private bool isGrounded;
    private bool lerpCrouch; // Flag for crouch transition
    private bool crouching;
    private bool sprinting;
    private float crouchTimer; // Timer for crouch transition

    // Movement speeds
    private float baseSpeed = 5f;
    private float crouchSpeed = 3f;
    private float sprintSpeed = 8f;

    // jumping parameters
    private float gravity = -20f;
    private float jumpHeight = 2f;

    // Blocking parameters
    private bool isBlocking = false; // Flag to track if the player is currently blocking
    private float blockDuration = 0.5f; // Duration of blocking 
    public bool IsBlocking => isBlocking; // Property to provide read-only access to isBlocking

    // Stamina parameters
    public float maxStamina = 100f;
    private float stamina;
    private float staminaRegenRate = 10f;
    private float staminaDrainRate = 20f;
    private bool canSprint = true;

    public float Stamina => stamina; // Public property to access stamina

    // Attack parameters
    public float attackDistance = 3f;
    public float attackDelay = 0.4f;
    public float attackSpeed = 1f;
    public int attackDamage = 10;
    public LayerMask attackLayer;

    bool attacking = false;
    bool readyToAttack = true;
    int attackCount;

    //Animation
    Animator animator;

    public const string Blocking = "Blocking";
    public const string IDLE = "Idle";
    public const string WALK = "Walk";
    public const string ATTACK1 = "Attack 1";
    public const string ATTACK2 = "Attack 2";

    string currentAnimationState;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        cam = GetComponent<PlayerLook>().cam;
        animator = GetComponentInChildren<Animator>(); ;
        stamina = maxStamina;
    }

    void Update()
    {
        isGrounded = controller.isGrounded;
        HandleCrouch();
        SetAnimations();
        HandleStamina();
    }

    public void ProcessMove(Vector2 input)
    {
        // Adjust speed based on player state
        float currentSpeed = sprinting && canSprint ? sprintSpeed : (crouching ? crouchSpeed : baseSpeed);

        // Converted 2D to 3D movement direction and scaled by current speed
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

    // Manages the crouch transition
    private void HandleCrouch()
    {
        if (!lerpCrouch) return; // Exit if not interpolating.

        crouchTimer += Time.deltaTime;
        float targetHeight = crouching ? 1f : 2f; // Set target height based on crouching state.
        controller.height = Mathf.Lerp(controller.height, targetHeight, crouchTimer); // Smoothly interpolate height.

        // Reset lerp and timer when the transition completes.
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

    private void HandleStamina()
    {
        bool isMoving = controller.velocity.magnitude > 0.1f; // Check if player is moving

        if (sprinting && canSprint && isMoving)
        {
            stamina -= staminaDrainRate * Time.deltaTime;
            if (stamina <= 0)
            {
                stamina = 0;
                canSprint = false;
                sprinting = false; // Automatically stop sprinting if stamina is depleted
            }
        }
        else
        {
            stamina += staminaRegenRate * Time.deltaTime;
            if (stamina > maxStamina)
            {
                stamina = maxStamina;
            }
            if (stamina > 10) // Allow sprinting again when stamina is sufficiently regenerated
            {
                canSprint = true;
            }

            // Stop sprinting if the player is not moving
            if (!isMoving)
            {
                sprinting = false;
            }
        }
    }

    public void Attack()
    {
        Debug.Log("attacked");
        if (!readyToAttack || attacking) return;

        readyToAttack = false;
        attacking = true;

        Invoke(nameof(ResetAttack), attackSpeed); // Schedules attack reset based on attack speed
        Invoke(nameof(AttackRaycast), attackDelay); // Schedules the attack's hit detection after a delay

        if (attackCount == 0)
        {
            ChangeAnimationState(ATTACK1);
            attackCount++;
        }
        else
        {
            ChangeAnimationState(ATTACK2);
            attackCount = 0;
        }
    }

    void ResetAttack()
    {
        attacking = false;
        readyToAttack = true;
    }

    void AttackRaycast()
    {
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit hit, attackDistance, attackLayer))
        {
            HitTarget(hit.point);

            // Check if the hit object has an Enemy component and deal damage
            if (hit.transform.TryGetComponent<Enemy>(out Enemy T))
            { T.TakeDamage(attackDamage); }
            Debug.Log("raycast");
        }
    }

    void HitTarget(Vector3 pos)
    {
        Debug.Log("hit target");
    }

    public void ChangeAnimationState(string newState)
    {
        Debug.Log("Changing animation state to: " + newState);
        // Stops animation for iterrupting itslef
        if (currentAnimationState == newState) return;

        currentAnimationState = newState;
        animator.CrossFadeInFixedTime(currentAnimationState, 0.2f);
    }

    void SetAnimations()
    {
        // If player is blocking, return and do not change the animation
        if (isBlocking) return;

        // Existing code for handling other animations
        if (!attacking)
        {
            if (playerVelocity.x == 0 && playerVelocity.z == 0)
            {
                ChangeAnimationState(IDLE);
            }
            else
            {
                ChangeAnimationState(WALK);
            }
        }
    }

    public void Block()
    {
        if (!isBlocking)
        {
            // Start blocking
            isBlocking = true;

            // Trigger block animation if available
            ChangeAnimationState(Blocking);

            // Invoke method to stop blocking after blockDuration
            Invoke(nameof(StopBlocking), blockDuration);
        }
    }

    private void StopBlocking()
    {
        // Stop blocking
        isBlocking = false;

        // If you have a block animation, return to idle
        ChangeAnimationState(IDLE);
    }
}
