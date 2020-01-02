using System.Collections;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    [HideInInspector] public static GameObject prefabTree { get; private set; }
    [HideInInspector] public static GameObject prefabRock { get; private set; }
    [HideInInspector] public static GameObject prefabBase { get; private set; }

    [HideInInspector] public GameObject[,] Grid;
    [HideInInspector] public bool GeneratingWorld = true;

    [HideInInspector] public const int Columns = 100;
    [HideInInspector] public const int Rows = 100;
    [HideInInspector] public const float BaseMinimumDistance = 32;

    public void Awake()
    {
        prefabTree = Resources.Load("Prefabs/Tree") as GameObject;
        prefabRock = Resources.Load("Prefabs/Rock") as GameObject;
        prefabBase = Resources.Load("Prefabs/Base") as GameObject;
    }

    public void Start()
    {
        GenerateSpawn(1);
        AddFaction(GetPoint(Rows / 2, Columns / 2)); // Temp
        //AddFactions(BaseMinimumDistance);
        GeneratingWorld = false;
    }

    public void GenerateSpawn(int size)
    {
        for (int x = -size; x < size; x++)
        {
            for (int z = -size; z < size; z++)
            {
                GameObject goChunk = new GameObject("Chunk " + x + " " + z);
                WorldChunk worldChunk = goChunk.AddComponent<WorldChunk>();
                worldChunk.Generate(x, z);
            }
        }
    }

    private void AddFactions(float minDistance)
    {
        foreach (Vector3 point in PoissonDiscSampling.Generate(minDistance, new Vector2(Columns, Rows)))
            AddFaction(point);
    }

    private void AddFaction(Vector3 pos)
    {
        Faction faction = new Faction("Faction!")
        {
            Base = Instantiate(prefabBase, pos, Quaternion.identity).GetComponent<Base>()
        };

        for (int i = 0; i < 5; i++)
            Colonist.New(
                location: pos,
                faction: faction
            );
    }

    private Vector2 GetPoint(int i, int j)
    {
        return new Vector2(i - Columns / 2, j - Rows / 2);
    }

    private IEnumerator WaitForWorldGeneration()
    {
        while (GeneratingWorld)
            yield return new WaitForSeconds(0.05f);
    }
}
