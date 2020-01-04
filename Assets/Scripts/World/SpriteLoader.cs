using System.Collections.Generic;
using UnityEngine;

public class SpriteLoader : MonoBehaviour
{
    private static Dictionary<string, Vector2[]> tileUVMap;

    public void Awake()
    {
        tileUVMap = new Dictionary<string, Vector2[]>();

        Sprite[] sprites = Resources.LoadAll<Sprite>("Sprites/Tiles");

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
            uvs[1] = new Vector2(s.rect.x / imageWidth, (s.rect.y + s.rect.height) / imageHeight);
            uvs[2] = new Vector2((s.rect.x + s.rect.width) / imageWidth, s.rect.y / imageHeight);
            uvs[3] = new Vector2((s.rect.x + s.rect.width) / imageWidth, (s.rect.y + s.rect.height) / imageHeight);

            tileUVMap.Add(s.name, uvs);
        }
    }

    public static Vector2[] GetTileUVs(string key)
    {
        if (tileUVMap.ContainsKey(key))
        {
            return tileUVMap[key];
        }
        else
        {
            Debug.Log("There is no UV map for tile type: " + key);
            return tileUVMap["Void"];
        }
    }
}
