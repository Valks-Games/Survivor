using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class WorldChunk : MonoBehaviour
{
    private Mesh mesh;
    private MeshRenderer meshRenderer;

    private Vector3[] vertices;
    private Vector2[] uvs;
    private int[] triangles;

    private Transform chunks;

    private float cellSize;
    private int chunkSize;
    private int worldSize;

    private GameObject prefabTree;
    private GameObject prefabRock;

    public Category CategoryStructures;
    public Category CategoryRocks;
    public Category CategoryTrees;
    public Category CategoryBases;

    private float seed;
    private float freqX;
    private float freqZ;
    private float amplitude;

    public void Awake()
    {
        seed = World.Seed;
        freqX = World.FreqX;
        freqZ = World.FreqZ;
        amplitude = World.Amplitude;

        mesh = GetComponent<MeshFilter>().mesh;
        meshRenderer = GetComponent<MeshRenderer>();
        prefabTree = World.prefabTree;
        prefabRock = World.prefabRock;
        chunkSize = World.ChunkSize;
        cellSize = World.CellSize;
        worldSize = World.WorldSize;

        // Setup layout
        CategoryStructures = new Category("Structures", transform);
        CategoryTrees = new Category("Trees", CategoryStructures.Transform);
        CategoryRocks = new Category("Rocks", CategoryStructures.Transform);
        CategoryBases = new Category("Bases", CategoryStructures.Transform);

        chunks = World.CategoryChunks.Transform;
        transform.parent = chunks;
    }

    public void Start()
    {
        meshRenderer.material = Resources.Load("Materials/Terrain") as UnityEngine.Material;
    }

    public void Generate(int chunkX, int chunkZ)
    {
        // Discrete Procedural Grid (2 vertices are never shared)
        Vector3 renderOffset = new Vector3(chunkX * chunkSize * cellSize, 0, chunkZ * chunkSize * cellSize);

        vertices = new Vector3[chunkSize * chunkSize * 4];
        uvs = new Vector2[vertices.Length];
        triangles = new int[chunkSize * chunkSize * 6];

        int v = 0;
        int t = 0;

        float vertexOffset = cellSize * 0.5f; // Multiplication is not as intensive as division.

        for (int cellX = 0; cellX < chunkSize; cellX++)
        {
            for (int cellZ = 0; cellZ < chunkSize; cellZ++)
            {
                Vector3 cellOffset = new Vector3(cellX * cellSize, 0, cellZ * cellSize);

                vertices[v] = new Vector3(-vertexOffset, 0, -vertexOffset) + cellOffset + renderOffset;
                vertices[v + 1] = new Vector3(-vertexOffset, 0, vertexOffset) + cellOffset + renderOffset;
                vertices[v + 2] = new Vector3(vertexOffset, 0, -vertexOffset) + cellOffset + renderOffset;
                vertices[v + 3] = new Vector3(vertexOffset, 0, vertexOffset) + cellOffset + renderOffset;

                triangles[t] = v;
                triangles[t + 1] = triangles[t + 4] = v + 1;
                triangles[t + 2] = triangles[t + 3] = v + 2;
                triangles[t + 5] = v + 3;

                v += 4;
                t += 6;
            }
        }

        // UVs
        Vector2[] dirt1 = SpriteLoader.GetTileUVs("Dirt_1");
        Vector2[] dirt2 = SpriteLoader.GetTileUVs("Dirt_2");
        Vector2[] water1 = SpriteLoader.GetTileUVs("Water_1");

        int k = 0;
        for (int g = 0; g < vertices.Length / 4; g++)
        {
            float groundNoise = amplitude * Mathf.PerlinNoise(seed + (g / freqX), seed + (g / freqZ));
            float riverNoise = amplitude * Mathf.PerlinNoise(seed + (g / freqX * 2), seed + (g / freqZ * 2));
            if (riverNoise <= 0.3f)
            {
                for (int i = 0; i < water1.Length; i++)
                {
                    uvs[i + k] = water1[i];
                }
            }

            if (groundNoise < 0.5f)
            {
                for (int i = 0; i < dirt1.Length; i++)
                {
                    uvs[i + k] = dirt1[i];
                }
            }
            else
            {
                for (int i = 0; i < dirt2.Length; i++)
                {
                    uvs[i + k] = dirt2[i];
                }
            }

            k += 4;
        }

        UpdateMesh();

        InstantiateEntities(chunkX, chunkZ);
    }

    private void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        mesh.RecalculateNormals();
    }

    private void InstantiateEntities(int chunkX, int chunkZ)
    {
        int posX = chunkX * chunkSize;
        int posZ = chunkZ * chunkSize;

        for (int x = posX; x < posX + chunkSize; x++)
        {
            for (int z = posZ; z < posZ + chunkSize; z++)
            {
                float entityFreq = 0.1f;
                float noise = amplitude * Mathf.PerlinNoise(seed + (x / freqX), seed + (z / freqZ));

                if (noise < entityFreq)
                {
                    if (noise < 0.001f)
                    {
                        GameObject go = Instantiate(prefabTree, new Vector3(x * cellSize, 0, z * cellSize), Quaternion.identity);
                        go.transform.parent = CategoryTrees.Transform;
                        World.WorldGrid[(worldSize / 2) + x, 0, (worldSize / 2) + z] = go;
                    }
                    else
                    {
                        GameObject go = Instantiate(prefabRock, new Vector3(x * cellSize, 0, z * cellSize), Quaternion.identity);
                        go.transform.parent = CategoryRocks.Transform;
                        World.WorldGrid[(worldSize / 2) + x, 0, (worldSize / 2) + z] = go;
                    }
                }
            }
        }
    }
}