using System.Collections.Generic;
using UnityEngine;

public class Button : Interactable
{
    [SerializeField]
    private GameObject door;
    private bool doorOpen;

    [SerializeField]
    private List<GameObject> requiredDestroyedObjects;

    protected override void Interact()
    {
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
