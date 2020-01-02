public class Rock : Structure
{
    public string te; // <-- what is this?

    public static int id = 0;

    public override void Awake()
    {
        base.Awake();
        id++;
        SetName("Rock " + id);
    }

    public override void Start()
    {
        base.Start();
        SetParent("Rocks");
    }
}
