using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Tooltip("The max health value of the player.")]
    public float maxHealth = 100f;

    [Tooltip("The current health value of the player.")]
    public float currentHealth = 50f;

    [SerializeField]
    [Tooltip("The health regeneration rate of the player.")]
    private float healthRegenerationRate = 1f;

    [SerializeField]
    [Tooltip("The health bar component of the player's UI.")]
    private HealthBar healthBar;

    private void Update()
    {
        regenerateHealth();

        // Update the UI
        healthBar.SetHealth(currentHealth);
    }

    private void regenerateHealth()
    {
        if (currentHealth < maxHealth)
        {
            currentHealth += healthRegenerationRate * Time.deltaTime;
            if (currentHealth > maxHealth)
                currentHealth = maxHealth;
        }
    }
}
