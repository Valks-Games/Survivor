using UnityEngine;

public class MenuMusicManager : MonoBehaviour
{
    public static GameObject MenuMusic;

    public void Awake()
    {
        if (MenuMusic == null)
        {
            MenuMusic = gameObject;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (MenuMusic != this)
            {
                Destroy(gameObject);
            }
        }
    }
}
