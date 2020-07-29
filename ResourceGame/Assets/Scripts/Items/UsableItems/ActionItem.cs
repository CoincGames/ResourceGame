using UnityEngine;

public abstract class ActionItem : DurabilityItem
{
    public ActionItem(string name, int amount, float xp, bool dropped, ItemType type, int maxDurability, int durability) : base(name, amount, xp, dropped, type, maxDurability, durability)
    {

    }

    public ActionItem(string name, float xp, bool dropped, ItemType type, int maxDurability, int durability) : base(name, 1, xp, dropped, type, maxDurability, durability)
    {

    }

    public virtual void Update()
    {
        Action1();
        Action2();
    }

    public virtual void Action1() 
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Left Click");
        }
    }

    public virtual void Action2()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log("Right Click");
        }
    }
}
