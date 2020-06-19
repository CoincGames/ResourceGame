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
            GameObject createdWood = Instantiate(resourceToSpawn, player.transform.position, resourceToSpawn.transform.rotation) as GameObject;
            ItemStack itemStack = createdWood.GetComponent<ItemStack>();
            itemStack.amount = 16;
        }
    }
}
