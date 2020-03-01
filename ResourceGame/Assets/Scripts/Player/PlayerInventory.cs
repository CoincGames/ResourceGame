using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [Tooltip("The max items a player can carry at one time.")]
    public int maxInventorySpace;

    [SerializeField]
    [Tooltip("The menu to close on the inventory press key.")]
    private GameObject inventoryMenu;

    public Dictionary<Resource.ResourceType, int> invMap = new Dictionary<Resource.ResourceType, int>();

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            Cursor.lockState = Cursor.lockState == CursorLockMode.Locked ? CursorLockMode.None : CursorLockMode.Locked;

            if (!inventoryMenu.activeSelf)
            {
                inventoryMenu.SetActive(true);
            } else
            {
                inventoryMenu.SetActive(false);
            }
        }
    }
}
