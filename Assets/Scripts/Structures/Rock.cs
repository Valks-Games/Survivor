public class Rock : Structure
{
    public static int id = 0;

    public override void Awake()
    {
        base.Awake();
        id++;
        SetName("Rock " + id);
        SetParent("Rocks");
    }

    public override void Start()
    {
        base.Start();
    }
}
