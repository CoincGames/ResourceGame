using UnityEngine;

public class Resource : MonoBehaviour
{
    public new string name { get; set; }
    public float xp { get; set; }
    public ResourceType type { get; set; }

    public virtual void PickUp(PlayerExperience experience, PlayerInventory inventory)
    {
        addToInv(type, inventory);
        experience.addXp(xp);

        Destroy(gameObject);
    }

    public void addToInv(ResourceType resourceType, PlayerInventory inventory)
    {
        if (inventory.invMap.ContainsKey(resourceType))
        {
            int valuedPreviouslyMapped;
            inventory.invMap.TryGetValue(resourceType, out valuedPreviouslyMapped);
            inventory.invMap.Remove(resourceType);
            inventory.invMap.Add(resourceType, valuedPreviouslyMapped + 1);
        }
        else
        {
            inventory.invMap.Add(resourceType, 1);
        }
    }

    public enum ResourceType
    {
        SmallRock,
        WoodStick,
        WoodLog
    }
}
