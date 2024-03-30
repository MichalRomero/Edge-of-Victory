using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
using UnityEngine.Windows;

public class InputManager : MonoBehaviour
{
    private PlayerInput playerInput;
    public PlayerInput.OnFootActions onFoot;
    PlayerInput.MainActions input;

    private PlayerMotor motor;
    private PlayerLook look;

    void Awake()
    {
        playerInput = new PlayerInput();
        onFoot = playerInput.OnFoot;
        input = playerInput.Main; 

        motor = GetComponent<PlayerMotor>();
        look = GetComponent<PlayerLook>();


        onFoot.Jump.performed += ctx => motor.Jump(); // Subscribes jump function to jump action, anytime onFoot. Jump is performed, use ctx to call motor.Jump() function.
        onFoot.Crouch.performed += ctx => motor.Crouch();
        onFoot.Sprint.performed += ctx => motor.Sprint();

        input.Attack.started += ctx => motor.Attack(); // Similar to above but does not require player to be on foot.
        input.Block.started += ctx => motor.Block();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
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
        // Enable the action map.
        onFoot.Enable(); 
        input.Enable();
    }

    private void OnDisable()
    {
        // Disable the action map.
        onFoot.Disable(); 
        input.Disable();
    }

}