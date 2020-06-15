using UnityEngine;

public class DurabilityItem : Item
{
    public int maxDurability { get; set; }
    public int durability { get; set; }

    public virtual void Break()
    {
        // Show message to screen showing that it broke
        Destroy(this);
    }
}
