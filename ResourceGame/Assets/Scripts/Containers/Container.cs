using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Container : IEnumerable<ItemStack>
{
    private int size = 0;
    private List<ItemStack> containedItems = new List<ItemStack>();

    public Container(int size)
    {
        this.size = size;
    }

    public int getSize()
    {
        return size;
    }

    public IEnumerator GetEnumerator()
    {
        for (int index = 0; index < containedItems.Count; index++)
        {
            yield return containedItems[index];
        }
    }

    IEnumerator<ItemStack> IEnumerable<ItemStack>.GetEnumerator()
    {
        for (int index = 0; index < containedItems.Count; index++)
        {
            yield return containedItems[index];
        }
    }
}
