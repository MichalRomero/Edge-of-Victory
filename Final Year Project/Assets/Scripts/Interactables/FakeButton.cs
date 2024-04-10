using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FakeButton : Interactable
{
    [SerializeField]
    private List<GameObject> requiredDestroyedObjects;

    protected override void Interact()
    {
        if (AreRequiredObjectsDestroyed())
        {
            // Save the game time before changing scenes
            Timer timer = FindObjectOfType<Timer>();
            if (timer != null)
            {
                timer.GameFinished();
            }
            else
            {
                Debug.LogError("Timer script not found in the scene.");
            }

            // Change the scene
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
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
