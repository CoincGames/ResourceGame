using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PickupSlideView : MonoBehaviour
{
    public Text slideText;
    public Animation slideAnimation;

    public void displayWithResource(Resource rss)
    {
        gameObject.SetActive(true);
        slideText.text = "+ " + rss.name + " x1";
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
        yield return new WaitForSeconds(2f);
        finshClosing();
    }

    public void finshClosing()
    {
        foreach (AnimationState item in slideAnimation)
        {
            item.speed = 1f;
        }
    }

    public void resetView()
    {
        gameObject.SetActive(false);
    }
}
