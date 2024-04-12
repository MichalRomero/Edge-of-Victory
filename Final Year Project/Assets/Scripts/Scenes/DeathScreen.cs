using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathScreen : MonoBehaviour
{
    public void RestartGame()
    {
        SceneManager.LoadScene(2); // Loads the game scene
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0); // Loads the main menu scene
    }
}
