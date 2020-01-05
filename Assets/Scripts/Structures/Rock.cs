public class Rock : StructureResource
{
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
    }
}
