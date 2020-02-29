using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The slider fill bar to appear as the health bar.")]
    private Slider healthFill;

    [SerializeField]
    [Tooltip("The text component of the health bar that displays the current health over the max health.")]
    private Text healthText;

    public void setMaxHealth(float maxHealth)
    {
        healthFill.maxValue = maxHealth;
        UpdateUI();
    }

    public void SetHealth(float value)
    {
        healthFill.value = value;
        UpdateUI();
    }

    public void UpdateUI()
    {
        healthText.text = Mathf.RoundToInt(healthFill.value) + " / " + healthFill.maxValue;
    }
}
