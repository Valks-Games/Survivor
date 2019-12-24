![Preview](preview.gif)

Currently working on a worker AI system.
```cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColonistAI : MonoBehaviour
{
    private GameObject _world;
    public GameObject Tree;
    private GameObject _base;
    private Base _baseScript;

    private Text _textWood;

    public float Speed = 10.0f;
    public float InteractionRange = 1.5f;

    private Rigidbody2D _rb;
    public bool _findClosestTarget = true;
    private string _job = "MineTree";
    private bool _atTarget = false;

    private int _wood = 0;
    private int _axePower = 10;
    private int _inventoryCapacity = 3;

    private List<Transform> _trees = new List<Transform>();
    private List<Transform> _rocks = new List<Transform>();
    private List<Transform> _bases = new List<Transform>();

    public Transform _closestTarget;

    private void Start()
    {
        _world = GameObject.Find("World");
        _base = GameObject.Find("Base(Clone)");
        _baseScript = _base.GetComponent<Base>();
        _bases.Add(_base.transform);
        _rb = GetComponent<Rigidbody2D>();
        _textWood = GameObject.Find("Wood").GetComponent<Text>();

        UpdateList(_trees, "Tree");
        UpdateList(_rocks, "Rock");
    }

    private void UpdateList(List<Transform> list, string type) {
        list.Clear();
        foreach (Transform child in _world.transform)
        {
            if (child.tag.Equals(type))
            {
                list.Add(child);
            }
        }
    }

    private void Update()
    {
        switch (_job) {
            case "MineTree":
                Job(_trees, "Tree", MineTree());
                break;
            case "MineRock":
                break;
            case "DropOffResources":
                if (_wood > 0)
                {
                    Job(_bases, "Base", DropOffResources());
                }
                else
                {
                    _job = "MineTree";
                }
                break;
        }
    }

    private IEnumerator MineTree()
    {
        yield return new WaitForSeconds(1);

        if (_closestTarget == null)
        {
            AssignJob("DropOffResources");
        }
        else {
            Tree treeScript = _closestTarget.gameObject.GetComponent<Tree>();
            _wood += treeScript.ChopWood(_axePower, _wood, _inventoryCapacity);

            if (_wood <= _inventoryCapacity - 1)
            {
                Debug.Log(name + ": Mining another tree.");
                StartCoroutine(MineTree());
            }
            else
            {
                Debug.Log(name + ": Dropping off resources.");
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
        _baseScript.DepositResource("Wood", _wood);
        _wood = 0;
        _textWood.text = "Wood: " + (_baseScript.Wood);

        AssignJob("MineTree");
    }

    public void Job(List<Transform> targets, string type, IEnumerator task) {
        if (_atTarget) {
            return;
        }

        if (_findClosestTarget)
        {
            UpdateList(targets, type);
            _closestTarget = GetClosestTarget(targets);
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
        _rb.AddForce(direction * Speed * Time.deltaTime);
    }

    public bool AtTarget(Transform target) {
        _rb.drag = 1.6f;
        return Vector2.Distance(transform.position, target.position) < InteractionRange;
    }

    private Transform GetClosestTarget(List<Transform> targets) {
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
```
