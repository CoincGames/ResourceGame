public class SmallRock : Resource
{
    public SmallRock()
    {
        name = "Small Rock";
        xp = 1.5f;
    }

    public override void PickUp(PlayerExperience experience, PlayerInventory inventory)
    {
        addToInv(ResourceType.SmallRock, inventory);

        base.PickUp(experience, inventory);
    }
}
