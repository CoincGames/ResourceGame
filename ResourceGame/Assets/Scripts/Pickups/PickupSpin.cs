using UnityEngine;

public class PickupSpin : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The speed of which a pickup rotates. (In angle per second)")]
    [Range(25f, 100f)]
    private float rotationSpeed = 50f;

    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(transform.position, Vector3.up, Time.deltaTime * rotationSpeed);
    }
}
