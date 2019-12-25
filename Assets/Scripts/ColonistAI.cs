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
    private Base _baseScript;
    private Rigidbody2D _rb;

    [Header("Behaviour")]
    [SerializeField] private Transform _closestTarget;
    [SerializeField] private string _job = "MineStone";
    [SerializeField] private bool _findClosestTarget = true;
    [SerializeField] private bool _atTarget = false;

    [Header("Stats")]
    [SerializeField] private float _speed = 10.0f;
    [SerializeField] private float _interactionRange = 1.5f;
    [SerializeField] private int _axePower = 10;
    [SerializeField] private int _inventoryCapacity = 3;
    [SerializeField] private int _health;
    [SerializeField] private int _damage;
    [SerializeField] private int _team;

    [Header("Resources")]
    [SerializeField] private int _wood = 0;
    [SerializeField] private int _stone = 0;

    public void Start()
    {
        _health = 10 + Random.Range(0, 5);
        _damage = 1 + Random.Range(0, 3);
        _team = Random.Range(1, 3);

        _world = GameObject.Find("World");
        _base = GameObject.Find("Base(Clone)");
        _baseScript = _base.GetComponent<Base>();
        _bases.Add(_base.transform);
        _rb = GetComponent<Rigidbody2D>();

        UpdateList(_trees, "Trees");
        UpdateList(_rocks, "Rocks");
    }

    private void UpdateList(List<Transform> list, string type) {
        list.Clear();
        foreach (Transform child in _world.transform.Find(type))
        {
            list.Add(child);
        }
    }

    public void Update()
    {
        switch (_job) {
            case "CreateBase":
                break;
            case "GroupUp":
                break;
            case "MineTree":
                JobStructure(_trees, "Trees", MineWood());
                break;
            case "MineStone":
                JobStructure(_rocks, "Rocks", MineStone());
                break;
            case "DropOffResources":
                JobStructure(_bases, "Bases", DropOffResources());
                break;
            case "FindFight":
                break;
        }
    }

    private IEnumerator MineWood()
    {
        yield return new WaitForSeconds(1);

        if (_closestTarget == null)
        {
            AssignJob("DropOffResources");
        }
        else {
            Structure structureScript = _closestTarget.gameObject.GetComponent<Structure>();
            _wood += structureScript.GatherResource(_axePower, _wood, _inventoryCapacity);

            if (_wood <= _inventoryCapacity - 1)
            {
                StartCoroutine(MineWood());
            }
            else
            {
                AssignJob("DropOffResources");
            }
        }
    }

    private IEnumerator MineStone()
    {
        yield return new WaitForSeconds(1);

        if (_closestTarget == null)
        {
            AssignJob("DropOffResources");
        }
        else
        {
            Structure structureScript = _closestTarget.gameObject.GetComponent<Structure>();
            _stone += structureScript.GatherResource(_axePower, _stone, _inventoryCapacity);

            if (_stone <= _inventoryCapacity - 1)
            {
                StartCoroutine(MineStone());
            }
            else
            {
                AssignJob("DropOffResources");
            }
        }
    }

    private void AssignJob(string job) {
        _atTarget = false;
        _findClosestTarget = true;
        _job = job;
    }

    private IEnumerator DropOffResources()
    {
        yield return new WaitForSeconds(1);

        _baseScript.DepositResource("Stone", _stone);
        _stone = 0;


        _baseScript.DepositResource("Wood", _wood);
        _wood = 0;

        if (!_baseScript.CanUpgrade())
        {
            Dictionary<string, int> reqResources = _baseScript.ResourcesRequired();

            if (reqResources["Wood"] > reqResources["Stone"])
            {
                AssignJob("MineTree");
            }
            else
            {
                AssignJob("MineStone");
            }
        }
        else
        {
            _baseScript.Upgrade();
            AssignJob("DropOffResources");
        }
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

    private Transform GetClosestStructure(List<Transform> targets) {
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
}
