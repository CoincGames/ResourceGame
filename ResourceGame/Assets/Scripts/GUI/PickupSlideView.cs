using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickupSlideView : MonoBehaviour
{
    public Text slideText;
    public Animation slideAnimation;

    private Queue<Notification> hudQueue = new Queue<Notification>();

    public void display(Notification notif)
    {
        if (gameObject.activeSelf)
        {
            // A notif is already occurring

            if (notif.GetType() == typeof(TextNotif))
            {
                hudQueue.Enqueue(notif);
                return;
            }

            if (notif.GetType() == typeof(ResourceNotif))
            {
                ResourceNotif rssNotif = (ResourceNotif) notif;
                foreach (Notification notification in hudQueue)
                {
                    if (notification.GetType() == typeof(ResourceNotif))
                    {
                        ((ResourceNotif)notification).count += rssNotif.count;
                        return;
                    }
                }

                // If we got here, this resource entry type does not exist...
                hudQueue.Enqueue(notif);

                return;
            }
        } else
        {
            // No notif is alive so display this one!

            gameObject.SetActive(true);
            updateTextDisplay(notif.getMessage());
        }
    }

    private void updateTextDisplay(string message)
    {
        slideText.text = message;
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
        bool waiting = true;
        while (waiting)
        {
            waiting = false;
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

    // Called at the end of the animation by the animation frame
    public void resetView()
    {
        gameObject.SetActive(false);

        if (hudQueue.Count > 0)
        {
            display(hudQueue.Dequeue());
        }
    }
}
