using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHP : MonoBehaviour
{
    private float health; // Player health
    private float maxHealth = 100f; // Player max health
    private float lerpTimer;
    private float delaySpeed = 4f; // Speed for health bar change
    public Image healthBar;
    public Image backHealthBar;

    private PlayerMotor playerMotor;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth; // Initialise health to max at start
        playerMotor = GetComponent<PlayerMotor>();
    }

    // Update is called once per frame
    void Update()
    {
        health = Mathf.Clamp(health, 0, maxHealth);// Ensure health stays within bounds
        UpdateHealth();

        // USED AS TESTING - Check for key presses to simulate damage or healing 
        if (Input.GetKeyDown(KeyCode.P))
        {
            TakeDamage(Random.Range(5, 10));
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            Heal(Random.Range(5, 10));
        }
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
            backHealthBar.color = Color.green; // Change color to magenta to indicate healing
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / delaySpeed; // Calculate interpolation progress
            percentComplete = percentComplete * percentComplete;
            healthBar.fillAmount = Mathf.Lerp(fill, backHealthBar.fillAmount, percentComplete);
        }
    }

    // Reduces player's health
    public void TakeDamage (float damage)
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
        }
    }

    // Increases player's health
    public void Heal(float healAmount)
    {
        health += healAmount;
        lerpTimer = 0f;
    }
}
