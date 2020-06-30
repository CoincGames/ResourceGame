using UnityEngine;

public class ItemStack : MonoBehaviour
{
    public new string name { get; set; }
    public int amount { get; set; }
    public int maxStackSize { get; set; }
    public float xp { get; set; }
    public bool dropped { get; set; }
    public ItemType type { get; set; }

    protected ItemStack(string name, int amount, int maxStackSize, float xp, bool dropped, ItemType type)
    {
        this.name = name;
        this.amount = amount;
        this.maxStackSize = maxStackSize;
        this.xp = xp;
        this.dropped = dropped;
        this.type = type;
    }

    public virtual void PickUp(PlayerExperience experience, PlayerInventory inventory)
    {
        addToInv(type, inventory);
        if (!dropped)
            experience.addXp(xp);

        Destroy(gameObject);
    }

    public void addToInv(ItemType itemType, PlayerInventory playerInventory)
    {
        // Try to add and stack to the existing players item stacks
        ItemStack leftOver = playerInventory.inventory.TryAddItem(new ItemStack("Test_Item", 5, 100, 1, false, ItemType.SmallRock));

        if (leftOver != null)
        {
            // TODO Player inventory is full.  Spawn whatever is left near the player on the ground.

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
