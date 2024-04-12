using System.Collections.Generic;
using UnityEngine;

public class Button : Interactable
{
    [SerializeField]
    private GameObject door;
    private bool doorOpen;

    [SerializeField]
    private List<GameObject> requiredDestroyedObjects;

    // Triggered when the button is interacted
    protected override void Interact()
    {
        // Check if all bots within the level are destroyed
        if (AreRequiredObjectsDestroyed())
        {
            doorOpen = !doorOpen;
            door.GetComponent<Animator>().SetBool("IsOpen", doorOpen);
        }
        else
        {
            Debug.Log("Cannot interact yet, objects still remaining.");
        }
    }

    // Checks if all objects in requiredDestroyedObjects list are null, meaning they have been destroyed
    private bool AreRequiredObjectsDestroyed()
    {
        foreach (GameObject obj in requiredDestroyedObjects)
        {
            if (obj != null) // Object still exists
                return false;
        }
        return true;
    }
}
