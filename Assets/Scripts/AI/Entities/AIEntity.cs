using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public abstract class AIEntity : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] public int Health = 1;
    [SerializeField] public int Damage = 1;
    [SerializeField] public Faction Faction = null;
    [SerializeField] public Transform ClosestTarget;
    [SerializeField] public bool Searching = true;
    [SerializeField] public bool IsAtTarget;
    [SerializeField] public int AxePower = 10;
    [SerializeField] protected float speed = 100.0f;
    [SerializeField] protected float interactionRange = 0.5f;

    [Header("Resources")]
    [SerializeField] public int MaxInventorySize = 3;

    [SerializeField] public static Dictionary<string, List<Transform>> StructureList = new Dictionary<string, List<Transform>>(); 
    //public static List<Transform> structures = new List<Transform>();

    public string CurrentTask = "NONE";
    [SerializeField] public readonly Dictionary<Material, int> Inventory = new Dictionary<Material, int>();

    public AITask Task { get; private set; }

    protected GameObject world;
    protected Rigidbody rb;

    public Base Base => Faction.Base;

    public void Init(int health = 1, int damage = 1)
    {
        foreach (Material resource in System.Enum.GetValues(typeof(Material)))
            Inventory.Add(resource, 0);

        world = GameObject.Find("World");
        rb = GetComponent<Rigidbody>();

        if (StructureList.Count == 0)
        {
            StructureList.Add("Trees", RetrieveList("Trees"));
            StructureList.Add("Rocks", RetrieveList("Rocks"));
            StructureList.Add("Bases", RetrieveList("Bases"));
        }

        Task = new IdleTask(this);

        Health = health;
        Damage = damage;
    }

    public void InitFaction(Faction faction) {
        Faction = faction;
    }

    public void Update()
    {
        if (Task == null || Task.Target == null)
            return;

        Task.Update(StructureList[Task.Target]);
    }

    public void AssignTask(AITask task)
    {
        IsAtTarget = false;
        Searching = true;
        Task = task;
        CurrentTask = Task.Target;
    }

    public void JobAttack(List<Transform> targets)
    {

    }

    public void WalkTowardsTarget(Transform target)
    {
        Vector3 direction = target.position - transform.position;
        direction.Normalize();

        rb.drag = 1.0f;
        rb.AddForce(direction * speed * Time.deltaTime);
    }

    public bool AtTarget(Transform target)
    {
        rb.drag = 1.6f;
        return (target.position - transform.position).sqrMagnitude < interactionRange * interactionRange;
    }

    private List<Transform> RetrieveList(string type)
    {
        List<Transform> list = new List<Transform>();

        foreach (Transform child in world.transform.Find(type))
        {
            list.Add(child);
        }

        return list;
    }

    private Transform FindClosestTeammate(List<Transform> targets) //Might be removed soon 
    {
        Transform closestTransform = null;
        float minDist = Mathf.Infinity;
        Vector3 currentPos = transform.position;
        foreach (Transform t in targets)
        {
            if (t.gameObject.GetComponent<AIEntity>().Faction == Faction)
            {
                float dist = Vector3.Distance(t.position, currentPos);
                if (dist < minDist)
                {
                    closestTransform = t;
                    minDist = dist;
                }
            }
        }

        return closestTransform;
    }

    public Transform GetClosestObject(string type)
    {
        return StructureList[type].OrderBy(t => (t.position - this.transform.position).sqrMagnitude).Where(t => t.gameObject.gameObject.GetComponent<Structure>().Workers == 0).FirstOrDefault();
    }

}
