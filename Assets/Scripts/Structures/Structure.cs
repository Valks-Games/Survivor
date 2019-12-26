using UnityEngine;

public class Structure : MonoBehaviour
{
    private GameObject _canvas;
    private GameObject world;
    public int Resource { get; private set; } = 3;
    public string Type { get; set; } = "None";
    public int Workers { get; set; } = 0;
    public int Team { get; set; } = 0;

    public virtual void Awake() {
        SetName("Structure");
        SetTeam(1);
    }

    public virtual void Start()
    {
        transform.parent = transform;
        world = GameObject.Find("World");
        _canvas = GameObject.Find("World Canvas");
    }

    public virtual void Update()
    {
        ClickedStructureListener();
    }

    private void ClickedStructureListener()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);

            if (hit.collider != null)
            {
                if (hit.collider.tag.Equals("Base"))
                {
                    //var panel = Resources.Load("Prefabs/PanelBaseInfo");
                    //Instantiate(panel, _canvas.transform);

                    Debug.Log("Yes");
                }
            }
        }
    }

    public void SetParent(string parent) {
        transform.parent = world.transform.Find(parent);
    }

    public void SetName(string name) {
        transform.name = name;
    }

    public void SetTeam(int team) {
        Team = team;
    }

    public int GatherResource(int toolPower, int colonistResource, int colonistInvCapacity)
    {
        int gathered = toolPower >= Resource ? Resource : toolPower;
        gathered = Mathf.Min(colonistResource + gathered, colonistInvCapacity);

        Resource -= gathered;

        if (Resource <= 0)
            Destroy();

        return gathered;
    }

    

    public void Destroy() {
        Destroy(gameObject);
    }
}
