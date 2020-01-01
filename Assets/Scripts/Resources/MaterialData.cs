[System.AttributeUsage(System.AttributeTargets.Field)]
public class MaterialData : System.Attribute
{
    public string Name { get; private set; }
    public string Description { get; private set; }
    public string AssociatedStructure { get; private set; }
    public float GatherTime { get; private set; }

    public MaterialData(string name, string description = "No description.", string associatedStructure = "Base", float gatherTime = 0f)
    {
        this.Name = name;
        this.Description = description;
        this.AssociatedStructure = associatedStructure;
        this.GatherTime = gatherTime;
    }
}