using UnityEngine;

public class DurabilityItem : Item
{
    public int maxDurability { get; set; }
    public int durability { get; set; }

    public virtual void Break()
    {
        Destroy(this);
    }
}
