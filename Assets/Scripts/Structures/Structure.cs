using UnityEngine;

public class Structure : MonoBehaviour
{
    private GameObject _world;
    public int Resource { get; set; } = 3;
    public int Workers { get; set; } = 0;

    public virtual void Awake()
    {
        SetName("Structure");
    }

    public virtual void Start()
    {
        transform.parent = transform;
        _world = GameObject.Find("World");
    }

    public void SetParent(string parent) =>
        transform.parent = _world.transform.Find(parent);

    public void SetName(string name) =>
        transform.name = name;



    public void Destroy() =>
        Destroy(gameObject);
}
