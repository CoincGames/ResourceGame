using UnityEngine;

public class RandomColor : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The renderer that owns this script.")]
    private Renderer meshRenderer;

    // Start is called before the first frame update
    void Start()
    {
        meshRenderer.material.color = new Color(Random.value, Random.value, Random.value);
    }
}
