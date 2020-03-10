using UnityEngine;

[CreateAssetMenu()]
public class MeshSettings : UpdatableData
{
    public const int numSupportedLODs = 5;
    public const int numSupportedChunkSizes = 9;
    public const int numSupportedFlatShadedChunkSizes = 3;
    public static readonly int[] supportedChunkSizes = { 48, 72, 96, 120, 144, 168, 192, 216, 240 };

    public float meshScale = 3f;
    public bool useFlatShading;

    [Range(0, numSupportedChunkSizes - 1)]
    public int chunkSizeIndex;

    [Range(0,numSupportedFlatShadedChunkSizes - 1)]
    public int flatShadedChunkSizeIndex;

    // Accessor for the number of vertices per line of mesh rendered at LOD = 0 (The highest detail level).  
    // Includes the 2 extra vertices that are excluded from final mesh, but used for calculating normals for lighting and seems
    public int numberVerticesPerLine
    {
        get
        {
            return supportedChunkSizes[useFlatShading ? flatShadedChunkSizeIndex : chunkSizeIndex] + 5;
        }
    }

    public float meshWorldSize
    {
        get
        {
            return (numberVerticesPerLine - 3) * meshScale;
        }
    }
}
