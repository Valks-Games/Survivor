using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    [HideInInspector] public static Dictionary<string, List<Transform>> StructureList = new Dictionary<string, List<Transform>>();
    [HideInInspector] public static GameObject World;

    [HideInInspector] public static GameObject prefabTree { get; private set; }
    [HideInInspector] public static GameObject prefabRock { get; private set; }
    [HideInInspector] public static GameObject prefabBase { get; private set; }

    [HideInInspector] public GameObject[,] Grid;
    [HideInInspector] public static bool GeneratingWorld = true;

    [HideInInspector] public const int Columns = 100;
    [HideInInspector] public const int Rows = 100;
    [HideInInspector] public const float BaseMinimumDistance = 32;

    public static int ChunkSize = 40;
    public static float CellSize = 0.25f;

    public static Category CategoryChunks;
    public static Category CategoryColonists;

    public string StringSeed = "";
    public bool RandomizeSeed;

    public float SetFreqX;
    public float SetFreqZ;
    public float SetAmplitude;
    [ReadOnly] public int ViewSeed;

    public static int Seed;
    public static float FreqX = 0.15f;
    public static float FreqZ = 0.15f;
    public static float Amplitude = 2f;

    public void Awake()
    {
        FreqX = SetFreqX;
        FreqZ = SetFreqX;
        Amplitude = SetAmplitude;

        if (StringSeed != "" || StringSeed != null)
        {
            // Hash too long and will not work if not divided by 100.
            Seed = StringSeed.GetHashCode() / 100;
        }

        if (RandomizeSeed)
        {
            Seed = Random.Range(0, 99999);
        }

        Random.InitState(Seed);

        ViewSeed = Seed;

        prefabTree = Resources.Load("Prefabs/Tree") as GameObject;
        prefabRock = Resources.Load("Prefabs/Rock") as GameObject;
        prefabBase = Resources.Load("Prefabs/Base") as GameObject;
        World = gameObject;

        CategoryChunks = new Category("Chunks", transform);
        CategoryColonists = new Category("Colonists", transform);
    }

    public void Start()
    {
        GenerateSpawn(1);

        if (StructureList.Count == 0)
        {
            StructureList.Add("Trees", RetrieveList("Trees"));
            StructureList.Add("Rocks", RetrieveList("Rocks"));
            StructureList.Add("Bases", RetrieveList("Bases"));
        }

        // Generate spawn with resources first otherwise colonists will complain that they can't find resources.
        AddFaction(GetPoint(Rows / 2, Columns / 2)); // Temp
        //AddFactions(BaseMinimumDistance);

        GeneratingWorld = false;
        WorldChunkLoader.Started = true;
    }

    private List<Transform> RetrieveList(string type)
    {
        List<Transform> list = new List<Transform>();

        Transform parent = WorldGenerator.CategoryChunks.Transform.Find("Chunk 0 0").Find("Structures").Find(type);

        foreach (Transform child in parent)
            list.Add(child);

        return list;
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
}