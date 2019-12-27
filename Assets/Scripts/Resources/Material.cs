using System;
using System.Collections.Generic;
using System.Linq;

public enum Material
{
    [MaterialData(
        name: "Wood",
        associatedStructure: "Trees",
        gatherTime: 1f
    )]
    Wood,

    [MaterialData(
        name: "Stone",
        associatedStructure: "Rocks",
        gatherTime: 1f
    )]
    Stone
}

public static class MaterialExtension
{
    private static Dictionary<Material, MaterialData> data = new Dictionary<Material, MaterialData>();

    static MaterialExtension()
    {
        CacheMaterialData();
    }

    public static void CacheMaterialData()
    {
        foreach (Material material in (Material[])Enum.GetValues(typeof(Material)))
            data.Add(material, material.GetMaterialData(true));
    }

    public static MaterialData GetMaterialData(this Material material, bool force = false)
    {
        if (!force)
            return data[material];

        Type type = material.GetType();
        string name = Enum.GetName(type, material);

        return type.GetField(name).GetCustomAttributes(false).OfType<MaterialData>().SingleOrDefault();
    }

    public static string GetName(this Material material)
    {
        return material.GetMaterialData().name;
    }

    public static string GetDescription(this Material material)
    {
        return material.GetMaterialData().description;
    }

    public static float GetGatherTime(this Material material)
    {
        return material.GetMaterialData().gatherTime;
    }

    public static string GetResourceSource(this Material material)
    {
        return material.GetMaterialData().associatedStructure;
    }
}