using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [Tooltip("The max items a player can carry at one time.")]
    public int maxInventorySpace;

    [SerializeField]
    [Tooltip("The menu to close on the inventory press key.")]
    private GameObject inventoryMenu = null;

    [SerializeField]
    [Tooltip("The crosshair view to open when the menu gets closed.")]
    private GameObject crosshair = null;

    public Dictionary<Item.ItemType, int> invMap = new Dictionary<Item.ItemType, int>();

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            Cursor.lockState = Cursor.lockState == CursorLockMode.Locked ? CursorLockMode.None : CursorLockMode.Locked;

            if (!inventoryMenu.activeSelf)
            {
                inventoryMenu.SetActive(true);
                crosshair.SetActive(false);
            } else
            {
                inventoryMenu.SetActive(false);
                crosshair.SetActive(true);
            }
        }
    }
}
