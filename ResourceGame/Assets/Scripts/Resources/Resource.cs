using UnityEngine;

public class Resource : MonoBehaviour
{
    public new string name { get; set; }
    public float xp { get; set; }

    public void PickUp(PlayerExperience experience)
    {
        experience.addXp(xp);

        Destroy(gameObject);
    }
}
