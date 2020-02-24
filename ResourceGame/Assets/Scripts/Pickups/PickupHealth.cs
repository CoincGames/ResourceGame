using UnityEngine;

public class PickupHealth : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The Game Object which owns this script.")]
    private GameObject owner;

    [SerializeField]
    [Tooltip("The amount of health to give to a player.")]
    [Range(1f, 100f)]
    private float suppliedHealth = 25;

    [Tooltip("The node parent spawner of this object.\n\nNOTE: Can be left empty")]
    public HealthSpawner nodeSpawnerScript;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            Pickup(other);
    }

    private void Pickup(Collider player)
    {
        if (player.GetComponent<PlayerHealth>().currentHealth >= player.GetComponent<PlayerHealth>().maxHealth)
            return;

        if (nodeSpawnerScript != null)
        {
            nodeSpawnerScript.isSpawned = false;
        }

        player.GetComponent<PlayerHealth>().currentHealth += suppliedHealth;
        Destroy(owner);
    }
}
