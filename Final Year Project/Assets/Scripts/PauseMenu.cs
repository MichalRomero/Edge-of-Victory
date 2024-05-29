using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro; // Import TextMeshPro namespace

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public bool isPaused;

    public Slider sensitivitySlider;
    [SerializeField] private TextMeshProUGUI sensitivityValueText;  // Use TextMeshProUGUI

    private PlayerLook playerLook;

    // Start is called before the first frame update
    void Start()
    {
        if (pauseMenu != null)
        {
            pauseMenu.SetActive(false);
        }

        // Find the PlayerLook component
        playerLook = FindObjectOfType<PlayerLook>();

        if (playerLook != null)
        {
            // Initialize slider with current sensitivity setting
            sensitivitySlider.value = playerLook.sensitivity;

            // Update the text with the current sensitivity
            UpdateSensitivityText(playerLook.sensitivity);
        }

        // Add listener for sensitivity slider
        sensitivitySlider.onValueChanged.AddListener(SetSensitivity);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        if (pauseMenu != null)
        {
            pauseMenu.SetActive(true);
        }
        Time.timeScale = 0f;
        isPaused = true;

        // Show the cursor and unlock it
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void ResumeGame()
    {
        if (pauseMenu != null)
        {
            pauseMenu.SetActive(false);
        }
        Time.timeScale = 1f;
        isPaused = false;

        // Hide the cursor and lock it
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0); // Loads main menu
    }

    public void Retry()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(2); // Loads game
    }

    public void SetSensitivity(float value)
    {
        if (playerLook != null)
        {
            playerLook.SetSensitivity(value);
        }

        // Update the text with the new sensitivity value
        UpdateSensitivityText(value);
    }

    private void UpdateSensitivityText(float value)
    {
        if (sensitivityValueText != null)
        {
            sensitivityValueText.text = value.ToString("F2"); // Display the value with 2 decimal places
        }
    }

    private void OnDestroy()
    {
        // Remove listener to avoid memory leaks
        sensitivitySlider.onValueChanged.RemoveListener(SetSensitivity);
    }
}
