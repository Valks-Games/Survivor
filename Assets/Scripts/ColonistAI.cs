using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColonistAI : MonoBehaviour
{
    public GameObject Tree;

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
    public Dictionary<Material, int> Inventory = new Dictionary<Material, int>();

    public void Start()
    {

        foreach (Material resource in System.Enum.GetValues(typeof(Material))) {
            Inventory.Add(resource, 0);
        }

        _task = new GatherResourceTask(this, Material.Stone);
        _health = 10 + Random.Range(0, 5);
        _damage = 1 + Random.Range(0, 3);
        _team = Random.Range(1, 3);

        _world = GameObject.Find("World");
        _base = GameObject.Find("Base 1");
        BaseScript = _base.GetComponent<Base>();
        _rb = GetComponent<Rigidbody2D>();


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
