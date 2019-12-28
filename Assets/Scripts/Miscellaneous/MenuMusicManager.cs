using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
