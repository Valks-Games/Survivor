using System.Collections.Generic;
using UnityEngine;

public class SpriteLoader : MonoBehaviour
{
    public static SpriteLoader Instance;
    public Dictionary<string, Vector2[]> TileUVMap;

    public void Awake()
    {
        Instance = this;

        TileUVMap = new Dictionary<string, Vector2[]>();

        Sprite[] sprites = Resources.LoadAll<Sprite>("Sprites");

        float imageWidth = 0;
        float imageHeight = 0;

        foreach (Sprite s in sprites)
        {
            if (s.rect.x + s.rect.width > imageWidth)
                imageWidth = s.rect.x + s.rect.width;

            if (s.rect.y + s.rect.height > imageHeight)
                imageHeight = s.rect.y + s.rect.height;
        }

        foreach (Sprite s in sprites)
        {
            Vector2[] uvs = new Vector2[4];

            uvs[0] = new Vector2(s.rect.x / imageWidth, s.rect.y / imageHeight);
            uvs[1] = new Vector2((s.rect.x + s.rect.width) / imageWidth, s.rect.y / imageHeight);
            uvs[2] = new Vector2(s.rect.x / imageWidth, (s.rect.y + s.rect.height) / imageHeight);
            uvs[3] = new Vector2((s.rect.x + s.rect.width) / imageWidth, (s.rect.y + s.rect.height) / imageHeight);

            TileUVMap.Add(s.name, uvs);
        }
    }

    public Vector2[] GetTileUVs(string tile)
    {
        string key = tile;

        if (TileUVMap.ContainsKey(key))
        {
            return TileUVMap[key];
        }
        else
        {
            Debug.LogError("There is no UV map for tile type: " + key);
            //TODO: return void (black + pink checkerbox)
            return null;
        }
    }
}
