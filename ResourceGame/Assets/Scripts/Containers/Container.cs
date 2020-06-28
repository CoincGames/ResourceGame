﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Container : IEnumerable<ItemStack>
{
    private int maxSize = 0;
    private Vector2 location = new Vector2(0, 0);
    private Dictionary<int, ItemStack> inventoryMap = new Dictionary<int, ItemStack>();

    public Container(int maxSize, Vector2 location)
    {
        this.maxSize = maxSize;
        this.location = location;
    }

    public Vector2 GetLocation()
    {
        return this.location;
    }

    public bool TryAddItem(ItemStack item)
    {
        if (!CanAddItem())
            return false; // TODO send a thing back, let them know inventory is full

        inventoryMap.Add(inventoryMap.Count, item);

        refresh();
        return true;
    }

    public bool CanAddItem()
    {
        // TODO more complex logic for itemstacking instead of creating a new inventory slot
        return inventoryMap.Count >= getMaxSize();
    }

    public bool TryRemoveItem(ItemStack item)
    {
        int removedKey = -1;
        foreach (int key in inventoryMap.Keys)
        {
            ItemStack foundItem;
            inventoryMap.TryGetValue(key, out foundItem);
            if (foundItem == item)
                removedKey = key;
        }

        if (removedKey < 0)
            return false;

        inventoryMap.Remove(removedKey);
        return true;
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
