using UnityEngine;

public abstract class ItemStack : MonoBehaviour
{
    public new string name { get; set; }
    public int amount { get; set; }
    public int maxStackSize { get; set; }
    public float xp { get; set; }
    public bool dropped { get; set; }
    public ItemType type { get; set; }

    public virtual void PickUp(PlayerExperience experience, PlayerInventory inventory)
    {
        addToInv(type, inventory);
        if (!dropped)
            experience.addXp(xp);

        Destroy(gameObject);
    }

    public void addToInv(ItemType itemType, PlayerInventory inventory)
    {
        if (inventory.invMap.ContainsKey(itemType))
        {
            int valuedPreviouslyMapped;
            inventory.invMap.TryGetValue(itemType, out valuedPreviouslyMapped);
            inventory.invMap.Remove(itemType);
            inventory.invMap.Add(itemType, valuedPreviouslyMapped + 1);
        }
        else
        {
            inventory.invMap.Add(itemType, 1);
        }
    }

    public enum ItemType
    {
        // ActionItems
        Axe,
        Glock,

        // EquippableItems


        // Resources
        SmallRock,
        WoodStick,
        WoodLog,

        // Other
        Null
    }
}
