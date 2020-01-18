using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructurePlacer : MonoBehaviour
{
    public LayerMask LayerStructures;
    private float cellSize;
    private Plane rayPlane;

    private void Awake()
    {
        cellSize = World.CellSize;
        rayPlane = new Plane(new Vector3(0, 0.1f, 0), 0f);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity, LayerStructures))
            {
                // Structure exists here.
                // Debug.Log(hit.transform.name);
            }
            else
            {
                // Structure does not exist here.
                // Lets create one.
                float distanceToPlane;

                if (rayPlane.Raycast(ray, out distanceToPlane))
                {
                    Vector3 point = GetNearestPoint(ray.GetPoint(distanceToPlane), cellSize);
                    Vector3 pos = ConvertPointToPosition(point, cellSize);

                    GameObject structure = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    structure.transform.position = point;
                    structure.transform.localScale = new Vector3(cellSize, cellSize, cellSize);
                    structure.layer = LayerMask.NameToLayer("Structures");
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
