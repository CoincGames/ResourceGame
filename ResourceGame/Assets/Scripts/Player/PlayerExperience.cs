using UnityEngine;

public class PlayerExperience : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The current level of this player.")]
    public int currentLevel = 1;

    [SerializeField]
    [Tooltip("The current experience towards the next level for this player.")]
    public float currentExperience = 0;

    [SerializeField]
    [Tooltip("The XP bar to update the UI on when xp or levels change.")]
    public XPBar xpBar;

    private void Awake()
    {
        xpBar.setMaxXP(getNeededXP());
    }

    private void Update()
    {
        checkLevelUp();
        xpBar.SetXP(currentExperience);
    }

    public void addXp(float xp)
    {
        currentExperience += xp;
        checkLevelUp();
    }

    private void checkLevelUp()
    {
        while (currentExperience >= getNeededXP() && getNeededXP() > 0)
        {
            levelUp();
        }
    }

    public void levelUp()
    {
        currentExperience -= getNeededXP();
        currentLevel += 1;

        xpBar.setMaxXP(getNeededXP());
    }

    private long getNeededXP()
    {
        if (currentLevel >= Constants.maxLevel)
            return -1;

        float scalingFactor = Mathf.Log((Constants.xpNeededForFinalLevel / Mathf.Log(Constants.maxLevel, 2)) - 24) / Mathf.Log(Constants.maxLevel - 1);

        return System.Convert.ToInt64(Mathf.Log(currentLevel + 1, 2) * (Constants.xpNeededForFirstLevel - 1 + Mathf.Pow(currentLevel, scalingFactor)));
    }
}
