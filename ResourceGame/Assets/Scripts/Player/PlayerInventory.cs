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
    public Text inHandDisplay;

    public Container inventory;

    public PlayerMovement playerMovement;
    public GameObject FPSCamera;
    public GameObject itemInHand;
    public Vector3 ItemOffset;
    public float ItemInHandDistanceFromCamera;

    [Header("Sway Left/Right Constants")]
    public float swayAmount;
    public float maxSwayAmount;
    public float smoothAmount;

    [Header("Sway Up/Down Constants")]
    public float rotationSwayAmount;
    public float maxRotationSwayAmount;
    public float rotationSmoothAmount;

    [Header("Rotation Axis To Apply")]
    public bool rotationX;
    public bool rotationY;
    public bool rotationZ;

    /* Privates */
    private Vector3 initPos;
    private Quaternion initRot;

    private void Start()
    {
        inventory = new Container(9, true, inventoryOwner);

        initPos = itemInHand.transform.localPosition;
        initRot = itemInHand.transform.localRotation;
    }

    private void Update()
    {
        updateMouse();
        updateGUI();
        updateItemInHandLocation();
    }

    private void updateMouse()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            Cursor.lockState = Cursor.lockState == CursorLockMode.Locked ? CursorLockMode.None : CursorLockMode.Locked;

            inventoryMenu.SetActive(!inventoryMenu.activeSelf);
            crosshair.SetActive(!crosshair.activeSelf);
        }
    }

    private void updateGUI()
    {
        inHandDisplay.text = itemInHand.GetComponent<ItemStack>().name;
    }

    private void updateItemInHandLocation()
    {
        Vector3 offset = itemInHand.transform.rotation * ItemOffset;
        itemInHand.transform.position = FPSCamera.transform.position + offset + FPSCamera.transform.forward * ItemInHandDistanceFromCamera;

        // Sway
        float movementX = -Input.GetAxis("Mouse X") * swayAmount;
        float movementY = -Input.GetAxis("Mouse Y") * swayAmount;

        movementX = Mathf.Clamp(movementX, -maxSwayAmount, maxSwayAmount);
        movementY = Mathf.Clamp(movementY, -maxSwayAmount, maxSwayAmount);

        Vector3 finalPos = new Vector3(movementX, movementY, 0);

        Vector3 swayPos = Vector3.zero;
        if (playerMovement.isMovingLeftRight)
        {
            float value = 5 * swayAmount * Mathf.Sin(Time.time);
            swayPos = new Vector3(value, value, 0);
        }

        // Rotation
        float tiltY = Input.GetAxis("Mouse X") * rotationSwayAmount;
        float tiltX = Input.GetAxis("Mouse Y") * rotationSwayAmount;

        tiltY = Mathf.Clamp(tiltY, -maxRotationSwayAmount, maxRotationSwayAmount);
        tiltX = Mathf.Clamp(tiltX, -maxRotationSwayAmount, maxRotationSwayAmount);

        Quaternion finalRotPos = Quaternion.Euler(
            new Vector3(
                rotationX ? -tiltX : 0f,
                rotationY ? tiltY : 0f,
                rotationZ ? tiltY : 0f
            )
        );

        Quaternion rotPos = Quaternion.Euler(Vector3.zero);
        if (playerMovement.isMovingForwardBackward)
        {
            float value = .8f * rotationSwayAmount * Mathf.Sin(Time.time);
            rotPos = Quaternion.Euler(
                new Vector3(
                    rotationX ? -value : 0f,
                    rotationY ? value : 0f,
                    rotationZ ? value : 0f
                )
            );
        }

        itemInHand.transform.localPosition = Vector3.Lerp(itemInHand.transform.localPosition, initPos + finalPos + swayPos, Time.deltaTime * smoothAmount);
        itemInHand.transform.localRotation = Quaternion.Slerp(itemInHand.transform.localRotation, initRot * finalRotPos * rotPos, Time.deltaTime * smoothAmount);
    }
}
