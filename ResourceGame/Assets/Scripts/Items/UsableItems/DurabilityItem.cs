using UnityEngine;

public class DurabilityItem : ItemStack
{
    public int maxDurability { get; set; }
    public int durability { get; set; }

    public DurabilityItem(string name, int amount, int maxStackSize, float xp, bool dropped, ItemType type, int maxDurability, int durability) : base(name, amount, maxStackSize, xp, dropped, type)
    {
        this.maxDurability = maxDurability;
        this.durability = durability;
    }

    public virtual void Break()
    {
        // Show message to screen showing that it broke
        string breakMessage = "Your " + name + " has broke...";

        // TODO Remove the object
        // This is something that is mostly in the player inventory, so check if destroying actually kills it
        Destroy(this);
    }
}
