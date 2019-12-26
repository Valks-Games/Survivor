using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColonistAI : MonoBehaviour
{
    public GameObject Tree;

    private List<Transform> _trees = new List<Transform>();
    private List<Transform> _rocks = new List<Transform>();
    private List<Transform> _bases = new List<Transform>();
    private GameObject _world;
    private GameObject _base;
    public Base BaseScript;
    private Rigidbody2D _rb;

    [Header("Behaviour")]
    [SerializeField] private Transform _closestTarget;
    [SerializeField] private AITask _task;
    [SerializeField] private bool _findClosestTarget = true;
    [SerializeField] private bool _atTarget = false;

    [Header("Stats")]
    [SerializeField] private float _speed = 10.0f;
    [SerializeField] private float _interactionRange = 1.5f;
    [SerializeField] public int axePower = 10;
    [SerializeField] public int inventoryCapacity = 3;
    [SerializeField] private int _health;
    [SerializeField] private int _damage;
    [SerializeField] private int _team;

    [Header("Resources")]

    [SerializeField]
    public Dictionary<Material, int> inventory = new Dictionary<Material, int>();

    public void Start()
    {

        foreach (Material resource in System.Enum.GetValues(typeof(Material))) {
            inventory.Add(resource, 0);
        }

        _task = new GatherResourceTask(this, Material.STONE);
        _health = 10 + Random.Range(0, 5);
        _damage = 1 + Random.Range(0, 3);
        _team = Random.Range(1, 3);

        _world = GameObject.Find("World");
        _base = GameObject.Find("Base 1");
        BaseScript = _base.GetComponent<Base>();
        _bases.Add(_base.transform);
        _rb = GetComponent<Rigidbody2D>();

        UpdateList(_trees, "Trees");
        UpdateList(_rocks, "Rocks");
    }

    public void UpdateList(List<Transform> list, string type) {
        list.Clear();
        foreach (Transform child in _world.transform.Find(type))
        {
            list.Add(child);
        }
    }

    private List<Transform> RetrieveList(string type)
    {
        List<Transform> list = new List<Transform>();


        
        if (_world == null) Debug.Log("world is null");
        if (_world.transform == null) Debug.Log("transform is null");
        if (_world.transform.Find(type) == null) Debug.Log("Transform found for " + type + " is null");

        foreach (Transform child in _world.transform.Find(type))
        {
            list.Add(child);
        }

        return list;
    }

    public void Update()
    {
        if (_task == null || _task.GetTarget() == null || RetrieveList(_task.GetTarget()) == null) return; 
        _task.Update(RetrieveList(_task.GetTarget()));

    }

    public void AssignTask(AITask task) {
        _atTarget = false;
        _findClosestTarget = true;
        _task = task;
    }


    public void JobAttack(List<Transform> targets) {
        if (_atTarget)
            return;

        if (_findClosestTarget) {

        }
    }

    public void JobStructure(List<Transform> targets, string type, IEnumerator task) {
        if (_atTarget) {
            return;
        }

        if (_findClosestTarget)
        {
            UpdateList(targets, type);
            _closestTarget = GetClosestStructure(targets);
            if (_closestTarget != null) {
                _closestTarget.GetComponent<Structure>().Workers += 1;
                _findClosestTarget = false;
            }
        }

        if (_closestTarget == null)
        {
            _findClosestTarget = true;
        }
        else {
            WalkTowardsTarget(_closestTarget);

            if (AtTarget(_closestTarget))
            {
                _atTarget = true;
                _findClosestTarget = true;
                StartCoroutine(task);
            }
        }
    }

    public void WalkTowardsTarget(Transform target) {
        Vector2 direction = target.position - transform.position;
        direction.Normalize();

        _rb.drag = 1.0f;
        _rb.AddForce(direction * _speed * Time.deltaTime);
    }

    public bool AtTarget(Transform target) {
        _rb.drag = 1.6f;
        return Vector2.Distance(transform.position, target.position) < _interactionRange;
    }

    private Transform FindClosestTeammate(List<Transform> targets) {
        Transform closestTransform = null;
        float minDist = Mathf.Infinity;
        Vector2 currentPos = transform.position;
        foreach (Transform t in targets) {
            if (t.gameObject.GetComponent<ColonistAI>()._team == _team) {
                float dist = Vector2.Distance(t.position, currentPos);
                if (dist < minDist) {
                    closestTransform = t;
                    minDist = dist;
                }
            }
        }

        return closestTransform;
    }

    public Transform GetClosestStructure(List<Transform> targets) {
        Transform closestTransform = null;
        float minDist = Mathf.Infinity;
        Vector2 currentPos = transform.position;
        foreach (Transform t in targets) {
            if (t.gameObject.GetComponent<Structure>().Workers >= 1 && !t.tag.Equals("Base")) {
                continue;
            }

            float dist = Vector2.Distance(t.position, currentPos);
            if (dist < minDist) {
                closestTransform = t;
                minDist = dist;
            }
        }


        return closestTransform;
    }

    private Transform GetClosestObject(System.Func<GameObject, bool> condition, List<Transform> targets) {
        Transform closestTransform = null;
        float minDist = Mathf.Infinity;
        Vector2 currentPos = transform.position;
        foreach (Transform t in targets) {
            if (condition(t.gameObject)) {
                float dist = Vector2.Distance(t.position, currentPos);
                if (dist < minDist) {
                    closestTransform = t;
                    minDist = dist;
                };
            }


        }

        return closestTransform;
    }

    public bool IsAtTarget()
    {
        return _atTarget;
    }

    public bool InSearchMode()
    {
        return _findClosestTarget;
    }

    public void SetSearchMode(bool newValue)
    {
        _findClosestTarget = newValue;
    }

    public Transform GetClosestTarget()
    {
        return _closestTarget;
    }

    public void SetClosestTarget(Transform target)
    {
        this._closestTarget = target;
    }

    public void SetFoundTarget(bool newValue)
    {
        _atTarget = newValue;
    }

    //TODO: Move to utilities
    private string ToTitleCase(string s)  {
        return System.Globalization.CultureInfo.InvariantCulture.TextInfo.ToTitleCase(s.ToLower());
    }

    
}
