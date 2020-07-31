using UnityEngine;

public class DurabilityItem : ItemStack
{
    public static int DURABILITY_MAX_STACKSIZE = 1;

    public int maxDurability;
    public int durability;

    public DurabilityItem(string name, int amount, float xp, bool dropped, ItemType type, int maxDurability, int durability) : base(name, amount, xp, dropped, type)
    {
        this.maxDurability = maxDurability;
        this.durability = durability;
    }

    public void Use()
    {
        durability--;

        if (durability <= 0)
            Break();
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
