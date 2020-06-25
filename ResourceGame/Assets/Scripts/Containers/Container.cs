using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Container : IEnumerable<ItemStack>
{
    private int maxSize = 0;
    private Dictionary<int, ItemStack> inventoryMap = new Dictionary<int, ItemStack>();

    public Container(int maxSize)
    {
        this.maxSize = maxSize;
    }

    public void TryAddItem(ItemStack item)
    {
        if (inventoryMap.Count >= getMaxSize())
            return; // TODO send a thing back, let them know inventory is full

        inventoryMap.Add(inventoryMap.Count, item);

        refresh();
    }

    public void SwapSlots(int slotFrom, int slotTo)
    {
        ItemStack fromItemStack;
        inventoryMap.TryGetValue(slotFrom, out fromItemStack);

        if (fromItemStack == null)
            return;

        ItemStack toItemStack;
        inventoryMap.TryGetValue(slotTo, out toItemStack);

        inventoryMap[slotTo] = fromItemStack;
        inventoryMap[slotFrom] = toItemStack;

        refresh();
    }

    public void refresh()
    {
        clearNulls();
    }

    public void clearNulls()
    {
        for(int i = 0; i < maxSize; i++)
        {
            Dictionary<int, ItemStack>.KeyCollection keys = inventoryMap.Keys;

            foreach(int key in keys)
            {
                ItemStack checkingItemStack;
                inventoryMap.TryGetValue(key, out checkingItemStack);

                if (checkingItemStack == null)
                    inventoryMap.Remove(key);
            }
        }
    }

    public int getSize()
    {
        return inventoryMap.Count;
    }

    public int getMaxSize()
    {
        return maxSize;
    }

    public IEnumerator GetEnumerator()
    {
        for (int index = 0; index < maxSize; index++)
        {
            yield return inventoryMap[index];
        }
    }

    IEnumerator<ItemStack> IEnumerable<ItemStack>.GetEnumerator()
    {
        for (int index = 0; index < maxSize; index++)
        {
            yield return inventoryMap[index];
        }
    }
}
