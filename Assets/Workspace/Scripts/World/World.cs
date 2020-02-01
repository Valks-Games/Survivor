using GameAPI.Editor.Attributes;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{
    public static Dictionary<string, List<Transform>> StructureList = new Dictionary<string, List<Transform>>();

    public static GameObject prefabTree { get; private set; }
    public static GameObject prefabRock { get; private set; }
    public static GameObject prefabBase { get; private set; }

    public static bool GeneratingWorld = true;

    public const int WorldSize = 10000;
    public const float BaseMinimumDistance = 32;

    public static Category CategoryChunks;
    public static Category CategoryColonists;

    // Hardcoded Settings
    public static int ChunkSize = 40;

    public static float CellSize = 0.25f;

    // Seed + Settings
    public static string StringSeed = "";

    public static int Seed;
    public static float FreqX = 0.15f;
    public static float FreqZ = 0.15f;
    public static float Amplitude = 2f;

    [Disabled] public int SeedPreview;
    public static bool RandomizeSeed = true;

    [Header("Settings")]
    public float SetAmplitude;

    public float SetFreqX;
    public float SetFreqZ;

    public void Awake()
    {
        FreqX = SetFreqX;
        FreqZ = SetFreqX;
        Amplitude = SetAmplitude;

        if (StringSeed != "" || StringSeed != null)
        {
            // Hash too long and will not work if not divided by at least 100.
            Seed = Mathf.Abs(StringSeed.GetHashCode() / 100000);
        }

        if (RandomizeSeed)
        {
            Seed = Random.Range(0, 99999);
        }

        Random.InitState(Seed);

        SeedPreview = Seed;

        prefabTree = Resources.Load("Prefabs/Tree") as GameObject;
        prefabRock = Resources.Load("Prefabs/Rock") as GameObject;
        prefabBase = Resources.Load("Prefabs/Base") as GameObject;

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
        AddFaction(new Vector3(0, 0, 0)); // Temp
        //AddFactions(BaseMinimumDistance);

        GeneratingWorld = false;

        // Start checking to see if new chunks need to be created or removed.
        WorldChunkLoader.Started = true;
    }

    private List<Transform> RetrieveList(string type)
    {
        List<Transform> list = new List<Transform>();

        Transform parent = CategoryChunks.Transform.Find("Chunk 0 0").Find("Structures").Find(type);

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
        foreach (Vector3 point in PoissonDiscSampling.Generate(minDistance, new Vector2(WorldSize, WorldSize)))
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

    public override string ToString()
    {
        return "World Size: " + WorldSize + ", Chunk Size: " + ChunkSize + ", Cell Size: " + CellSize;
    }
}