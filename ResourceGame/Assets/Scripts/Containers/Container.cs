using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Container : IEnumerable<ItemStack>
{
    private bool isPlayer = false;
    private GameObject player;
    private Vector3 location = new Vector3(0,0,0);

    private int maxSize = 0;
    private Dictionary<int, ItemStack> inventoryMap = new Dictionary<int, ItemStack>();

    public Container(int maxSize, bool isPlayer, GameObject player, Vector3 location)
    {
        this.maxSize = maxSize;
        this.isPlayer = isPlayer;
        this.player = player;
        this.location = location;
    }

    public Container(int maxSize, bool isPlayer, GameObject player)
    {
        this.maxSize = maxSize;
        this.isPlayer = isPlayer;
        this.player = player;
    }

    public Container(int maxSize, Vector3 location)
    {
        this.maxSize = maxSize;
        this.location = location;
    }

    public int GetStackableSlotForItemType(ItemStack.ItemType itemType)
    {
        foreach (int key in inventoryMap.Keys)
        {
            ItemStack stackAtKey;
            inventoryMap.TryGetValue(key, out stackAtKey);

            int maxStackSize = (stackAtKey.GetType() == typeof(ItemStack)) ? ItemStack.DEFAULT_MAX_STACKSIZE : DurabilityItem.DURABILITY_MAX_STACKSIZE;

            if (stackAtKey != null && stackAtKey.type == itemType && stackAtKey.amount < maxStackSize)
                return key;
        }

        return -1;
    }

    public bool ContainsItemType(ItemStack.ItemType itemType)
    {
        foreach (int key in inventoryMap.Keys)
        {
            ItemStack stackAtKey;
            inventoryMap.TryGetValue(key, out stackAtKey);

            if (stackAtKey != null && stackAtKey.type == itemType)
                return true;
        }

        return false;
    }

    public Vector3 GetLocation()
    {
        return isPlayer ? player.transform.position : location;
    }

    public ItemStack TryAddItem(ItemStack item)
    {
        if (!CanAddItem())
            return item;

        // TODO should really check for similar item types in the inventory before creating a new extry. 

        inventoryMap.Add(inventoryMap.Count, item);

        refresh();
        return null;
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
