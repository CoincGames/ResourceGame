using UnityEngine;

public class Resource : Item
{
    public Resource()
    {
        // Maybe have a way to do item stacks instead... very minecraft like
        amount = 1;
        maxStackSize = 100;
        dropped = false;
    }
}
