using System.Collections.Generic;
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
            ItemFactory itemFactory = FindObjectOfType<ItemFactory>();
            itemFactory.CreateResourceAtLocation(leftOver.gameObject, playerInventory.inventoryOwner.transform.position, leftOver.amount, leftOver.xp, leftOver.dropped, leftOver.type);
        }
    }
}

public class ItemType
{
    // ActionItems
    public static readonly ItemType AXE = new ItemType("AXE");
    public static readonly ItemType GLOCK = new ItemType("GLOCK");

    // EquippableItems


    // Placeable Items
    public static readonly ItemType SHIELD = new ItemType("SHIELD");

    // Resources
    public static readonly ItemType SMALL_ROCK = new ItemType("SMALL_ROCK");
    public static readonly ItemType WOOD_LOG = new ItemType("WOOD_LOG");
    public static readonly ItemType WOOD_STICK = new ItemType("WOOD_STICK");

    // Other
    public static readonly ItemType NULL = new ItemType("NULL");

    public static IEnumerable<ItemType> Values
    {
        get
        {
            yield return AXE;
            yield return GLOCK;
            yield return SHIELD;
            yield return SMALL_ROCK;
            yield return WOOD_LOG;
            yield return WOOD_STICK;
            yield return NULL;
        }
    }


    // Instance variables
    public string ItemTypeName { get; private set; }

    ItemType(string itemTypeName) => (ItemTypeName) = (itemTypeName);
}