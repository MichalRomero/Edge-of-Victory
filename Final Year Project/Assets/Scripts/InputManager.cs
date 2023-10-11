using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private PlayerInput playerInput;
    private PlayerInput.OnFootActions onFoot;

    private PlayerMotor motor;
    private PlayerLook look;
    // Start is called before the first frame update
    void Awake()
    {
        playerInput = new PlayerInput();
        onFoot = playerInput.OnFoot;

        motor = GetComponent<PlayerMotor>();
        look = GetComponent<PlayerLook>();

        //subscribes jump fucntion to jump action, anytime onFoot.Jump is performed, use ctx to call motor.Jump() function
        onFoot.Jump.performed += ctx => motor.Jump();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Tell the playermotor to move using the value ofrom movement action.
        motor.ProcessMove(onFoot.Movement.ReadValue<Vector2>());
        
    }

    private void LateUpdate(){
        look.ProcessLook(onFoot.Look.ReadValue<Vector2>());
    }

    //Allows to enable action map
    private void OnEnable(){
        onFoot.Enable();
    }

    //Allows to disable action map
    private void OnDisable(){
        onFoot.Disable();
    }

}
