using System.Collections;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    private GameObject _tree;
    private GameObject _colonist;
    private GameObject _rock;
    private GameObject _base;

    public GameObject[,] Grid;
    public bool GeneratingWorld = true;

    public const int Columns = 100;
    public const int Rows = 100;
    public const float BaseMinimumDistance = 32;

    // World

    public void Awake()
    {
        _tree = Resources.Load("Prefabs/Tree") as GameObject;
        _colonist = Resources.Load("Prefabs/Colonist") as GameObject;
        _rock = Resources.Load("Prefabs/Rock") as GameObject;
        _base = Resources.Load("Prefabs/Base") as GameObject;

        // World
    }

    public void Start()
    {
        PopulateGrid();
        InstantiateGrid();

        AddFactions(BaseMinimumDistance);

        // World
        GenerateSpawn(4);

        GeneratingWorld = false;
    }

    public void GenerateSpawn(int size)
    {
        for (int x = -size/2; x < size/2; x++)
        {
            for (int z = -size/2; z < size/2; z++)
            {
                GameObject goChunk = new GameObject("Chunk " + x + " " + z);
                WorldChunk worldChunk = goChunk.AddComponent<WorldChunk>();
                worldChunk.Generate(x, z);
            }
        }
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

    private void AddFactions(float minDistance)
    {
        AddFaction(GetPoint(Rows / 2, Columns / 2));

        //foreach (Vector2 point in PoissonDiscSampling.Generate(minDistance, new Vector2(Columns, Rows)))
        //    AddFaction(new Vector2((int)point.y, (int)point.x));
    }

    private void AddFaction(Vector2 pos)
    {
        Faction faction = new Faction("Faction!")
        {
            Base = Instantiate(_base, new Vector3(pos.x, 0, pos.y), Quaternion.identity).GetComponent<Base>()
        };

        for (int i = 0; i < 5; i++)
            Instantiate(_colonist, new Vector3(pos.x, 0.15f, pos.y), Quaternion.identity).SendMessage("InitFaction", faction);
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
         {
            Instantiate(theObject, new Vector3(x, 0, y), Quaternion.identity);
         }
    }

    private IEnumerator WaitForWorldGeneration()
    {
        while (GeneratingWorld)
            yield return new WaitForSeconds(0.05f);
    }
}
