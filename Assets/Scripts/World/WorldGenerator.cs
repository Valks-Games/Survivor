using System.Collections;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    [HideInInspector] public static GameObject prefabTree { get; private set; }
    [HideInInspector] public static GameObject prefabColonist { get; private set; }
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
        prefabColonist = Resources.Load("Prefabs/Colonist") as GameObject;
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
        foreach (Vector2 point in PoissonDiscSampling.Generate(minDistance, new Vector2(Columns, Rows)))
            AddFaction(new Vector2((int)point.y, (int)point.x));
    }

    private void AddFaction(Vector2 pos)
    {
        Faction faction = new Faction("Faction!")
        {
            Base = Instantiate(prefabBase, new Vector3(pos.x, 0, pos.y), Quaternion.identity).GetComponent<Base>()
        };

        for (int i = 0; i < 5; i++)
            Instantiate(prefabColonist, new Vector3(pos.x, 1, pos.y), Quaternion.identity).SendMessage("InitFaction", faction);
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
