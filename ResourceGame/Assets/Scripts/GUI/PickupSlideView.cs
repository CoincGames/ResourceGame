using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PickupSlideView : MonoBehaviour
{
    public Text slideText;
    public Animation slideAnimation;

    private Resource.ResourceType lastType;
    private bool recentPickup = false;
    private int count = 0;

    public void displayWithResource(Resource rss)
    {
        if (gameObject.activeSelf)
        {
            if (lastType == rss.type)
            {
                recentPickup = true;
                count++;
                updateTextDisplay(rss);
            } else
            {
                // put in queue and display after
            }
        } else
        {
            gameObject.SetActive(true);
            recentPickup = true;
            lastType = rss.type;
            count++;
            updateTextDisplay(rss);
        }
    }

    private void updateTextDisplay(Resource rss)
    {
        slideText.text = "+ " + rss.name + " x" + count;
    }

    public void pause()
    {
        foreach (AnimationState item in slideAnimation)
        {
            item.speed = 0f;
        }
        StartCoroutine(waitToClose());
    }

    private IEnumerator waitToClose()
    {
        while (recentPickup)
        {
            recentPickup = false;
            yield return new WaitForSeconds(2f);
        }
        finishClosing();
    }

    public void finishClosing()
    {
        foreach (AnimationState item in slideAnimation)
        {
            item.speed = 1f;
        }
    }

    // Called at the end of the animation
    public void resetView()
    {
        count = 0;
        gameObject.SetActive(false);
    }
}
