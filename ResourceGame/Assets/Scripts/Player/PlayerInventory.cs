using UnityEngine;
using UnityEngine.UI;

public class PlayerInventory : MonoBehaviour
{
    [Tooltip("The player who owns this inventory (Player's GameObject)")]
    public GameObject inventoryOwner;

    [Tooltip("The max items a player can carry at one time.")]
    public int maxInventorySpace;

    [SerializeField]
    [Tooltip("The menu to close on the inventory press key.")]
    private GameObject inventoryMenu = null;

    [SerializeField]
    [Tooltip("The crosshair view to open when the menu gets closed.")]
    private GameObject crosshair = null;

    // TEMP
    public ItemType itemInHand = ItemType.NULL;

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
