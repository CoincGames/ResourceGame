using UnityEngine;

public class Gun : ActionItem
{
    public float damage { get; set; }
    public int range { get; set; }
    public int magSize { get; set; }
    public int ammoInMag { get; set; }

    public Gun(string name, ItemType itemType, float damage, int range, int magSize, int ammoInMag) : base(name, 5, false, itemType, 250, 250)
    {
        this.damage = damage;
        this.range = range;
        this.magSize = magSize;
        this.ammoInMag = ammoInMag;
    }

    public override void Action1()
    {
        base.Action1();

        ammoInMag--;
    }

    public override void Action2()
    {
        base.Action2();


    }
}
