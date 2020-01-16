using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructurePlacer : MonoBehaviour
{
    private float cellSize;

    private void Awake()
    {
        cellSize = WorldGenerator.CellSize;
    }

    private void Start()
    {

    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            Plane rayPlane = new Plane(new Vector3(0, 0.1f, 0), 0f);

            float distanceToPlane;

            if (rayPlane.Raycast(ray, out distanceToPlane))
            {
                GameObject structure = GameObject.CreatePrimitive(PrimitiveType.Cube);
                structure.transform.position = GetNearestPoint(ray.GetPoint(distanceToPlane), cellSize);
                structure.transform.localScale = new Vector3(cellSize, cellSize, cellSize);
            }
        }
    }

    private Vector3 GetNearestPoint(Vector3 pos, float size)
    {
        float xCount = Mathf.RoundToInt(pos.x / size) * size;
        float zCount = Mathf.RoundToInt(pos.z / size) * size;

        return new Vector3(xCount, size / 2f, zCount);
    }
}
