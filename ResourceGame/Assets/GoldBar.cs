using UnityEngine;
using UnityEngine.UI;

public class GoldBar : MonoBehaviour
{
    public GameObject container;
    public PlayerInventory inventory;
    public Text textView;
    public Slider slider;

    private void Start()
    {
        container.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        textView.text = inventory.gold + " / " + inventory.maxInventorySpace;
        slider.value = inventory.gold;
    }
}
