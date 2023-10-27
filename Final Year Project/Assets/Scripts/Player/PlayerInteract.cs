using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    private Camera cam; // The player's camera component.
    [SerializeField]
    private float distance = 3f; // The max distance from the player within which interactions are possible.
    [SerializeField]
    private LayerMask mask; // Layer mask to filter objects the raycast can hit.
    private PlayerUI playerUI; // Reference to the PlayerUI to display prompts.
    private InputManager inputManager; // Input manager handling player inputs.

    // Initialization
    void Start()
    {
        cam = GetComponent<PlayerLook>().cam; // Get the Camera component from the PlayerLook script attached to the player.
        playerUI = GetComponent<PlayerUI>(); // Get the PlayerUI component attached to the player.
        inputManager = GetComponent<InputManager>(); // Get the InputManager component attached to the player.
    }

    // Update is called once per frame
    void Update()
    {
        // Clear any existing text from the player's UI.
        playerUI.UpdateText(string.Empty);

        // Create a ray originating from the camera and moving forward.
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        
        // Debbug to show how long the ray is.
        Debug.DrawRay(ray.origin, ray.direction * distance);

        RaycastHit hitInfo; //Variable Declaration to store our collision information.
        if (Physics.Raycast(ray, out hitInfo, distance, mask)) // Check if it hits anything within the specified distance and layer mask.
        {
            if (hitInfo.collider.GetComponent<Interactable>() != null)
            {
                Interactable interactable = hitInfo.collider.GetComponent<Interactable>();
                playerUI.UpdateText(interactable.promptMessage); // Update the UI text
                if (inputManager.onFoot.Interact.triggered) // If the interact button is pressed, call the interact method on the interactable.
                {
                    interactable.BaseInteract();
                }
            }
        }
    }
}
