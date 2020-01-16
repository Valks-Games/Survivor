using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructurePlacer : MonoBehaviour
{
    private float cellSize;
    private int worldSize;

    private void Awake()
    {
        cellSize = World.CellSize;
        worldSize = World.WorldSize;
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
                Vector3 point = GetNearestPoint(ray.GetPoint(distanceToPlane), cellSize);
                Vector3 pos = ConvertPointToPosition(point, cellSize);

                GameObject check = World.WorldGrid[(worldSize / 2) + (int)pos.x, 0, (worldSize / 2) + (int)pos.z];
                if (!check)
                {
                    GameObject structure = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    structure.transform.position = point;
                    structure.transform.localScale = new Vector3(cellSize, cellSize, cellSize);

                    World.WorldGrid[(worldSize / 2) + (int)pos.x, 0, (worldSize / 2) + (int)pos.z] = structure;
                }
            }
        }
    }

    private Vector3 GetNearestPoint(Vector3 pos, float size)
    {
        return new Vector3(Mathf.RoundToInt(pos.x / size) * size, size / 2f, Mathf.RoundToInt(pos.z / size) * size);
    }

    private Vector3 ConvertPointToPosition(Vector3 pos, float size)
    {
        return pos / size;
    }
}
