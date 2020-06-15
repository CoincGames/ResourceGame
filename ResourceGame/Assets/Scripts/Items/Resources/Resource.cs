using UnityEngine;

public class Resource : Item
{
    public Resource()
    {
        amount = 1;
        maxStackSize = 100;
        dropped = false;
    }
}
