using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    public Camera cam; 
    [SerializeField]
    private float dist = 3f; // The max distance from the player within which interactions are possible.
    [SerializeField]
    private LayerMask mask; // Layer mask to filter objects the raycast can hit.
    private PlayerUI playerUI; 
    private InputManager inputManager; 

    // Initialization - Getting components
    void Start()
    {
        cam = GetComponent<PlayerLook>().cam; 
        playerUI = GetComponent<PlayerUI>(); 
        inputManager = GetComponent<InputManager>(); 

    }

    // Update is called once per frame
    void Update()
    {
        // Clear any existing text from the player's UI 
        playerUI.TextUpdate(string.Empty);
        RaycastHit hitInfo;

        // Create a ray from camera outwards
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);

        Debug.DrawRay(ray.origin, ray.direction * dist); // Debug to shown ray

        // Check if the ray hits anything within the specified distance and layer mask.
        if (Physics.Raycast(ray, out hitInfo, dist, mask) && hitInfo.collider.TryGetComponent(out Interactable interactable))
        {
            playerUI.TextUpdate(interactable.pMessage);

            // Check if the interact button is pressed and call the interact method.
            if (inputManager.onFoot.Interact.triggered)
            {
                interactable.BaseInteract();
            }
        }

    }

}
