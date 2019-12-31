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
    [SerializeField] protected float _speed = 100.0f;
    [SerializeField] protected float _interactionRange = 0.5f;

    [Header("Resources")]
    [SerializeField] public int MaxInventorySize = 3;

    [SerializeField]

    public static Dictionary<string, List<Transform>> StructureList = new Dictionary<string, List<Transform>>(); 
    //public static List<Transform> structures = new List<Transform>();

    public string CurrentTask = "NONE";
    [SerializeField] public readonly Dictionary<Material, int> Inventory = new Dictionary<Material, int>();

    public AITask Task { get; private set; }

    protected GameObject _world;
    protected Rigidbody2D _rb;

    public Base Base => Faction.Base;

    public void Init(int health = 1, int damage = 1)
    {


        foreach (Material resource in System.Enum.GetValues(typeof(Material)))
            Inventory.Add(resource, 0);

        _world = GameObject.Find("World");
        _rb = GetComponent<Rigidbody2D>();

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
        Vector2 direction = target.position - transform.position;
        direction.Normalize();

        _rb.drag = 1.0f;
        _rb.AddForce(direction * _speed * Time.deltaTime);
    }

    public bool AtTarget(Transform target)
    {
        _rb.drag = 1.6f;
        return (target.position - this.transform.position).sqrMagnitude < _interactionRange * _interactionRange;
    }

    private List<Transform> RetrieveList(string type)
    {
        List<Transform> list = new List<Transform>();

        foreach (Transform child in _world.transform.Find(type))
        {
            list.Add(child);
        }

        return list;
    }

    private Transform FindClosestTeammate(List<Transform> targets) //Might be removed soon 
    {
        Transform closestTransform = null;
        float minDist = Mathf.Infinity;
        Vector2 currentPos = transform.position;
        foreach (Transform t in targets)
        {
            if (t.gameObject.GetComponent<AIEntity>().Faction == Faction)
            {
                float dist = Vector2.Distance(t.position, currentPos);
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
