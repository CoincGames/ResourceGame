using System.Collections;
using UnityEngine;

public class Gun : ActionItem
{
    public float damage;
    public int range;
    public int magSize;
    public int ammoInMag;

    public bool reloading;

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

        if (ammoInMag > 0 && !reloading)
        {
            Debug.Log("Shoot");
            Shoot();
            Use();
        } else
        {
            Debug.Log("Click, Ammo = " + ammoInMag +"/" + magSize);
            // Start reload here
            StartCoroutine(Reload());
        }
    }

    public void Shoot()
    {
        // Animation
        // https://www.google.com/search?q=unity+doing+reload+animation&rlz=1C1CHBF_enUS879US879&oq=unity+doing+reload+animation&aqs=chrome..69i57j33.2789j0j7&sourceid=chrome&ie=UTF-8#kpvalbx=_dEMqX57_B4H--gSP64No19
        gameObject.transform.rotation = Quaternion.Slerp(gameObject.transform.rotation, gameObject.transform.rotation * Quaternion.Euler(new Vector3(-80f, 0f, 0f)), Time.deltaTime * 5f);

        // Spawn bullet or particle effect
        // https://www.reddit.com/r/Unity3D/comments/4hhvba/performance_particles_vs_objects_for_ammo/

        // Reduce ammo in mag
        ammoInMag--;
    }

    public IEnumerator Reload()
    {
        reloading = true;

        // Animation
        // https://youtu.be/kAx5g9V5bcM?t=348

        // Length of the reload animation
        yield return new WaitForSeconds(1.5f);

        // Set ammo back to max mag size at end of animation
        ammoInMag = magSize;
        reloading = false;
    }

    public void ADS()
    {
        base.Action2();


    }
}
