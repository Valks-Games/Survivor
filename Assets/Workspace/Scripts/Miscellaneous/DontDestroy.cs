using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DontDestroy : MonoBehaviour
{
    public static List<GameObject> DontDestroyObjects = new List<GameObject>();

    public void Awake()
    {
        if (DontDestroyObjects.Count == 0)
        {
            AddSelf();
            return;
        }

        GameObject foundObject = DontDestroyObjects.Where(t => t.name == gameObject.name).First();

        if (foundObject != null)
            Destroy(this);
        else
            AddSelf();
    }

    private void AddSelf()
    {
        DontDestroyObjects.Add(gameObject);
        DontDestroyOnLoad(gameObject);
    }
}