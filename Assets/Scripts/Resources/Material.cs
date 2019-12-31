using System.Collections.Generic;
using WorldAPI.Resources;

public sealed partial class Material : Resource
{
    public static List<Material> Values = new List<Material>();

    public readonly string Description;
    public readonly string AssociatedStructure;
    public readonly float GatherTime;

    private Material(string name, string description = "No description.", string associatedStructure = "Base", float gatherTime = 0f) : base(name)
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
        name: "Wood",
        associatedStructure: "Trees",
        gatherTime: 1f
    );

    public static readonly Material Stone = new Material(
        name: "Stone",
        associatedStructure: "Rocks",
        gatherTime: 1f
    );
}