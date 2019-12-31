using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class WorldChunk : MonoBehaviour
{
    private Mesh _mesh;
    private MeshRenderer _meshRenderer;

    private Vector3[] _vertices;
    private int[] _triangles;

    public void Awake()
    {
        _mesh = GetComponent<MeshFilter>().mesh;
        _meshRenderer = GetComponent<MeshRenderer>();
        
        transform.Rotate(new Vector3(-90, 0, 0));
    }

    public void Start()
    {
        _meshRenderer.material = Resources.Load("Materials/Sprite-Diffuse") as UnityEngine.Material;
    }

    public void Generate(int x, int z)
    {
        float cellSize = 0.25f;
        int chunkSize = 11;

        Vector3 renderOffset = new Vector3(x * chunkSize * cellSize, 0, z * chunkSize * cellSize);

        // Discrete Procedural Grid (2 vertices are never shared)
        _vertices = new Vector3[chunkSize * chunkSize * 4];
        _triangles = new int[chunkSize * chunkSize * 6];

        int v = 0;
        int t = 0;

        float vertexOffset = cellSize * 0.5f; // Multiplication is not as intensive as division.

        for (int cellX = 0; cellX < chunkSize; cellX++)
        {
            for (int cellZ = 0; cellZ < chunkSize; cellZ++)
            {
                Vector3 cellOffset = new Vector3(cellX * cellSize, 0, cellZ * cellSize);

                _vertices[v] = new Vector3(-vertexOffset, 0, -vertexOffset) + cellOffset + renderOffset;
                _vertices[v + 1] = new Vector3(-vertexOffset, 0, vertexOffset) + cellOffset + renderOffset;
                _vertices[v + 2] = new Vector3(vertexOffset, 0, -vertexOffset) + cellOffset + renderOffset;
                _vertices[v + 3] = new Vector3(vertexOffset, 0, vertexOffset) + cellOffset + renderOffset;

                _triangles[t] = v;
                _triangles[t + 1] = _triangles[t + 4] = v + 1;
                _triangles[t + 2] = _triangles[t + 3] = v + 2;
                _triangles[t + 5] = v + 3;

                v += 4;
                t += 6;
            }
        }

        UpdateMesh();
    }

    private void UpdateMesh()
    {
        _mesh.Clear();
        _mesh.vertices = _vertices;
        _mesh.triangles = _triangles;
        _mesh.RecalculateNormals();
    }
}