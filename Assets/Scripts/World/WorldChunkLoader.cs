using System.Collections;
using UnityEngine;

public class WorldChunkLoader : MonoBehaviour
{
    private WorldGenerator world;
    private int chunkSize;
    private float cellSize;

    public static bool Started = false;
    private readonly bool checkChunks = true;

    private Transform cameraTransform;

    public void Awake()
    {
        world = GetComponent<WorldGenerator>();
        chunkSize = WorldGenerator.ChunkSize;
        cellSize = WorldGenerator.CellSize;
        cameraTransform = Camera.main.transform;
    }

    private GameObject GetChunk(int x, int z)
    {
        return GameObject.Find("Chunk " + x + " " + z);
    }

    private void LoadChunk(int posX, int posZ, int offsetX, int offsetZ)
    {
        if (!GetChunk(posX + offsetX, posZ + offsetZ))
        {
            GameObject chunk = new GameObject("Chunk " + (posX + offsetX) + " " + (posZ + offsetZ));
            chunk.transform.parent = gameObject.transform;

            WorldChunk renderChunk = chunk.AddComponent<WorldChunk>();
            renderChunk.Generate(posX + offsetX, posZ + offsetZ);
        }
    }

    private void UnloadChunk(int posX, int posZ, int offsetX, int offsetZ)
    {
        GameObject chunk = GetChunk(posX + offsetX, posZ + offsetZ);
        if (chunk)
        {
            Destroy(chunk);
        }
    }

    private IEnumerator UnloadChunks(float dist)
    {
        while (checkChunks)
        {
            int childCount = transform.childCount;

            for (int i = 0; i < childCount; i++)
            {
                Transform chunk = transform.GetChild(i);

                Vector3 vecToTarget = cameraTransform.position - chunk.position;
                vecToTarget.y = 0;

                Debug.Log(vecToTarget.magnitude);

                if (vecToTarget.magnitude >= dist)
                {
                    Destroy(chunk.gameObject);
                }
            }

            yield return new WaitForSeconds(1f);
        }
    }

    private IEnumerator LoadChunks(int dist)
    {
        while (checkChunks)
        {
            Vector3 position = cameraTransform.position;
            int posX = (int)((position.x / (chunkSize * cellSize)));
            int posZ = (int)((position.z / (chunkSize * cellSize)));

            for (int x = -dist; x < dist; x++)
            {
                for (int z = -dist; z < dist; z++)
                {
                    LoadChunk(posX, posZ, x, z);
                }
            }

            yield return new WaitForSeconds(0.2f);
        }
    }

    public void Update()
    {
        if (Started)
        {
            Started = false;
            StartCoroutine(LoadChunks(7));
            //StartCoroutine(UnloadChunks((chunkSize * cellSize * 3) - 0.5f));
        }
    }
}