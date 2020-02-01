using GameAPI.Namespacing;
using System.Collections.Generic;

public sealed partial class Material : GameAPI.Items.Material
{
    public static List<Material> Values = new List<Material>();

    public readonly string Description;
    public readonly string AssociatedStructure;
    public readonly float GatherTime;

    private Material(Key key, string name, string description = "No description.", string associatedStructure = "Base", float gatherTime = 0f) : base(key, name)
    {
        Description = description;
        AssociatedStructure = associatedStructure;
        GatherTime = gatherTime;

        Values.Add(this);
    }
}

public partial class Material
{
    public static readonly Material Wood = new Material(
        key: "material:wood",
        name: "Wood",
        associatedStructure: "Trees",
        gatherTime: 1f
    );

    public static readonly Material Stone = new Material(
        key: "material:stone",
        name: "Stone",
        associatedStructure: "Rocks",
        gatherTime: 1f
    );
}