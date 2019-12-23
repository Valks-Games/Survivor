using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    public GameObject Tree;
    public GameObject Colonist;
    public GameObject Base;

    public GameObject[,] Grid;
    public int Columns = 10;
    public int Rows = 10;

    private void Start()
    {
        SetupGrid();

        for (int i = 0; i < 10; i++) {
            Instantiate(Colonist);
        }
    }

    private void SetupGrid() {
        PopulateGrid();
        InstantiateGrid();
    }

    private void PopulateGrid() {
        Grid = new GameObject[Columns, Rows];

        for (int i = 0; i < Columns; i++)
        {
            for (int j = 0; j < Rows; j++)
            {
                if (Mathf.PerlinNoise(Random.Range(0f, 1f), Random.Range(0f, 1f)) < 0.3f)
                {
                    Grid[i, j] = Tree;
                }
            }
        }

        Grid[Columns / 2, Rows / 2] = Base;
    }

    private void InstantiateGrid() {
        for (int i = 0; i < Columns; i++)
        {
            for (int j = 0; j < Rows; j++)
            {
                SpawnObject(Grid[i, j], i - Columns / 2, j - Rows / 2);
            }
        }
    }

    private void SpawnObject(GameObject theObject, int x, int y) {
        if (theObject != null) {
            var go = Instantiate(theObject, new Vector2(x, y), Quaternion.identity);
            go.transform.parent = transform;
        }
        
    }
}
