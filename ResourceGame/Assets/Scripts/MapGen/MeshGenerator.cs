using UnityEngine;

public static class MeshGenerator
{
    public static MeshData GenerateTerrainMesh(float[,] heightMap, MeshSettings settings, int levelOfDetail)
    {
        int skipIncrement = levelOfDetail == 0 ? 1 : levelOfDetail * 2;
        int numberVerticesPerLine = settings.numberVerticesPerLine;

        Vector2 topLeft = new Vector2(-1, 1) * settings.meshWorldSize / 2f;

        MeshData meshData = new MeshData(numberVerticesPerLine, skipIncrement, settings.useFlatShading);

        int[,] vertexIndicesMap = new int[numberVerticesPerLine, numberVerticesPerLine];
        int meshVertexIndex = 0;
        int outOfMeshVertexIndex = -1;

        for (int y = 0; y < numberVerticesPerLine; y++)
        {
            for (int x = 0; x < numberVerticesPerLine; x++)
            {
                bool isOutOfMeshVertex = y == 0 || y == numberVerticesPerLine - 1 || x == 0 || x == numberVerticesPerLine - 1;
                bool isSkippedVertex = x > 2 && x < numberVerticesPerLine - 3 && y > 2 && y < numberVerticesPerLine - 3 && ((x - 2) % skipIncrement != 0 || (y - 2) % skipIncrement != 0);

                if (isOutOfMeshVertex)
                {
                    vertexIndicesMap[x, y] = outOfMeshVertexIndex;
                    outOfMeshVertexIndex--;

                } 
                else if (!isSkippedVertex)
                {
                    vertexIndicesMap[x, y] = meshVertexIndex;
                    meshVertexIndex++;
                }
            }
        }

        for (int y = 0; y < numberVerticesPerLine; y++)
        {
            for (int x = 0; x < numberVerticesPerLine; x++)
            {
                bool isSkippedVertex = x > 2 && x < numberVerticesPerLine - 3 && y > 2 && y < numberVerticesPerLine - 3 && ((x - 2) % skipIncrement != 0 || (y - 2) % skipIncrement != 0);

                if (isSkippedVertex)
                    continue;

                bool isOutOfMeshVertex = y == 0 || y == numberVerticesPerLine - 1 || x == 0 || x == numberVerticesPerLine - 1;
                bool isMeshEdgeVertex = (y == 1 || y == numberVerticesPerLine - 2 || x == 1 || x == numberVerticesPerLine - 2) && !isOutOfMeshVertex;
                bool isMainVertex = (x - 2) % skipIncrement == 0 && (y - 2) % skipIncrement == 0 && !isOutOfMeshVertex && !isMeshEdgeVertex;
                bool isEdgeConnectionVertex = (y == 2 || y == numberVerticesPerLine - 3 || x == 2 || x == numberVerticesPerLine - 3) && !isOutOfMeshVertex && !isMeshEdgeVertex && !isMainVertex;

                int vertexIndex = vertexIndicesMap[x, y];
                Vector2 percent = new Vector2(x - 1, y - 1) / (numberVerticesPerLine - 3);
                Vector2 vertexPostion2D = topLeft + new Vector2(percent.x, -percent.y) * settings.meshWorldSize;
                float height = heightMap[x, y];

                if (isEdgeConnectionVertex)
                {
                    bool isVertical = x == 2 || x == numberVerticesPerLine - 3;
                    int distanceToMainVertexA = ((isVertical ? y - 2 : x - 2) % skipIncrement);
                    int distanceToMainVertexB = (skipIncrement - distanceToMainVertexA);
                    float distancePercentFromAToB = distanceToMainVertexA / (float)skipIncrement;

                    float heightMainVertexA = heightMap[(isVertical ? x : x - distanceToMainVertexA), (isVertical ? y - distanceToMainVertexA : y)];
                    float heightMainVertexB = heightMap[(isVertical ? x : x + distanceToMainVertexB), (isVertical ? y + distanceToMainVertexB : y)];

                    height = heightMainVertexA * (1 - distancePercentFromAToB) + heightMainVertexB * distancePercentFromAToB;
                }

                meshData.AddVertex(new Vector3(vertexPostion2D.x, height, vertexPostion2D.y), percent, vertexIndex);

                bool createTriangle = x < numberVerticesPerLine - 1 && y < numberVerticesPerLine - 1 && (!isEdgeConnectionVertex || (x != 2 && y != 2));

                if (createTriangle)
                {
                    int currentIncrement = (isMainVertex && x != numberVerticesPerLine - 3 && y != numberVerticesPerLine - 3) ? skipIncrement : 1;

                    int a = vertexIndicesMap[x, y];
                    int b = vertexIndicesMap[x + currentIncrement, y];
                    int c = vertexIndicesMap[x, y + currentIncrement];
                    int d = vertexIndicesMap[x + currentIncrement, y + currentIncrement];

                    meshData.AddTriangle(a, d, c);
                    meshData.AddTriangle(d, a, b);
                }
            }
        }

        meshData.ProcessMesh();

        return meshData;
    }
}

public class MeshData
{
    Vector3[] vertices;
    int[] triangles;
    Vector2[] uvs;
    Vector3[] bakedNormals;

    Vector3[] outOfMeshVertices;
    int[] outOfMeshTriangles;

    int triangleIndex;
    int outOfMeshTriangleIndex;

    bool useFlatShading;

    public MeshData(int numberVerticesPerLine, int skipIncrement, bool useFlatShading)
    {
        this.useFlatShading = useFlatShading;

        int numberMeshEdgeVertices = (numberVerticesPerLine - 2) * 4 - 4;
        int numberEdgeConnectionVertices = (skipIncrement - 1) * (numberVerticesPerLine - 5) / skipIncrement * 4;
        int numberMainVerticesPerLine = (numberVerticesPerLine - 5) / skipIncrement + 1;
        int numberMainVertices = numberMainVerticesPerLine * numberMainVerticesPerLine;

        vertices = new Vector3[numberMeshEdgeVertices + numberEdgeConnectionVertices + numberMainVertices];
        uvs = new Vector2[vertices.Length];

        int numberMeshEdgeTriangles = 8 * (numberVerticesPerLine - 4);
        int numberMainTriangles = (numberMainVerticesPerLine - 1) * (numberMainVerticesPerLine - 1) * 2;
        triangles = new int[(numberMeshEdgeTriangles + numberMainTriangles) * 3];

        outOfMeshVertices = new Vector3[numberVerticesPerLine * 4 - 4];
        outOfMeshTriangles = new int[24 * (numberVerticesPerLine - 2)];
    }

    public void AddVertex(Vector3 vertexPosition, Vector2 uv, int vertexIndex)
    {
        if (vertexIndex < 0)
        {
            outOfMeshVertices[-vertexIndex - 1] = vertexPosition;
        } else
        {
            vertices[vertexIndex] = vertexPosition;
            uvs[vertexIndex] = uv;
        }
    }

    public void AddTriangle(int a, int b, int c)
    {
        if (a < 0 || b < 0 || c < 0)
        {

            outOfMeshTriangles[outOfMeshTriangleIndex] = a;
            outOfMeshTriangles[outOfMeshTriangleIndex + 1] = b;
            outOfMeshTriangles[outOfMeshTriangleIndex + 2] = c;
            outOfMeshTriangleIndex += 3;
        } else
        {
            triangles[triangleIndex] = a;
            triangles[triangleIndex + 1] = b;
            triangles[triangleIndex + 2] = c;
            triangleIndex += 3;
        }
    }

    Vector3[] CalculateNormals()
    {
        Vector3[] vertexNormals = new Vector3[vertices.Length];
        int triangleCount = triangles.Length / 3;
        for (int i = 0; i < triangleCount; i++)
        {
            int normalTriangleIndex = i * 3;
            int vertexIndexA = triangles[normalTriangleIndex];
            int vertexIndexB = triangles[normalTriangleIndex + 1];
            int vertexIndexC = triangles[normalTriangleIndex + 2];

            Vector3 triangleNormal = SurfaceNormalFromIndices(vertexIndexA, vertexIndexB, vertexIndexC);
            vertexNormals[vertexIndexA] += triangleNormal;
            vertexNormals[vertexIndexB] += triangleNormal;
            vertexNormals[vertexIndexC] += triangleNormal;
        }

        int borderTriangleCount = outOfMeshTriangles.Length / 3;
        for (int i = 0; i < borderTriangleCount; i++)
        {
            int normalTriangleIndex = i * 3;
            int vertexIndexA = outOfMeshTriangles[normalTriangleIndex];
            int vertexIndexB = outOfMeshTriangles[normalTriangleIndex + 1];
            int vertexIndexC = outOfMeshTriangles[normalTriangleIndex + 2];

            Vector3 triangleNormal = SurfaceNormalFromIndices(vertexIndexA, vertexIndexB, vertexIndexC);
            if (vertexIndexA >= 0)
                vertexNormals[vertexIndexA] += triangleNormal;
            if (vertexIndexB >= 0)
                vertexNormals[vertexIndexB] += triangleNormal;
            if (vertexIndexC >= 0)
                vertexNormals[vertexIndexC] += triangleNormal;
        }

        for (int i = 0; i < vertexNormals.Length; i++)
        {
            vertexNormals[i].Normalize();
        }

        return vertexNormals;
    }

    Vector3 SurfaceNormalFromIndices(int indexA, int indexB, int indexC)
    {
        Vector3 pointA = indexA < 0 ? outOfMeshVertices[-indexA - 1] : vertices[indexA];
        Vector3 pointB = indexB < 0 ? outOfMeshVertices[-indexB - 1] : vertices[indexB];
        Vector3 pointC = indexC < 0 ? outOfMeshVertices[-indexC - 1] : vertices[indexC];

        Vector3 sideAB = pointB - pointA;
        Vector3 sideAC = pointC - pointA;
        return Vector3.Cross(sideAB, sideAC).normalized;
    }

    public void ProcessMesh()
    {
        if (useFlatShading)
            FlatShading();
        else
            BakeNormals();
    }

    private void BakeNormals()
    {
        bakedNormals = CalculateNormals();
    }

    void FlatShading()
    {
        Vector3[] flatShadedVertices = new Vector3[triangles.Length];
        Vector2[] flatShadedUvs = new Vector2[triangles.Length];

        for (int i = 0; i < triangles.Length; i++)
        {
            flatShadedVertices[i] = vertices[triangles[i]];
            flatShadedUvs[i] = uvs[triangles[i]];
            triangles[i] = i;
        }

        vertices = flatShadedVertices;
        uvs = flatShadedUvs;
    }

    public Mesh CreateMesh()
    {
        Mesh mesh = new Mesh();

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        if (useFlatShading)
            mesh.RecalculateNormals();
        else
            mesh.normals = bakedNormals;

        return mesh;
    }
}