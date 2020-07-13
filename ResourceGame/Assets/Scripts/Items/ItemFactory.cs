using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemFactory : MonoBehaviour
{
    [Header("Item Stack Dictionary")]
    public ItemStackEntry[] itemStackDictionary;

    [System.Serializable]
    public struct ItemStackEntry
    {
        public string name;
        public GameObject itemStack;

        public ItemStackEntry(string name, GameObject itemStack)
        {
            this.name = name;
            this.itemStack = itemStack;
        }
    }

    public ItemStackEntry GetResourceByName(string name)
    {
        foreach(ItemStackEntry itemStackEntry in itemStackDictionary)
        {
            if (string.Equals(itemStackEntry.name, name))
                return itemStackEntry;
        }

        return new ItemStackEntry(null, null);
    }

    public void CreateResourceAtLocation(GameObject resourceToCreate, Vector3 location, int stackSize, float xpAmount, bool dropped, ItemType itemType)
    {
        GameObject created = Instantiate(resourceToCreate, location, resourceToCreate.transform.rotation) as GameObject;

        ItemStack itemStack = created.GetComponent<ItemStack>();
        itemStack.name = "ENUM_NAME_HERE_FROM_TYPE";
        itemStack.amount = stackSize;
        itemStack.xp = xpAmount;
        itemStack.dropped = dropped;
        itemStack.type = itemType;
    }

    public void CreateResourceAtLocation(GameObject resourceToCreate, Vector3 location, ItemStack itemStack)
    {
        CreateResourceAtLocation(resourceToCreate, location, itemStack.amount, itemStack.xp, itemStack.dropped, itemStack.type);
    }

    // TODO ITEMSTACK ENUMS!
}
