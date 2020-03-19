using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapPreview : MonoBehaviour
{
    public Renderer textureRender;
    public MeshFilter meshFilter;
    public MeshRenderer meshRenderer;

    public enum DrawMode { NoiseMap, FalloffMap, Mesh, Map };
    public DrawMode drawMode;

    public MeshSettings meshSettings;
    public HeightMapSettings heightMapSettings;
    public TextureData textureData;
    public MapRulesSettings mapRulesSettings;

    public Material terrainMaterial;

    [Range(0, MeshSettings.numSupportedLODs - 1)]
    public int editorPreviewLOD;

    public bool autoUpdate;

    [Header("Tree testing")]
    public float treeDistance = 8f;

    private List<GameObject> createdTrees = new List<GameObject>();

    public void DrawMapInEditor()
    {
        textureData.ApplyToMaterial(terrainMaterial);
        textureData.UpdateMeshHeights(terrainMaterial, heightMapSettings.minHeight, heightMapSettings.maxHeight);

        ChunkBorderInfo chunkBorderInfo = new ChunkBorderInfo(false, FalloffGenerator.Corner.TOPLEFT, false, FalloffGenerator.Edge.TOP);
        HeightMap heightMap = HeightMapGenerator.GenerateHeightMap(meshSettings.numberVerticesPerLine, meshSettings.numberVerticesPerLine, heightMapSettings, mapRulesSettings, Vector2.zero, chunkBorderInfo);

        CleanScene();

        if (drawMode == DrawMode.NoiseMap)
            DrawTexture(TextureGenerator.TextureFromHeightMap(heightMap));
        else if (drawMode == DrawMode.FalloffMap)
            DrawTexture(TextureGenerator.TextureFromHeightMap(new HeightMap(FalloffGenerator.GenerateFalloffMap(meshSettings.numberVerticesPerLine, mapRulesSettings), 0, 1)));
        else if (drawMode == DrawMode.Mesh)
            DrawMesh(MeshGenerator.GenerateTerrainMesh(heightMap.values, meshSettings, editorPreviewLOD));
        else if (drawMode == DrawMode.Map)
        {
            DrawMesh(MeshGenerator.GenerateTerrainMesh(heightMap.values, meshSettings, editorPreviewLOD));
            DrawMap();
        }
    }

    public void DrawTexture(Texture2D texture)
    {
        textureRender.sharedMaterial.mainTexture = texture;
        textureRender.transform.localScale = new Vector3(texture.width, 1, texture.height) / 10f;

        textureRender.gameObject.SetActive(true);
        meshFilter.gameObject.SetActive(false);
    }

    public void DrawMesh(MeshData meshData)
    {
        meshFilter.sharedMesh = meshData.CreateMesh();

        textureRender.gameObject.SetActive(false);
        meshFilter.gameObject.SetActive(true);
    }

    public void DrawMap()
    {
        List<Vector3> treePoints = ResourceGeneration.GenerateResourcePoints(treeDistance, meshFilter.sharedMesh.vertices, meshFilter.gameObject, Vector3.zero, heightMapSettings);

        foreach (Vector3 location in treePoints)
        {
            GameObject treeAsset = FindObjectOfType<TerrainGenerator>().resourcePool.tree;
            GameObject createdTree = Object.Instantiate(treeAsset, location, treeAsset.transform.rotation, meshFilter.gameObject.transform) as GameObject;
            createdTrees.Add(createdTree);
        }
    }

    void CleanScene()
    {
        // Clear the list of created trees from DrawMap()
        foreach (GameObject gameObject in createdTrees)
        {
            DestroyImmediate(gameObject);
        }
        createdTrees.Clear();
    }

    void OnValuesUpdated()
    {
        if (!Application.isPlaying)
        {
            DrawMapInEditor();
        }
    }

    void OnTextureValuesUpdated()
    {
        textureData.ApplyToMaterial(terrainMaterial);
    }

    private void OnValidate()
    {
        if (meshSettings != null)
        {
            meshSettings.OnValuesUpdated -= OnValuesUpdated;
            meshSettings.OnValuesUpdated += OnValuesUpdated;
        }
        if (heightMapSettings != null)
        {
            heightMapSettings.OnValuesUpdated -= OnValuesUpdated;
            heightMapSettings.OnValuesUpdated += OnValuesUpdated;
        }
        if (textureData != null)
        {
            textureData.OnValuesUpdated -= OnTextureValuesUpdated;
            textureData.OnValuesUpdated += OnTextureValuesUpdated;
        }
        if (mapRulesSettings != null)
        {
            mapRulesSettings.OnValuesUpdated -= OnValuesUpdated;
            mapRulesSettings.OnValuesUpdated += OnValuesUpdated;
        }
    }
}
