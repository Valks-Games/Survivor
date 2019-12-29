using UnityEngine;

public class Structure : MonoBehaviour
{
    private GameObject world;
    public int Resource { get; private set; } = 3;
    public string Type { get; set; } = "None";
    public int Workers { get; set; } = 0;
    public int Team { get; set; } = 0;

    public virtual void Awake()
    {
        SetName("Structure");
        SetTeam(1);
    }

    public virtual void Start()
    {
        transform.parent = transform;
        world = GameObject.Find("World");
    }

    public void SetParent(string parent)
    {
        transform.parent = world.transform.Find(parent);
        Type = parent;
    }

    public void SetName(string name)
    {
        transform.name = name;
    }

    public void SetTeam(int team)
    {
        Team = team;
    }

    public int GatherResource(int toolPower, int colonistResource, int colonistInvCapacity)
    {
        int gathered = toolPower >= Resource ? Resource : toolPower;
        gathered = Mathf.Min(colonistResource + gathered, colonistInvCapacity);

        Resource -= gathered;


        if (Resource <= 0) {
            AIEntity.StructureList[Type].Remove(this.transform);
            Destroy();
        }

        return gathered;
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}
