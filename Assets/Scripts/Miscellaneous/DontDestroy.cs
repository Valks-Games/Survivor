using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class DontDestroy : MonoBehaviour
{
    public static List<GameObject> DontDestroyObjects = new List<GameObject>(); // <-- this should eventually be a list of some sorts to make it useful for more then one gameobject

    public void Awake()
    {
        if (DontDestroyObjects.Count == 0)
        {
            AddSelf();
            return;
        }

        GameObject foundObject = DontDestroyObjects.Where(t => t.name == gameObject.name).First();
        
        if (foundObject != null)
        {
            Destroy(this);
        } else {
            AddSelf();
        }

    }

    private void AddSelf()
    {
        DontDestroyObjects.Add(gameObject);
        DontDestroyOnLoad(gameObject);
    }
}
