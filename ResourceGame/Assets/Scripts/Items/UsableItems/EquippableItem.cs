public abstract class EquippableItem : DurabilityItem
{
    public EquippableItem(string name, int amount, float xp, bool dropped, ItemType type, int maxDurability, int durability) : base(name, amount, xp, dropped, type, maxDurability, durability)
    {

    }

    public EquippableItem(string name, float xp, bool dropped, ItemType type, int maxDurability, int durability) : base(name, 1, xp, dropped, type, maxDurability, durability)
    {

    }

    public void onDefense()
    {

    }
}
