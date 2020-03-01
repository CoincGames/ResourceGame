using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The type of terrain node to spawn.")]
    private GameObject terrainTile;

    [SerializeField]
    [Tooltip("The map size to generate.")]
    private int mapSize = 12;

    [Header("Spawning Resources Properties.")]
    [Tooltip("The spawn rate of these items.")]
    [Range(0f, 1f)]
    public float spawnRatePercentage;

    [SerializeField]
    [Tooltip("The possible prefab resources to spawn.\n\nNOTE: Should be of type pickup")]
    private GameObject[] possibleSpawns;

    // Start is called before the first frame update
    void Start()
    {
        Vector3 centerVector = new Vector3(.5f, 0, .5f);
        int startPos = (mapSize / 2) * -1;

        for (int x = startPos; x < mapSize / 2; x++)
        {
            for (int z = startPos; z < mapSize / 2; z++)
            {
                GameObject instantiated = Instantiate(terrainTile, centerVector + new Vector3(x, 0, z), transform.rotation) as GameObject;
                instantiated.transform.SetParent(gameObject.transform);

                SpawnResource script = instantiated.AddComponent<SpawnResource>();
                script.spawnRatePercentage = spawnRatePercentage;
                script.possibleSpawns = possibleSpawns;
            }
        }

        foreach (var renderer in gameObject.GetComponentsInChildren<Renderer>())
        {
            // Creates a nice grassy color
            renderer.material.color = new Color(0, (Random.value / 8) + .4f, 0);

        }
    }
}
