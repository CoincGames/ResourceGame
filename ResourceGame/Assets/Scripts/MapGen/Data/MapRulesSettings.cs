using UnityEngine;

[CreateAssetMenu()]
public class MapRulesSettings : UpdatableData
{
    [Header("Map Max Size Properties")]
    public bool useMaxMapSize;
    public Vector2 maxMapSizeInChunks;

    [Header("Battle Royale")]
    public bool useFalloff;
    [Range(0f, 10f)]
    public float slope;
    [Range(0f, 10f)]
    public float shift;
}
