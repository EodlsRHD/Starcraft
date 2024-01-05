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
        StartCoroutine(GenerateMeshData(mapData, (map, resouecrs, startPositions) => 
        {
            Done(map, resouecrs, startPositions);
        }));
    }

    IEnumerator GenerateMeshData(MapData mapData, Action<GameObject, List<Node>, List<Node>> onCallback)
    {
        List<Node> startPositions = new List<Node>();
        List<Node> resources = new List<Node>();
        MeshData meshData = new MeshData(mapData.mapSizeX, mapData.mapSizeY);

        float topLeftX = (mapData.mapSizeX - 1) * (-0.5f);
        float topLeftZ = (mapData.mapSizeY - 1) * (0.5f);
        int vertexIndex = 0;

        for (int y = 0; y < mapData.mapSizeY; y++)
        {
            for (int x = 0; x < mapData.mapSizeX; x++)
            {
                Node node = mapData.nodes[x, y];

                if(node.startPosition.playerColor != ePlayerColor.Non)
                {
                    startPositions.Add(node);
                }

                if(node.resource.type != eResourceType.Non)
                {
                    if(node.resource.type == eResourceType.Gas)
                    {
                        resources.Add(node);
                    }

                    if(node.resource.type == eResourceType.Mineral)
                    {
                        resources.Add(node);
                    }
                }

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

        onCallback?.Invoke(DrawMesh(meshData), resources, startPositions);
    }

    private GameObject DrawMesh(MeshData meshData)
    {
        GameObject obj = new GameObject();
        obj.name = "Map Object";
        MeshFilter meshFilter = obj.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = obj.AddComponent<MeshRenderer>();

        meshFilter.sharedMesh = meshData.CreateMesh();
        meshRenderer.sharedMaterial = _matDefult;

        return obj;
    }

    private void Done(GameObject map, List<Node> resources, List<Node> startPositions)
    {
        MapManager.SetMapObject(map, resources, startPositions);

        _isDone = true;
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
