using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public struct Resource {
    public string name;
    public string description;
    public int amount;



}

public enum Material {
    WOOD, STONE
}

//Not sure if there's a better way of doing this, but oh well
public static class MaterialExtension 
{
    private static Dictionary<Material, string> associatedStructureName = new Dictionary<Material, string>();
    static MaterialExtension()
    {
        associatedStructureName.Add(Material.WOOD, "Trees");
        associatedStructureName.Add(Material.STONE, "Rocks");
    }

    public static string GetResourceSource(this Material m)
    {
        return associatedStructureName[m];
    }



}

