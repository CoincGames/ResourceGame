using UnityEngine;

public class SpawnItemTesting : MonoBehaviour
{
    public GameObject player;
    public GameObject resourceToSpawn;

    // Update is called once per frame
    void Update()
    {
        listenForSpawnKey();
    }

    private void listenForSpawnKey()
    {
        if (Input.GetKeyDown("`"))
        {
            ItemFactory factory = FindObjectOfType<ItemFactory>();
            factory.CreateResourceAtLocation(resourceToSpawn, player.transform.position, 16, 2.5f, true, ItemStack.ItemType.SmallRock);
        }
    }
}
