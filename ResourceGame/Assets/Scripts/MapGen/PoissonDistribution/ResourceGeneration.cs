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

        for (int y = 0; y < verticesPerSide; y++)
        {
            for (int x = 0; x < verticesPerSide; x++)
            {
                int vertexIndex = (y * verticesPerSide) + x;

                Vector3 center, up, left, right, down;

                center = vertices[vertexIndex];

                if (y == 0)
                {
                    up = center;
                } 
                else
                {
                    up = vertices[vertexIndex - verticesPerSide];
                }

                if (y == verticesPerSide - 1)
                {
                    down = center;
                }
                else
                {
                    down = vertices[vertexIndex + verticesPerSide];
                }

                if (x == 0)
                {
                    left = center;
                } 
                else
                {
                    left = vertices[vertexIndex - 1];
                }

                if (x == verticesPerSide - 1)
                {
                    right = center;
                } 
                else
                {
                    right = vertices[vertexIndex + 1];
                }

                vertexInfos[x, y] = new VertexInfo(center, up, left, right, down);
            }
        }

        List<Vector3> v3points = new List<Vector3>();

        v2points.ForEach(vector2 =>
        {
            //Debug.Log(topLeftVertex + "->" + bottomRightVertex + " and " + vector2);
            float xPercentInChunk = Mathf.InverseLerp(topLeftVertex.x, bottomRightVertex.x, vector2.x - gameObjectLoc.x);
            float yPercentInChunk = Mathf.InverseLerp(topLeftVertex.z, bottomRightVertex.z, vector2.y - gameObjectLoc.z);
            //Debug.Log("xPercent: " + xPercentInChunk + " | " + "yPercent: " + yPercentInChunk);

            int xIndex = (int) Mathf.Lerp(0, verticesPerSide, xPercentInChunk);
            int yIndex = (int) Mathf.Lerp(0, verticesPerSide, yPercentInChunk);

            // Check slope FIRST
            //Debug.Log("xIndex: " + xIndex + " | " + "yIndex: " + yIndex);

            float slope = vertexInfos[xIndex, yIndex].GetLargestSlope();
            if (slope < .1 || slope > .35)
                return;

            float height = vertexInfos[xIndex, yIndex].centerPoint.y + 2.75f;

            v3points.Add(new Vector3(vector2.x, height, vector2.y));
        });

        // endregion NEW

        return v3points;
    }
}

class Edge
{
    Vector3 start;
    Vector3 end;
    public float slope;

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
    public Vector3 centerPoint;
    public Edge upEdge, leftEdge, rightEdge, downEdge;

    public VertexInfo(Vector3 center, Vector3 up, Vector3 left, Vector3 right, Vector3 down)
    {
        centerPoint = center;

        upEdge = new Edge(center, up);
        leftEdge = new Edge(center, left);
        rightEdge = new Edge(center, right);
        downEdge = new Edge(center, down);
    }

    public float GetLargestSlope()
    {
        float slope = float.MinValue;

        if (upEdge.slope > slope)
            slope = upEdge.slope;

        if (leftEdge.slope > slope)
            slope = leftEdge.slope;

        if (rightEdge.slope > slope)
            slope = rightEdge.slope;

        if (downEdge.slope > slope)
            slope = downEdge.slope;

        return slope;
    }
}