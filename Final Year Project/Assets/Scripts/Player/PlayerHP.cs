using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHP : MonoBehaviour
{
    private float health; // Player health
    private float maxHealth = 100f; // Player max health
    private float lerpTimer;
    private float delaySpeed = 4f; // Speed for health bar change
    public Image healthBar;
    public Image backHealthBar;

    private float stamina;
    private float maxStamina = 100f;
    public Image staminaBar;
    public Image backStaminaBar;

    private PlayerMotor playerMotor;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth; // Initialise health to max at start
        stamina = maxStamina;
        playerMotor = GetComponent<PlayerMotor>();
    }

    // Update is called once per frame
    void Update()
    {
        health = Mathf.Clamp(health, 0, maxHealth); // Ensure health stays within bounds
        stamina = playerMotor.Stamina; // Get current stamina from PlayerMotor
        UpdateHealth();
        UpdateStamina();

        // USED AS TESTING - Check for key presses to simulate damage or healing 
        //if (Input.GetKeyDown(KeyCode.P))
        //{
            //TakeDamage(Random.Range(5, 10));
        //}

        //if (Input.GetKeyDown(KeyCode.L))
        //{
            //Heal(Random.Range(5, 10));
        //
    }

    // Updates the health bar UI to reflect current health
    public void UpdateHealth()
    {
        Debug.Log(health);
        float fill = healthBar.fillAmount;
        float fillBack = backHealthBar.fillAmount;
        float healthFraction = health / maxHealth;

        // Math behind player getting damaged
        if (fillBack > healthFraction)
        {
            healthBar.fillAmount = healthFraction;
            backHealthBar.color = Color.red; // Change color to red to indicate damage
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / delaySpeed; // Calculate interpolation progress
            percentComplete = percentComplete * percentComplete;
            backHealthBar.fillAmount = Mathf.Lerp(fillBack, healthFraction, percentComplete);
        }

        // Math behind player getting healed
        if (fill < healthFraction)
        {
            backHealthBar.fillAmount = healthFraction;
            backHealthBar.color = Color.green; // Change color to green to indicate healing
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / delaySpeed; // Calculate interpolation progress
            percentComplete = percentComplete * percentComplete;
            healthBar.fillAmount = Mathf.Lerp(fill, backHealthBar.fillAmount, percentComplete);
        }
    }

    // Updates the stamina bar UI to reflect current stamina
    public void UpdateStamina()
    {
        float fill = staminaBar.fillAmount;
        float fillBack = backStaminaBar.fillAmount;
        float staminaFraction = stamina / maxStamina;

        // Math behind stamina drain
        if (fillBack > staminaFraction)
        {
            staminaBar.fillAmount = staminaFraction;
            backStaminaBar.color = Color.yellow; // Change color to yellow to indicate stamina drain
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / delaySpeed; // Calculate interpolation progress
            percentComplete = percentComplete * percentComplete;
            backStaminaBar.fillAmount = Mathf.Lerp(fillBack, staminaFraction, percentComplete);
        }

        // Math behind stamina regeneration
        if (fill < staminaFraction)
        {
            backStaminaBar.fillAmount = staminaFraction;
            backStaminaBar.color = Color.cyan; // Change color to cyan to indicate stamina regeneration
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / delaySpeed; // Calculate interpolation progress
            percentComplete = percentComplete * percentComplete;
            staminaBar.fillAmount = Mathf.Lerp(fill, backStaminaBar.fillAmount, percentComplete);
        }
    }

    // Reduces player's health
    public void TakeDamage(float damage)
    {
        // Check if the player is blocking
        if (playerMotor != null && playerMotor.IsBlocking)
        {
            Debug.Log("BLOCKED");
            // Player is blocking, so don't apply damage
            // Optionally, you can add a visual or audio effect here
        }
        else
        {
            // Player is not blocking, apply damage
            health -= damage;
            lerpTimer = 0f;

            if (health <= 0)
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
        }
    }

    // Increases player's health
    public void Heal(float healAmount)
    {
        health += healAmount;
        lerpTimer = 0f;
    }
}
