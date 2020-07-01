using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemStackNotif : Notification
{
    public ItemStack itemStack { get; set; }
    public int count { get; set; }

    public ItemStackNotif(ItemStack itemStack)
    {
        this.itemStack = itemStack;
        this.count = itemStack.amount;
    }

    public override string getMessage()
    {
        return "+ " + itemStack.name + " x" + count;
    }
}
