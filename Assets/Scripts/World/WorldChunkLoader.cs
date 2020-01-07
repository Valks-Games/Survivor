using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldChunkLoader : MonoBehaviour
{
    private WorldGenerator world;
    private int chunkSize;

    public static bool Started = false;
    private bool checkChunks = true;

    private Transform cameraTransform;

    public void Awake()
    {
        world = GetComponent<WorldGenerator>();
        chunkSize = WorldGenerator.ChunkSize;
        cameraTransform = Camera.main.transform;
    }

    private GameObject GetChunk(int x, int z) {
        return GameObject.Find("Chunk " + x + " " + z);
    }

    private void LoadChunk(int posX, int posZ, int offsetX, int offsetZ) {
        //Debug.Log("X: " + (posX + offsetX) + ", Z: " + (posZ + offsetZ));
        if (!GetChunk(posX + offsetX, posZ + offsetZ))
        {
            GameObject chunk = new GameObject("Chunk " + (posX + offsetX) + " " + (posZ + offsetZ));
            chunk.transform.parent = gameObject.transform;
            //chunk.transform.position = new Vector3((posX + offsetX) * chunkSize, 0, (posZ + offsetZ) * chunkSize);

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

    private IEnumerator UnloadChunks(int dist)
    {
        while (checkChunks)
        {
            int childCount = transform.childCount;

            for (int i = 0; i < childCount; i++) {
                Transform chunk = transform.GetChild(i);
                
                if (Vector3.Distance(cameraTransform.position, chunk.position) >= dist) {
                    Destroy(chunk.gameObject);
                }
            }

            yield return new WaitForSeconds(1f);
        }
    }

    private IEnumerator LoadChunks() {
        while (checkChunks)
        {
            Vector3 position = cameraTransform.position;
            int posX = (int)position.x / (int) (WorldGenerator.ChunkSize * WorldGenerator.CellSize);
            int posZ = (int)position.z / (int) (WorldGenerator.ChunkSize * WorldGenerator.CellSize);

            //Debug.Log("X: " + posX + ", Z: " + posZ);

            LoadChunk(posX, posZ, 0, 0);
            LoadChunk(posX, posZ, 1, 0);
            LoadChunk(posX, posZ, 0, 1);
            LoadChunk(posX, posZ, -1, 0);
            LoadChunk(posX, posZ, 0, -1);
            LoadChunk(posX, posZ, 1, 1);
            LoadChunk(posX, posZ, -1, -1);
            LoadChunk(posX, posZ, -1, 1);
            LoadChunk(posX, posZ, 1, -1);

            yield return new WaitForSeconds(1f);
        }
    }

    public void Update()
    {
        if (Started) {
            Started = false;
            StartCoroutine(LoadChunks());
            //StartCoroutine(UnloadChunks(chunkSize * 3));
        }
    }
}
