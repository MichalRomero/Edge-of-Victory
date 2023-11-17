using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    // Message displayed to player when looking at an interactable.
    public string pMessage;

    // This function will be called from the player.
    public void BaseInteract()
    {
        Interact();
    }

    protected virtual void Interact()
    {
        // Won't have any code written in this function.
        // This is a template function to be overridden by our subclasses.
    }
}
