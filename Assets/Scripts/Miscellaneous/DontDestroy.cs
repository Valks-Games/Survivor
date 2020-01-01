using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroy : MonoBehaviour
{
    public static GameObject go;

    public void Awake()
    {
        if (go == null)
        {
            go = gameObject;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (go != this)
            {
                Destroy(gameObject);
            }
        }
    }
}
