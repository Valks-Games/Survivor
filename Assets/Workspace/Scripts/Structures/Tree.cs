public class Tree : StructureResource
{
    public static int ID = 0;

    public override void Awake()
    {
        base.Start();
        ID++;
        SetName("Tree " + ID);
    }

    public override void Start()
    {
        base.Start();
    }

    public override string ToString()
    {
        return "ID: " + ID;
    }
}