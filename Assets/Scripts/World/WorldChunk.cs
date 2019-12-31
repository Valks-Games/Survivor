using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class WorldChunk : MonoBehaviour
{
    private float _cellSize = 0.25f;
    private int _chunkSize = 11;
    private Vector3 _gridOffset;

    private Mesh _mesh;

    private Vector3[] _vertices;
    private int[] _triangles;

    private void Awake()
    {
        _gridOffset = new Vector3(-_chunkSize * _cellSize / 2, 0, -_chunkSize * _cellSize / 2);
        _mesh = GetComponent<MeshFilter>().mesh;
    }

    public void GenerateChunk()
    {
        // Discrete Procedural Grid meaning 2 vertices are never shared.
        _vertices = new Vector3[_chunkSize * _chunkSize * 4];
        _triangles = new int[_chunkSize * _chunkSize * 6];

        int v = 0;
        int t = 0;

        float vertexOffset = _cellSize * 0.5f; // Multiplication is not as intensive as division.

        for (int x = 0; x < _chunkSize; x++)
        {
            for (int z = 0; z < _chunkSize; z++)
            {
                Vector3 cellOffset = new Vector3(x * _cellSize, 0, z * _cellSize);

                _vertices[v] = new Vector3(-vertexOffset, 0, -vertexOffset) + cellOffset + _gridOffset;
                _vertices[v + 1] = new Vector3(-vertexOffset, 0, vertexOffset) + cellOffset + _gridOffset;
                _vertices[v + 2] = new Vector3(vertexOffset, 0, -vertexOffset) + cellOffset + _gridOffset;
                _vertices[v + 3] = new Vector3(vertexOffset, 0, vertexOffset) + cellOffset + _gridOffset;

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