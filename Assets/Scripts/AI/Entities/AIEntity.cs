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

        /*if (structures.Count == 0)
        {
            //structures.AddRange(RetrieveList("Bases"));
            structures.AddRange(RetrieveList("Trees"));
            structures.AddRange(RetrieveList("Rocks"));
        }*/

        Task = new GatherResourceTask(this, Material.Stone);

        Health = health;
        Damage = damage;
    }

    public void InitFaction(Faction faction) {
        Faction = faction;
    }

    public void Update()
    {
        if (Task == null || Task.Target == null || RetrieveList(Task.Target) == null)
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
        return Vector2.Distance(transform.position, target.position) < _interactionRange;
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

    private Transform FindClosestTeammate(List<Transform> targets)
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
        
        //Debug.Log(Base.transform);
        //return GetClosestStructure(type, StructureList[type]);
        return StructureList[type].Where(t => t.gameObject.gameObject.GetComponent<Structure>().Workers == 0).FirstOrDefault();
    }

    private Transform GetClosest(string type, List<Transform> targets)
    {

        IOrderedEnumerable<Transform> targetSorted = targets.OrderBy(t => Vector2.Distance(this.transform.position, t.position)/*(t.position - this.transform.position).sqrMagnitude*/);
        //Debug.Log(targetSorted.ToArray().ToString());
        if (type != "Bases") return targets.Where(t => t.gameObject.GetComponent<Structure>().Workers == 0).First();
        return targetSorted.First();

        //return targets[0];
    }

    private Transform GetClosestStructure(string type, List<Transform> targets)
    {

        Debug.Log("Checking for " + type);



        if (type == "Bases")
        {

            foreach (Transform t in targets)
            {       
                if (t.position == Base.transform.position) return t;
            }
            
            //if (targets[0] == null) Debug.Log("it's null");
            //return targets[0];

            return null;
            
            //Transform e = targets.Where(t => t.position == Base.transform.position).FirstOrDefault();
            //StartCoroutine(debug(targets));
            //Transform e = targets.OrderBy(t => (t.position - transform.position).sqrMagnitude).FirstOrDefault();
            //Transform e = targets[0];

            //return e;

        } else 
        {
            List<Transform> newTargets = targets.OrderBy(t => (t.position - transform.position).sqrMagnitude).ToList();
            foreach (Transform t in newTargets)
            {
                Debug.Log("Starting process");
                if (t.gameObject.GetComponent<Structure>().Workers == 0) {
                    Debug.Log("Found one mate");   
                    return t;
                }
            }
            //Transform e = targets.Where(t => t.gameObject.GetComponent<Structure>().Workers == 0 /*t.Equals(Base.transform)*/).OrderBy(t => (t.position - transform.position).sqrMagnitude).FirstOrDefault();
            //Debug.Log(e);
            return null;
        }

        
        /*Transform closestTransform = null;
        float minDist = Mathf.Infinity;
        Vector2 currentPos = transform.position;

        foreach (Transform t in targets)
        {
            if (t.gameObject.GetComponent<Structure>().Workers >= 1 && !t.CompareTag("Base"))
            {
                continue;
            }

            float dist = Vector2.Distance(t.position, currentPos);
            if (dist < minDist)
            {
                closestTransform = t;
                minDist = dist;
            }
        }

        return closestTransform;*/
    }
    
    private IEnumerator debug(List<Transform> targets)
    {
        Debug.Log(targets.Count);
        yield return null;
    }

    private Transform GetClosestObject(System.Func<GameObject, bool> condition, List<Transform> targets)
    {
        Transform closestTransform = null;
        float minDist = Mathf.Infinity;
        Vector2 currentPos = transform.position;

        foreach (Transform t in targets)
        {
            if (condition(t.gameObject))
            {
                float dist = Vector2.Distance(t.position, currentPos);
                if (dist < minDist)
                {
                    closestTransform = t;
                    minDist = dist;
                };
            }
        }

        return closestTransform;
    }
}
