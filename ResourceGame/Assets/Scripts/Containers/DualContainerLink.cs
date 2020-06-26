using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DualContainerLink
{
    private Container leftContainer { get; set; }
    private Container rightContainer { get; set; }

    public DualContainerLink(Container leftContainer, Container rightContainer)
    {
        this.leftContainer = leftContainer;
        this.rightContainer = rightContainer;
    }

    public void transferFromContainerToContainer(Container containerFrom, Container containerTo, ItemStack itemStack)
    {
        if (!containerTo.CanAddItem())
        {
            // Send message to player about full inv
            return;
        }

        if (!containerFrom.TryRemoveItem(itemStack))
        {
            // Item wasnt found in the inventory being transferred from
            return;
        }

        containerTo.TryAddItem(itemStack);
    }
}
