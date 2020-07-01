using UnityEngine;

public abstract class ActionItem : DurabilityItem
{
    public ActionItem(string name, int amount, float xp, bool dropped, ItemType type, int maxDurability, int durability) : base(name, amount, xp, dropped, type, maxDurability, durability)
    {

    }

    public ActionItem(string name, float xp, bool dropped, ItemType type, int maxDurability, int durability) : base(name, 1, xp, dropped, type, maxDurability, durability)
    {

    }

    public virtual void Action1() { }

    public virtual void Action2() { }
}
