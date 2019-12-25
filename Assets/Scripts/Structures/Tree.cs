using UnityEngine;

public class Tree : Structure
{
    public static int id = 0;

    public override void Awake()
    {
        base.Start();
        id++;
        SetName("Tree " + id);
        
    }

    public override void Start()
    {
        base.Start();
        SetParent("Trees");
    }
}
