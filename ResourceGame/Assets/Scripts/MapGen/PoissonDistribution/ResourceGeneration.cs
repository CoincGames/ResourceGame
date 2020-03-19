using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ResourceGeneration
{
    public static List<Vector3> GenerateResourcePoints(float radius, Vector3[] vertices, GameObject gameObject, Vector3 gameObjectLoc, HeightMapSettings heightMapSettings)
    {
        Vector3 topLeftVertex = vertices[0];
        Vector3 bottomRightVertex = vertices[vertices.Length - 1];

        int chunkSizeX = (int)Mathf.Abs(topLeftVertex.x - bottomRightVertex.x);
        int chunkSizeZ = (int)Mathf.Abs(topLeftVertex.z - bottomRightVertex.z);

        Vector2 sampleSize = new Vector2(chunkSizeX, chunkSizeZ);
        Vector2 sampleCenter = new Vector2(bottomRightVertex.x - gameObjectLoc.x, topLeftVertex.z - gameObjectLoc.z);

        List<Vector2> v2points = PoissonDiscSampling.GeneratePoints(radius, sampleSize, sampleCenter, 2, heightMapSettings.noiseSettings.seed);

        // region NEW
        int verticesPerSide = (int)Mathf.Sqrt(vertices.Length);
        VertexInfo[,] vertexInfos = new VertexInfo[verticesPerSide, verticesPerSide];


        int endIndex = 0;
        for (int x = 0; x < verticesPerSide; x++)
        {
            for (int y = 0; y < verticesPerSide; y++)
            {
                endIndex = (x * verticesPerSide) + y;
            }
        }

        Debug.LogError(endIndex);

        // endregion NEW

        List<Vector3> v3points = new List<Vector3>();

        // Add a y component for vertical based off ^^ vertices

        v2points.ForEach(vector2 => v3points.Add(new Vector3(vector2.x, 150, vector2.y)));

        return v3points;
    }
}

class Edge
{
    Vector3 start;
    Vector3 end;
    float slope;

    public Edge(Vector3 start, Vector3 end)
    {
        this.start = start;
        this.end = end;

        slope = CalculateSlope();
    }

    float CalculateSlope()
    {
        float value;

        if (start.x == end.x)
        {
            // Run on Z axis
            value = (end.y - start.y) / (end.z - start.z);
        } else
        {
            // Run on X axis
            value = (end.y - start.y) / (end.x - start.x);
        }

        return Mathf.Abs(value);
    }
}

class VertexInfo
{
    Edge upEdge, leftEdge, rightEdge, downEdge;


}