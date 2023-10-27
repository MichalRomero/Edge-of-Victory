using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    private CharacterController controller; // Reference to the CharacterController component.
    private Vector3 playerVelocity;
    private bool isGrounded;
    public float speed = 5f; // Player movement speed.
    public float gravity = -9.8f; // Gravity force applied to the player.
    public float jumpHeight = 3f; // Jump height.
    private bool lerpCrouch;
    private bool crouching;
    private bool sprinting;
    private float crouchTimer;
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = controller.isGrounded;

        if (lerpCrouch)
        {
            crouchTimer += Time.deltaTime;
            float p = crouchTimer / 1;
            p *= p;
            if (crouching)
                controller.height = Mathf.Lerp(controller.height, 1, p);
            else
                controller.height = Mathf.Lerp(controller.height, 2, p);

            if (p > 1)
            {
                lerpCrouch = false;
                crouchTimer = 0f;
            }
            
        }
    }

    // Will recive inputs from InputManger.cs and apply them to our character controller.
    public void ProcessMove(Vector2 input){
        Vector3 moveDirection = Vector3.zero;
        moveDirection.x = input.x;

        // Grabs the y component of the 2D vector and applying it to the z axis of our vector(vertical movement - forward backward movement).
        moveDirection.z = input.y; // Apply input to the player's movement.
        
        controller.Move(transform.TransformDirection(moveDirection) * speed * Time.deltaTime);

        // Apply a constant downwards force to our player.
        playerVelocity.y += gravity * Time.deltaTime;
        if (isGrounded && playerVelocity.y < 0){
            playerVelocity.y = -2f;
        }
        controller.Move(playerVelocity * Time.deltaTime); // Apply gravity and ensure the player stays grounded.
    }

    public void Jump(){
        if (isGrounded){
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -3.0f * gravity); // Perform a jump.
        }
    }

    public void Crouch()
    {
        crouching = !crouching;
        crouchTimer = 0;
        lerpCrouch = true; // Toggle crouching and initiate lerping for height adjustment.
    }

    public void Sprint()
    {
        sprinting = !sprinting;
        if (sprinting)
            speed = 8; // Increase speed when sprinting.
        else
            speed = 5;
    }
}
