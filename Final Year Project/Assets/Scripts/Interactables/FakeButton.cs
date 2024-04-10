using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FakeButton : Interactable
{
    protected override void Interact()
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
}
