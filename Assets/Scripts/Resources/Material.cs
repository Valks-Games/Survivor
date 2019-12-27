using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public enum Material
{
    Wood,
    Stone
}

//Not sure if there's a better way of doing this, but oh well
public static class MaterialExtension 
{
    private static Dictionary<Material, string> associatedStructureName = new Dictionary<Material, string>();
    static MaterialExtension()
    {
        associatedStructureName.Add(Material.Wood, "Trees");
        associatedStructureName.Add(Material.Stone, "Rocks");
    }

    public static string GetResourceSource(this Material m)
    {
        return associatedStructureName[m];
    }
}
