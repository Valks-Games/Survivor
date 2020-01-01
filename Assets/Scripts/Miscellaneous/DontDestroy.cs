using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroy : MonoBehaviour
{
    public static GameObject Go; // <-- this should eventually be a list of some sorts to make it useful for more then one gameobject

    public void Awake()
    {
        if (Go == null)
        {
            Go = gameObject;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (Go != this)
            {
                Destroy(gameObject);
            }
        }
    }
}
