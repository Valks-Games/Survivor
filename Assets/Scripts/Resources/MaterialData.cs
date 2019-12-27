[System.AttributeUsage(System.AttributeTargets.Field)]
public class MaterialData : System.Attribute
{
    public string name { get; private set; }
    public string description { get; private set; }
    public string associatedStructure { get; private set; }
    public float gatherTime { get; private set; }

    public MaterialData(string name, string description = "No description.", string associatedStructure = "Base", float gatherTime = 0f)
    {
        this.name = name;
        this.description = description;
        this.associatedStructure = associatedStructure;
        this.gatherTime = gatherTime;
    }
}