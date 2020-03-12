using UnityEngine;

public class TerrainChunk
{
    const float colliderGenerationDistanceThreshold = 5f;

    public event System.Action<TerrainChunk, bool> onVisibilityChanged;
    public Vector2 coord;

    GameObject meshObject;
    Vector2 sampleCenter;
    Bounds bounds;

    MeshRenderer meshRenderer;
    MeshFilter meshFilter;
    MeshCollider meshCollider;

    LODInfo[] detailLevels;
    LODMesh[] lodMeshes;
    int colliderLODIndex;

    HeightMap heightMap;
    bool hasMapData;
    int previousLODIndex = -1;
    bool hasCollider;
    float maxViewDistance;

    HeightMapSettings heightMapSettings;
    MeshSettings meshSettings;
    MapRulesSettings mapRulesSettings;

    Transform viewer;

    public TerrainChunk(Vector2 coord, HeightMapSettings heightMapSettings, MeshSettings meshSettings, MapRulesSettings mapRulesSettings, LODInfo[] detailLevels, int colliderLODIndex, Transform parent, Transform viewer, Material material)
    {
        this.coord = coord;
        this.detailLevels = detailLevels;
        this.colliderLODIndex = colliderLODIndex;

        this.heightMapSettings = heightMapSettings;
        this.meshSettings = meshSettings;
        this.mapRulesSettings = mapRulesSettings;

        this.viewer = viewer;

        sampleCenter = coord * meshSettings.meshWorldSize / meshSettings.meshScale;
        Vector2 position = coord * meshSettings.meshWorldSize;
        bounds = new Bounds(position, Vector2.one * meshSettings.meshWorldSize);

        meshObject = new GameObject("Terrain Chunk");
        meshObject.layer = LayerMask.NameToLayer("Terrain");
        meshRenderer = meshObject.AddComponent<MeshRenderer>();
        meshFilter = meshObject.AddComponent<MeshFilter>();
        meshCollider = meshObject.AddComponent<MeshCollider>();
        meshRenderer.material = material;

        meshObject.transform.position = new Vector3(position.x, 0, position.y);
        meshObject.transform.parent = parent;

        SetVisible(false);

        lodMeshes = new LODMesh[detailLevels.Length];
        for (int i = 0; i < detailLevels.Length; i++)
        {
            lodMeshes[i] = new LODMesh(detailLevels[i].lod);
            lodMeshes[i].updateCallback += UpdateTerrainChunk;
            if (i == colliderLODIndex)
            {
                lodMeshes[i].updateCallback += UpdateCollisionMesh;
            }
        }

        maxViewDistance = detailLevels[detailLevels.Length - 1].visibleDistanceThreshold;
    }

    public void Load()
    {
        ThreadedDataRequester.RequestData(() => (isWithinMapBounds()) ? 
            HeightMapGenerator.GenerateHeightMap(meshSettings.numberVerticesPerLine, meshSettings.numberVerticesPerLine, heightMapSettings, mapRulesSettings, sampleCenter) : 
            HeightMapGenerator.GenerateOcean(meshSettings.numberVerticesPerLine, meshSettings.numberVerticesPerLine), OnHeightMapReceived);
    }

    bool isWithinMapBounds()
    {
        bool isInXRange = true;
        bool isInYRange = true;

        if (mapRulesSettings.useMaxMapSize)
        {
            int xRangeFromZero = Mathf.FloorToInt(mapRulesSettings.maxMapSizeInChunks.x / 2);
            int yRangeFromZero = Mathf.FloorToInt(mapRulesSettings.maxMapSizeInChunks.y / 2);

            if (coord.x < -xRangeFromZero || coord.x > xRangeFromZero)
                isInXRange = false;
            if (coord.y < -yRangeFromZero || coord.y > yRangeFromZero)
                isInYRange = false;
        }

        return isInXRange && isInYRange;
    }

    void OnHeightMapReceived(object heightMap)
    {
        this.heightMap = (HeightMap)heightMap;
        hasMapData = true;

        UpdateTerrainChunk();
    }

    Vector2 viewerPosition
    {
        get
        {
            return new Vector2(viewer.position.x, viewer.position.z);
        }
    }

    public void UpdateTerrainChunk()
    {
        if (!hasMapData)
            return;

        float viewerDistanceFromNearestEdge = Mathf.Sqrt(bounds.SqrDistance(viewerPosition));

        bool wasVisible = isVisible();
        bool visible = viewerDistanceFromNearestEdge <= maxViewDistance;

        if (visible)
        {
            int lodIndex = 0;

            for (int i = 0; i < detailLevels.Length - 1; i++)
            {
                if (viewerDistanceFromNearestEdge > detailLevels[i].visibleDistanceThreshold)
                {
                    lodIndex = i + 1;
                }
                else
                {
                    break;
                }
            }

            if (lodIndex != previousLODIndex)
            {
                LODMesh lodMesh = lodMeshes[lodIndex];
                if (lodMesh.hasMesh)
                {
                    previousLODIndex = lodIndex;
                    meshFilter.mesh = lodMesh.mesh;
                }
                else if (!lodMesh.hasRequestedMesh)
                {
                    lodMesh.RequestMesh(heightMap, meshSettings);
                }
            }
        }

        if (wasVisible != visible)
        {
            SetVisible(visible);
            onVisibilityChanged?.Invoke(this, visible);
        }
    }

    public void UpdateCollisionMesh()
    {
        if (hasCollider)
            return;

        float sqrDistanceFromViewerToEdge = bounds.SqrDistance(viewerPosition);

        if (sqrDistanceFromViewerToEdge < detailLevels[colliderLODIndex].sqrVisibleDistanceThreshold)
        {
            if (!lodMeshes[colliderLODIndex].hasRequestedMesh)
            {
                lodMeshes[colliderLODIndex].RequestMesh(heightMap, meshSettings);
            }
        }

        if (sqrDistanceFromViewerToEdge < colliderGenerationDistanceThreshold * colliderGenerationDistanceThreshold)
        {
            if (lodMeshes[colliderLODIndex].hasMesh)
            {
                meshCollider.sharedMesh = lodMeshes[colliderLODIndex].mesh;
                hasCollider = true;
            }
        }
    }

    public void SetVisible(bool visible)
    {
        meshObject.SetActive(visible);
    }

    public bool isVisible()
    {
        return meshObject.activeSelf;
    }
}

class LODMesh
{
    public Mesh mesh;
    public bool hasRequestedMesh;
    public bool hasMesh;
    int lod;
    public event System.Action updateCallback;

    public LODMesh(int lod)
    {
        this.lod = lod;
    }

    void OnMeshDataReceived(object meshData)
    {
        mesh = ((MeshData)meshData).CreateMesh();
        hasMesh = true;

        updateCallback();
    }

    public void RequestMesh(HeightMap heightMap, MeshSettings meshSettings)
    {
        hasRequestedMesh = true;
        ThreadedDataRequester.RequestData(() => MeshGenerator.GenerateTerrainMesh(heightMap.values, meshSettings, lod), OnMeshDataReceived);
    }
}