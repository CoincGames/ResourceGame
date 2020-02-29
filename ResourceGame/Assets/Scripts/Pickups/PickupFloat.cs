using UnityEngine;

public class PickupFloat : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The intensity of the sin wave effect on the transform.")]
    [Range(.001f, .02f)]
    private float amplitude = .005f;
    [SerializeField]
    [Tooltip("The speed of which the sin wave effects the transform.")]
    [Range(.5f, 5f)]
    private float speed = 1f;

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y + amplitude * Mathf.Sin(speed * Time.time), transform.position.z);
    }
}
