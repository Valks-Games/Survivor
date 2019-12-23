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

    private Text _wood;

    public float Speed = 10.0f;
    public float InteractionRange = 1.5f;

    private Rigidbody2D _rb;
    public bool _walkToTree = true;
    public bool _walkToBase;
    public bool _findClosestTarget = true;

    private List<Transform> _trees = new List<Transform>();

    private Transform _closestTarget;

    private void Start()
    {
        _world = GameObject.Find("World");
        _base = GameObject.Find("Base(Clone)");
        _baseScript = _base.GetComponent<Base>();
        _rb = GetComponent<Rigidbody2D>();
        _wood = GameObject.Find("Wood").GetComponent<Text>();

        foreach (Transform child in _world.transform)
        {
            if (child.name.Equals("Tree(Clone)")){
                _trees.Add(child);
            }
        }
    }

    private void Update()
    {
        if (_walkToTree) {
            if (_findClosestTarget) {
                _closestTarget = GetClosestTarget(_trees);
                _findClosestTarget = false;
            }

            WalkTowardsTarget(_closestTarget);

            if (AtTarget(_closestTarget)) {
                _walkToTree = false;
                StartCoroutine(MineTree());
            }
        }

        if (_walkToBase) {
            WalkTowardsTarget(_base.transform);

            if (AtTarget(_base.transform)) {
                _walkToBase = false;
                StartCoroutine(DropOffResources());
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
            float dist = Vector2.Distance(t.position, currentPos);
            if (dist < minDist) {
                closestTransform = t;
                minDist = dist;
            }
        }
        return closestTransform;
    }

    private IEnumerator MineTree() {
        yield return new WaitForSeconds(1);
        _walkToBase = true;
        Debug.Log("Done mining a tree.");
    }

    private IEnumerator DropOffResources() {
        yield return new WaitForSeconds(1);
        _walkToTree = true;
        _findClosestTarget = true;

        _baseScript.AddWood(1);
        _wood.text = "Wood: " + (_baseScript.Wood);

        Debug.Log("Done dropping off resources.");
    }
}
