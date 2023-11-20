using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    public Camera cam; // The player's camera component.
    [SerializeField]
    private float dist = 3f; // The max distance from the player within which interactions are possible.
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
        // Clear any existing text from the player's UI in a chained method call.
        playerUI.TextUpdate(string.Empty);

        // Declare a RaycastHit variable at the beginning for storing collision information.
        RaycastHit hitInfo;

        // Create a ray originating from the camera and moving forward.
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);

        // Debug to show the ray length.
        Debug.DrawRay(ray.origin, ray.direction * dist);

        // Check if the ray hits anything within the specified distance and layer mask.
        if (Physics.Raycast(ray, out hitInfo, dist, mask) && hitInfo.collider.TryGetComponent(out Interactable interactable))
        {
            // Update the UI text with the message from the interactable object.
            playerUI.TextUpdate(interactable.pMessage);

            // Check if the interact button is pressed and call the interact method.
            if (inputManager.onFoot.Interact.triggered)
            {
                interactable.BaseInteract();
            }
        }

    }

}
