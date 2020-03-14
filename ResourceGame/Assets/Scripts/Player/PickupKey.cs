using UnityEngine;
using UnityEngine.UI;

public class PickupKey : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The camera that the player view is projected from.")]
    private Camera viewFromCamera = null;

    [SerializeField]
    [Tooltip("The pickup text view to display when looking at a pickupable object.")]
    private Text pickupText = null;

    [SerializeField]
    [Tooltip("The slide view component that owns the slide animation script.")]
    private PickupSlideView pickupSlideView = null;

    private Resource wantToSelect;

    // Update is called once per frame
    void Update()
    {
        findNearestPickupInLOS();
        listenForPickupKey();
    }

    // TODO If this is too touchy
    // https://answers.unity.com/questions/714071/how-can-i-make-auto-aim.html
    private void findNearestPickupInLOS()
    {
        RaycastHit hitInfo;
        // =~ is a binary inversion operator... in this case it gets all layers that ARENT Player
        int layerMask = ~LayerMask.GetMask("Player");

        bool hit = Physics.Raycast(viewFromCamera.transform.position, viewFromCamera.transform.forward, out hitInfo, 7f, layerMask);

        if (hit && hitInfo.collider.tag == "Pickup")
        {
            wantToSelect = hitInfo.collider.gameObject.GetComponent<Resource>();
            if (!pickupText.gameObject.activeSelf)
            {
                pickupText.gameObject.SetActive(true);

                pickupText.text = "Press E to pickup " + wantToSelect.name;
            }
        }
        else
        {
            wantToSelect = null;
            if (pickupText.gameObject.activeSelf)
            {
                pickupText.gameObject.SetActive(false);
            }
        }
    }

    private void listenForPickupKey()
    {
        if (Input.GetKeyDown("e"))
        {
            if (wantToSelect != null)
            {
                pickupSlideView.displayWithResource(wantToSelect);
                wantToSelect.PickUp(gameObject.GetComponentInParent<PlayerExperience>(), gameObject.GetComponentInParent<PlayerInventory>());
            }
        }
    }
}
