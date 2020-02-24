using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [Tooltip("The max items a player can carry at one time.")]
    public int maxInventorySpace;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
            Cursor.lockState = Cursor.lockState == CursorLockMode.Locked ? CursorLockMode.None : CursorLockMode.Locked;
    }
}
