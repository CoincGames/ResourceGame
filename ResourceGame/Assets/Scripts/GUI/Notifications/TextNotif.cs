using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextNotif : Notification
{
    private string message;

    public TextNotif(string message)
    {
        this.message = message;
    }

    public override string getMessage()
    {
        return message;
    }
}
