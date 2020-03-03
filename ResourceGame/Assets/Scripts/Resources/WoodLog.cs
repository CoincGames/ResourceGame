public class WoodLog : Resource
{
    public WoodLog()
    {
        name = "Wood Log";
        xp = 2.5f;
        type = ResourceType.WoodLog;
    }

    public override void PickUp(PlayerExperience experience, PlayerInventory inventory)
    {
        addToInv(ResourceType.WoodLog, inventory);

        base.PickUp(experience, inventory);
    }
}
