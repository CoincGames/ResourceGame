using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceNotif : Notification
{
    public Resource.ItemType itemType { get; set; }
    public int count { get; set; }

    public ResourceNotif(Resource.ItemType itemType, int count)
    {
        this.itemType = itemType;
        this.count = count;
    }

    public override string getMessage()
    {
        return "+ " + itemType + " x" + count;
    }
}
