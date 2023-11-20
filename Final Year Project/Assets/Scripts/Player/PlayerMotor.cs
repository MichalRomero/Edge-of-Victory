using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.Windows;


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
        cam = GetComponent<PlayerLook>().cam;
    }


    void Update()
    {
        isGrounded = controller.isGrounded;
        HandleCrouch();


        //if (input.Attack.IsPressed())
        //{ Attack(); }


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

    public float attackDistance = 3f;
    public float attackDelay = 0.4f;
    public float attackSpeed = 1f;
    public int attackDamage = 10;
    public LayerMask attackLayer;

    public GameObject hitEffect;
    public AudioClip swordSwing;
    public AudioClip hitSound;

    bool attacking = false;
    bool readyToAttack = true;
    int attackCount;

    public Camera cam;

    public void Attack()
    {
        Debug.Log("attacked");
        if (!readyToAttack || attacking) return;

        readyToAttack = false;
        attacking = true;

        Invoke(nameof(ResetAttack), attackSpeed);
        Invoke(nameof(AttackRaycast), attackDelay);

        if (attackCount == 0)
        {
            //ChangeAnimationState(ATTACK1);
            attackCount++;
        }
        else
        {
            //ChangeAnimationState(ATTACK2);
            attackCount = 0;
        }
  
    }

    void ResetAttack()
    {
        attacking = false;
        readyToAttack = true;
        //Debug.Log("resetAttack");
    }

    void AttackRaycast()
    {
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit hit, attackDistance, attackLayer))
        {
            HitTarget(hit.point);

            if (hit.transform.TryGetComponent<Enemy>(out Enemy T))
            { T.TakeDamage(attackDamage); }
            Debug.Log("raycast");
        }
    }

    void HitTarget(Vector3 pos)
    {
        //audioSource.pitch = 1;
        //audioSource.PlayOneShot(hitSound);

        //GameObject GO = Instantiate(hitEffect, pos, Quaternion.identity);
        //Destroy(GO, 20);

        Debug.Log("hit target");
    }

}