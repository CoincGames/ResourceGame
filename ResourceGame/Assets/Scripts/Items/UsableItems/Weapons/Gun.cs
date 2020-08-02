using UnityEngine;

public class Gun : ActionItem
{
    public float damage;
    public int range;
    public int magSize;
    public int ammoInMag;

    public Gun(string name, ItemType itemType, float damage, int range, int magSize, int ammoInMag) : base(name, 5, false, itemType, 250, 250)
    {
        this.damage = damage;
        this.range = range;
        this.magSize = magSize;
        this.ammoInMag = ammoInMag;
    }

    public override void Update()
    {
        if (Input.GetMouseButtonDown(0))
            Fire();

        if (Input.GetMouseButtonDown(1))
            ADS();
    }

    public void Fire()
    {
        base.Action1();

        if (ammoInMag > 0)
        {
            Debug.Log("Shoot");
            Shoot();
            Use();
        } else
        {
            Debug.Log("Click, Ammo = " + ammoInMag +"/" + magSize);
            // Start reload here
            Reload();
        }
        Use();
    }

    public void Shoot()
    {
        // Animation
        gameObject.transform.rotation = Quaternion.Slerp(gameObject.transform.rotation, gameObject.transform.rotation * Quaternion.Euler(new Vector3(-80f, 0f, 0f)), Time.deltaTime * 5f);

        // Spawn bullet or particle effect

        // Reduce ammo in mag
        ammoInMag--;
    }

    public void Reload()
    {
        // Animation

        // Set ammo back to max mag size at end of animation
        ammoInMag = magSize;
    }

    public void ADS()
    {
        base.Action2();


    }
}
