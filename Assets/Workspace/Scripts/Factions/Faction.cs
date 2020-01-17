using System.Collections.Generic;

public class Faction
{
    private static int id = 0;

    public int ID;
    public string Name;
    public string Description;

    public Base Base;
    public List<Colonist> members = new List<Colonist>();

    public Faction(string name = null, string description = "No description", Base b = null)
    {
        ID = id++;
        Name = name ?? "Faction " + ID;
        Description = description;
        Base = b;
    }

    public override string ToString()
    {
        return "ID: " + ID + ", Name: " + Name + ", Description: " + Description;
    }
}