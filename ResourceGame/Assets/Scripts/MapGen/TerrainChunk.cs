using UnityEngine;
using System.Collections.Generic;

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

    // TODO TEMP
    ResourcePool resourcePool;

    Transform viewer;

    bool hasResources;
    List<GameObject> resourceNodeList = new List<GameObject>();

    public TerrainChunk(Vector2 coord, HeightMapSettings heightMapSettings, MeshSettings meshSettings, MapRulesSettings mapRulesSettings, LODInfo[] detailLevels, int colliderLODIndex, Transform parent, Transform viewer, Material material, ResourcePool resourcePool)
    {
        this.coord = coord;
        this.detailLevels = detailLevels;
        this.colliderLODIndex = colliderLODIndex;

        this.heightMapSettings = heightMapSettings;
        this.meshSettings = meshSettings;
        this.mapRulesSettings = mapRulesSettings;

        this.resourcePool = resourcePool;

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
            lodMeshes[i].updateCallback += GenerateResourceNodes;
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
            HeightMapGenerator.GenerateHeightMap(meshSettings.numberVerticesPerLine, meshSettings.numberVerticesPerLine, heightMapSettings, mapRulesSettings, sampleCenter, GetChunkBorderInfo()) : 
            HeightMapGenerator.GenerateOcean(meshSettings.numberVerticesPerLine, meshSettings.numberVerticesPerLine), 
            OnHeightMapReceived
        );
    }

    ChunkBorderInfo GetChunkBorderInfo()
    {
        bool isCorner = false;
        FalloffGenerator.Corner corner = FalloffGenerator.Corner.TOPLEFT;
        bool isEdge = false;
        FalloffGenerator.Edge edge = FalloffGenerator.Edge.TOP;

        Vector4 mapBounds = getMapBounds();

        // Chunk in top left
        if (coord.x == mapBounds.x && coord.y == mapBounds.w)
        {
            isCorner = true;
        }
        // Top right
        else if (coord.x == mapBounds.w && coord.y == mapBounds.y)
        {
            isCorner = true;
            corner = FalloffGenerator.Corner.BOTTOMLEFT;
        }
        // Bottom left
        else if (coord.x == mapBounds.x && coord.y == mapBounds.z)
        {
            isCorner = true;
            corner = FalloffGenerator.Corner.TOPRIGHT;
        }
        // Bottom right
        else if (coord.x == mapBounds.y && coord.y == mapBounds.z)
        {
            isCorner = true;
            corner = FalloffGenerator.Corner.BOTTOMRIGHT;
        }
        // Left
        else if (coord.x == mapBounds.z)
        {
            isEdge = true;
        }
        // Bottom
        else if (coord.y == mapBounds.x)
        {
            isEdge = true;
            edge = FalloffGenerator.Edge.RIGHT;
        }
        // Right
        else if (coord.x == mapBounds.w)
        {
            isEdge = true;
            edge = FalloffGenerator.Edge.BOTTOM;
        }
        else if (coord.y == mapBounds.y)
        {
            isEdge = true;
            edge = FalloffGenerator.Edge.LEFT;
        }

        return new ChunkBorderInfo(isCorner, corner, isEdge, edge);
    }

    Vector4 getMapBounds()
    {
        int xRangeFromZero = Mathf.FloorToInt(mapRulesSettings.maxMapSizeInChunks.x / 2);
        int yRangeFromZero = Mathf.FloorToInt(mapRulesSettings.maxMapSizeInChunks.y / 2);

        return new Vector4(-xRangeFromZero, xRangeFromZero, -yRangeFromZero, yRangeFromZero);
    }

    bool isWithinMapBounds()
    {
        bool isInXRange = true;
        bool isInYRange = true;

        if (mapRulesSettings.useMaxMapSize)
        {
            Vector4 mapBounds = getMapBounds();

            if (coord.x < mapBounds.x || coord.x > mapBounds.y)
                isInXRange = false;
            if (coord.y < mapBounds.z || coord.y > mapBounds.w)
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

    void GenerateResourceNodes()
    {
        if (hasResources)
            return;

        Vector3[] vertices = meshFilter.mesh.vertices;
        Vector3 gameObjectPosition = meshObject.transform.position;
        ThreadedDataRequester.RequestData(() => ResourceGeneration.GenerateResourcePoints(20f, vertices, meshObject, gameObjectPosition, heightMapSettings), PlaceTrees);

        hasResources = true;

        /*for (int i = 0; i < chunkSize; i++)
        {
            for (int j = 0; j < chunkSize; j++)
            {
                int index = i * chunkSize + j;
                // Debug.Log("Index (" + index + ") = " + vertices[index]); // TODO Figure out what to do with these vertex cords... they are in world space... grab a distance around them and see if slope (in all 8 directions) is crazy? probably.
            }
        } */
    }

    void PlaceTrees(object points)
    {
        foreach (Vector3 location in (List<Vector3>)points)
        {
            GameObject tree = Object.Instantiate(resourcePool.tree, location, resourcePool.tree.transform.rotation, meshObject.transform) as GameObject;
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

public struct ChunkBorderInfo
{
    public readonly bool isCorner;
    public readonly FalloffGenerator.Corner corner;
    public readonly bool isEdge;
    public readonly FalloffGenerator.Edge edge;

    public ChunkBorderInfo(bool isCorner, FalloffGenerator.Corner corner, bool isEdge, FalloffGenerator.Edge edge) {
        this.isCorner = isCorner;
        this.corner = corner;
        this.isEdge = isEdge;
        this.edge = edge;
    }
}