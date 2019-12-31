using UnityEngine;

public class Structure : WorldAPI.Structures.Structure
{
    private GameObject _world;
    public int Resource { get; private set; } = 3;
    public int Workers { get; set; } = 0;

    public virtual void Awake()
    {
        SetName("Structure");
    }

    public virtual void Start()
    {
        transform.parent = transform;
        _world = GameObject.Find("World");
    }

    public void SetParent(string parent) =>
        transform.parent = _world.transform.Find(parent);

    public void SetName(string name) =>
        transform.name = name;

    public int GatherResource(int toolPower, int colonistResource, int colonistInvCapacity)
    {
        int gathered = Mathf.Min(colonistResource + toolPower, colonistInvCapacity, Resource);
        Resource -= gathered;

       if (Resource < 1)
       {
            Destroy(transform.gameObject);
            Destroy();
       }

        return gathered;
    }

    public void Destroy() =>
        Destroy(gameObject);
}
