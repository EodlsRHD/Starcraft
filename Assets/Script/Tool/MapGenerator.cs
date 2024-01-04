using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MapGenerator : MonoBehaviour
{
    [SerializeField]
    private Material _matDefult = null;

    private bool _isDone = false;

    public bool isDone
    {
        get { return _isDone; }
    }

    public void Initialize()
    {

    }

    public void Generate()
    {
        _isDone = false;

        MapData mapData = GameManager.instance.currentMapdata;
        StartCoroutine(GenerateMeshData(mapData, (meshData) => 
        {
            _isDone = true;
        }));
    }

    IEnumerator GenerateMeshData(MapData mapData, Action<MeshData> onCallback)
    {
        MeshData meshData = new MeshData(mapData.mapSizeX, mapData.mapSizeY);

        float topLeftX = (mapData.mapSizeX - 1) * (-0.5f);
        float topLeftZ = (mapData.mapSizeY - 1) * (0.5f);
        int vertexIndex = 0;

        for (int y = 0; y < mapData.mapSizeY; y++)
        {
            for (int x = 0; x < mapData.mapSizeX; x++)
            {
                Node node = mapData.nodes[x, y];

                meshData.vertices[vertexIndex] = new Vector3(topLeftX + x, node.topographic.height, topLeftZ - y);
                meshData.uvs[vertexIndex] = new Vector2(x / mapData.mapSizeX, y / mapData.mapSizeY);

                if (x < mapData.mapSizeX - 1 && y < mapData.mapSizeY - 1)
                {
                    meshData.AddTriangle(vertexIndex, vertexIndex + mapData.mapSizeX + 1, vertexIndex + mapData.mapSizeX);
                    meshData.AddTriangle(vertexIndex + mapData.mapSizeX + 1, vertexIndex, vertexIndex + 1);
                }

                vertexIndex++;
            }

            yield return null;
        }

        DrawMesh(meshData);

        onCallback?.Invoke(meshData);
    }

    private void DrawMesh(MeshData meshData)
    {
        GameObject obj = new GameObject();
        obj.name = "Map Object";
        MeshFilter meshFilter = obj.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = obj.AddComponent<MeshRenderer>();

        meshFilter.sharedMesh = meshData.CreateMesh();
        meshRenderer.sharedMaterial = _matDefult;

        MapManager.SetMapObject(obj);
    }
}

public class MeshData
{
    public Vector3[] vertices;
    public Vector2[] uvs;
    public int[] triangles;

    private int _triangleIndex;

    public MeshData(int sizeX, int sizeY)
    {
        vertices = new Vector3[sizeX * sizeY];
        uvs = new Vector2[vertices.Length];
        triangles = new int[(sizeX - 1) * (sizeY - 1) * 6];
    }

    public void AddTriangle(int a, int b, int c)
    {
        triangles[_triangleIndex] = a;
        triangles[_triangleIndex + 1] = b;
        triangles[_triangleIndex + 2] = c;

        _triangleIndex += 3;
    }

    public Mesh CreateMesh()
    {
        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        mesh.RecalculateNormals();

        return mesh;
    }
}
