using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathByFalling : MonoBehaviour
{
    public float deathYValue = -10f; // death value

    void Update()
    {
        // Checks if the player's Y position is less than the death value
        if (transform.position.y < deathYValue)
        {
            Die();
        }
    }

    void Die()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); // Loads the next scene (death screen)
    }
}
