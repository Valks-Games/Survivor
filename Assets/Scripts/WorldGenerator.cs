using System.Collections;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    private GameObject _tree;
    private GameObject _colonist;
    private GameObject _rock;
    private GameObject _base;

    public GameObject[,] Grid;
    public int Columns = 10;
    public int Rows = 10;
    public bool GeneratingWorld = true;

    public void Awake()
    {
        _tree = Resources.Load("Prefabs/Tree") as GameObject;
        _colonist = Resources.Load("Prefabs/Colonist") as GameObject;
        _rock = Resources.Load("Prefabs/Rock") as GameObject;
        _base = Resources.Load("Prefabs/Base") as GameObject;
    }

    public void Start()
    {
        PopulateGrid();
        AddFactions();

        InstantiateGrid();

        GeneratingWorld = false;
    }

    private void PopulateGrid()
    {
        Grid = new GameObject[Columns, Rows];

        for (int i = 0; i < Columns; i++)
            for (int j = 0; j < Rows; j++)
                if (Mathf.PerlinNoise(Random.Range(0f, 1f), Random.Range(0f, 1f)) < 0.3f)
                    if (Random.Range(0f, 1f) < 0.45f)
                        Grid[i, j] = _tree;
                    else
                        Grid[i, j] = _rock;
    }

    private void AddFactions()
    {
        Grid[0, 0] = _base;
        for (int i = 0; i < 5; i++)
            Instantiate(_colonist, GetPoint(0, 0), Quaternion.identity);

        Grid[Rows - 1, Columns - 1] = _base;
    }

    private Vector2 GetPoint(int i, int j)
    {
        return new Vector2(i - Columns / 2, j - Rows / 2);
    }

    private void InstantiateGrid()
    {
        for (int i = 0; i < Columns; i++)
            for (int j = 0; j < Rows; j++)
                SpawnObject(Grid[i, j], i - Columns / 2, j - Rows / 2);
    }

    private void SpawnObject(GameObject theObject, int x, int y)
    {
        if (theObject != null)
            Instantiate(theObject, new Vector2(x, y), Quaternion.identity);
    }

    private IEnumerator WaitForWorldGeneration()
    {
        while (GeneratingWorld)
            yield return new WaitForSeconds(0.1f);
    }
}
