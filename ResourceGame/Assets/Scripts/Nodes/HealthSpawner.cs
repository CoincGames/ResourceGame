using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSpawner : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The type of object to spawn at this node.")]
    private GameObject toSpawn = null;

    [SerializeField]
    [Tooltip("The time inbetween each spawn in seconds.")]
    private float spawnRateInSeconds = 0;

    [Tooltip("Keeps track of if the node has a currently spawned entity on it.")]
    public bool isSpawned;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnRegularly());
    }

    private IEnumerator SpawnRegularly()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnRateInSeconds);

            if (!isSpawned)
            {
                GameObject instantiated = Instantiate(toSpawn, transform.position + new Vector3(0, 1, 0), transform.rotation) as GameObject;
                instantiated.transform.Rotate(-90, 0, 0);

                instantiated.GetComponent<PickupHealth>().nodeSpawnerScript = this;
                isSpawned = true;
            }
        }
    }
}
