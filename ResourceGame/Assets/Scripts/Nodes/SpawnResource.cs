using UnityEngine;

public class SpawnResource : MonoBehaviour
{
    [Tooltip("The spawn rate of these items.")]
    [Range(0f, 1f)]
    public float spawnRatePercentage;

    [Tooltip("The possible prefab resources to spawn.\n\nNOTE: Should be of type pickup")]
    public GameObject[] possibleSpawns;

    // Start is called before the first frame update
    void Start()
    {
        float willSpawn = Random.Range(0f, 1f);
        if (willSpawn < spawnRatePercentage)
        {
            float whichToSpawn = Random.Range(0f, 1f);

            for (int i = 0; i < possibleSpawns.Length; i++)
            {
                if (whichToSpawn < (1f / possibleSpawns.Length) * (i + 1))
                {

                    Instantiate(possibleSpawns[i], transform.position + new Vector3(0, 1, 0), transform.rotation * Quaternion.Euler(Random.Range(0, 360f), Random.Range(0, 360f), Random.Range(0, 360f)));
                    return;
                }
            }
        }
    }
}
