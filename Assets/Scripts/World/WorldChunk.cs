using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class WorldChunk : MonoBehaviour
{
    private Mesh mesh;
    private MeshRenderer meshRenderer;

    private Vector3[] vertices;
    private int[] triangles;

    private readonly float cellSize = 0.25f;
    private readonly int chunkSize = 11;

    private GameObject prefabTree;
    private GameObject prefabRock;

    public void Awake()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        meshRenderer = GetComponent<MeshRenderer>();
        prefabTree = WorldGenerator.prefabTree;
        prefabRock = WorldGenerator.prefabRock;
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

        UpdateMesh();

        InstantiateEntities(chunkX, chunkZ);
    }

    private void InstantiateEntities(int chunkX, int chunkZ)
    {
        int posX = chunkX * chunkSize;
        int posZ = chunkZ * chunkSize;

        for (int x = posX; x < posX + chunkSize; x++)
        {
            for (int z = posZ; z < posZ + chunkSize; z++)
            {
                if (Mathf.PerlinNoise(Random.Range(0f, 1f), Random.Range(0f, 1f)) < 0.3f)
                {
                    if (Random.Range(0f, 1f) < 0.5f)
                    {
                        GameObject go = Instantiate(prefabTree, new Vector3(x * cellSize, 0, z * cellSize), Quaternion.identity);
                        go.transform.parent = WorldGenerator.World.transform.Find("Trees");
                    }
                    else
                    {
                        GameObject go = Instantiate(prefabRock, new Vector3(x * cellSize, 0, z * cellSize), Quaternion.identity);
                        go.transform.parent = WorldGenerator.World.transform.Find("Rocks");
                    }
                }
            }
        }
    }

    private void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }
}