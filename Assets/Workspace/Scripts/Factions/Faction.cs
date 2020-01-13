using System.Collections.Generic;

public class Faction
{
    private static int id = 0;

    public int Id;
    public string Name;
    public string Description;

    public Base Base;
    public List<Colonist> members = new List<Colonist>();

    public Faction(string name = null, string description = "No description", Base b = null)
    {
        Id = id++;
        Name = name ?? "Faction " + Id;
        Description = description;
        Base = b;
    }
}