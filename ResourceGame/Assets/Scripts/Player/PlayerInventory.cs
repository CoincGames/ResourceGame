using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventory : MonoBehaviour
{
    [Tooltip("The player who owns this inventory (GAMEOBJECT)")]
    public GameObject inventoryOwner;

    [Tooltip("The max items a player can carry at one time.")]
    public int maxInventorySpace;

    [SerializeField]
    [Tooltip("The menu to close on the inventory press key.")]
    private GameObject inventoryMenu = null;

    [SerializeField]
    [Tooltip("The crosshair view to open when the menu gets closed.")]
    private GameObject crosshair = null;

    public Dictionary<ItemStack.ItemType, int> invMap = new Dictionary<ItemStack.ItemType, int>();

    // TEMP
    public ItemStack.ItemType itemInHand = ItemStack.ItemType.Null;

    // TEMP
    public Text inHandDisplay;

    public Container inventory;

    private void Start()
    {
        inventory = new Container(9, true, inventoryOwner);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            Cursor.lockState = Cursor.lockState == CursorLockMode.Locked ? CursorLockMode.None : CursorLockMode.Locked;

            inventoryMenu.SetActive(!inventoryMenu.activeSelf);
            crosshair.SetActive(!crosshair.activeSelf);
        }

        inHandDisplay.text = itemInHand.ToString();
    }
}
