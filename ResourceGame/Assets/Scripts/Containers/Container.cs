using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Container : IEnumerable<ItemStack>
{
    private int size = 0;
    private Dictionary<int, ItemStack> inventoryMap = new Dictionary<int, ItemStack>();

    public Container(int size)
    {
        this.size = size;
    }

    public bool TryAddItem(ItemStack item)
    {
        // TODO New code here
        // Check max size
        // containedItems.Add(item);
        return true;
    }

    public int getSize()
    {
        return size;
    }

    public IEnumerator GetEnumerator()
    {
        for (int index = 0; index < size; index++)
        {
            yield return inventoryMap[index];
        }
    }

    IEnumerator<ItemStack> IEnumerable<ItemStack>.GetEnumerator()
    {
        for (int index = 0; index < size; index++)
        {
            yield return inventoryMap[index];
        }
    }
}
