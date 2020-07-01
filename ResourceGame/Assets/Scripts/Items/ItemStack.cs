using UnityEngine;

public class ItemStack : MonoBehaviour
{
    public static int DEFAULT_MAX_STACKSIZE = 100;

    public new string name { get; set; }
    public int amount { get; set; }
    public float xp { get; set; }
    public bool dropped { get; set; }
    public ItemType type { get; set; }

    protected ItemStack(string name, int amount, float xp, bool dropped, ItemType type)
    {
        this.name = name;
        this.amount = amount;
        this.xp = xp;
        this.dropped = dropped;
        this.type = type;
    }

    public virtual void PickUp(PlayerExperience experience, PlayerInventory inventory)
    {
        addToInv(inventory);

        if (!dropped)
            experience.addXp(xp);

        Destroy(gameObject);
    }

    public void addToInv(PlayerInventory playerInventory)
    {
        // Try to add and stack to the existing players item stacks
        ItemStack leftOver = playerInventory.inventory.TryAddItem(this);

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


        // Placeable Items
        Shield,

        // Resources
        SmallRock,
        WoodStick,
        WoodLog,

        // Other
        Null
    }
}
