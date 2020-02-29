using UnityEngine;

// TODO serialize and tooltips
public class PlayerExperience : MonoBehaviour
{
    public static int maxLevel = 100; // TODO Put in constants file
    public static int xpNeededForFirstLevel = 25;
    public static int xpNeededForFinalLevel = 10000;

    public int currentLevel = 1;

    public float currentExperience = 0;

    public XPBar xpBar;

    private void Awake()
    {
        xpBar.setMaxXP(getNeededXP());
    }

    private void Update()
    {
        currentExperience += Time.deltaTime * 5f; // TODO Temp

        checkLevelUp();
        xpBar.SetXP(currentExperience);
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
        if (currentLevel >= maxLevel)
            return -1;

        float scalingFactor = Mathf.Log((xpNeededForFinalLevel / Mathf.Log(maxLevel, 2)) - 24) / Mathf.Log(maxLevel - 1);

        return System.Convert.ToInt64(Mathf.Log(currentLevel + 1, 2) * (xpNeededForFirstLevel - 1 + Mathf.Pow(currentLevel, scalingFactor)));
    }
}
