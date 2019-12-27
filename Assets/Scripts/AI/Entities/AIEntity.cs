using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        Task = new IdleTask(this);

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

        Task.Update(RetrieveList(Task.Target));
    }

    public void AssignTask(AITask task)
    {
        IsAtTarget = false;
        Searching = true;
        Task = task;
    }

    public void JobAttack(List<Transform> targets)
    {

    }

    public void JobStructure(List<Transform> targets, string type, IEnumerator task)
    {
        if (IsAtTarget)
            return;

        if (Searching)
        {
            UpdateList(targets, type);
            ClosestTarget = GetClosestStructure(targets);

            if (ClosestTarget != null)
            {
                ClosestTarget.GetComponent<Structure>().Workers += 1;
                Searching = false;
            }
        }

        if (ClosestTarget == null)
        {
            Searching = true;
        }
        else
        {
            WalkTowardsTarget(ClosestTarget);

            if (AtTarget(ClosestTarget))
            {
                IsAtTarget = true;
                Searching = true;
                StartCoroutine(task);
            }
        }
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

    public void UpdateList(List<Transform> list, string type)
    {
        list.Clear();
        foreach (Transform child in _world.transform.Find(type))
        {
            list.Add(child);
        }
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

    public Transform GetClosestStructure(List<Transform> targets)
    {
        Transform closestTransform = null;
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

        return closestTransform;
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
