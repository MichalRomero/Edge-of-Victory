using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private PlayerInput playerInput;
    public PlayerInput.OnFootActions onFoot;

    private PlayerMotor motor;
    private PlayerLook look;

    void Awake()
    {
        playerInput = new PlayerInput();
        onFoot = playerInput.OnFoot;

        motor = GetComponent<PlayerMotor>();
        look = GetComponent<PlayerLook>();


        onFoot.Jump.performed += ctx => motor.Jump(); // Subscribes jump fucntion to jump action, anytime onFoot. Jump is performed, use ctx to call motor.Jump() function.
        onFoot.Crouch.performed += ctx => motor.Crouch();
        onFoot.Sprint.performed += ctx => motor.Sprint();
    }

    // Update is called once per frame.
    void FixedUpdate()
    {
        // Tell the playermotor to move using the value from movement action.
        motor.ProcessMove(onFoot.Movement.ReadValue<Vector2>());

    }

    private void LateUpdate()
    {
        look.ProcessLook(onFoot.Look.ReadValue<Vector2>()); // Send camera look input to the PlayerLook.
    }

    private void OnEnable()
    {
        onFoot.Enable(); // Enable the action map.
    }

    private void OnDisable()
    {
        onFoot.Disable(); // Disable the action map.
    }
    //TESTING
}