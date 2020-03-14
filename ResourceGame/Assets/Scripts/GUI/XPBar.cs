using UnityEngine;
using UnityEngine.UI;

public class XPBar : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The slider fill bar to appear as the XP bar.")]
    private Slider xpFill = null;

    [SerializeField]
    [Tooltip("The text component of the XP bar that displays the current XP over the next level's needed XP.")]
    private Text xpText = null;

    public void setMaxXP(float maxXP)
    {
        xpFill.maxValue = maxXP;
        UpdateUI();
    }

    public void SetXP(float value)
    {
        xpFill.value = value;
        UpdateUI();
    }

    public void UpdateUI()
    {
        if (xpFill.maxValue == -1) // Max level has been reached
            xpText.text = "MAX LEVEL";
        else 
            xpText.text = Mathf.RoundToInt(xpFill.value) + " / " + xpFill.maxValue;
    }
}
