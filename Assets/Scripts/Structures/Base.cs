using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Base : Structure
{
    public static int id = 0;

    public int costMultiplier = 10;

    private int tier = 1;

    public Dictionary<Material, int> Resources = new Dictionary<Material, int>();

    public override void Awake()
    {
        base.Awake();
        id++;
        SetName("Base " + id);
    }

    public override void Start()
    {
        foreach (Material resource in Enum.GetValues(typeof(Material)))
        {
            Resources.Add(resource, 0);
        }


        base.Start();
        SetParent("Bases");
    }

    public void DepositResource(Material type, int amount)
    {
        Resources[type] += amount;


        GameObject.Find(ToTitleCase(type.ToString().ToLower())).GetComponent<Text>().text = "" + ToTitleCase(type.ToString().ToLower()) + ": " + Resources[type];
    }

    public void Upgrade()
    {
        if (CanUpgrade())
        {
            Resources[Material.Wood] -= tier * costMultiplier;
            Resources[Material.Stone] -= tier * costMultiplier;

            tier++;
            Debug.Log("Upgraded base to tier " + tier);
        }
    }

    public bool CanUpgrade()
    {
        Dictionary<Material, int> reqResources = ResourcesRequired();
        //Debug.Log(reqResources.ToString());

        //TODO: See if there's any faster way to do this
        List<Material> keys = new List<Material>(reqResources.Keys);

        foreach (Material key in keys)
            if (reqResources[key] != 0)
                return false;

        return true;
    }



    public Dictionary<Material, int> ResourcesRequired()
    {
        Dictionary<Material, int> reqResources = new Dictionary<Material, int>();

        int reqWood = (tier * costMultiplier) - Resources[Material.Wood];
        int reqStone = (tier * costMultiplier) - Resources[Material.Stone];

        reqResources.Add(Material.Wood, reqWood);
        reqResources.Add(Material.Stone, reqStone);

        return reqResources;
    }

    private string ToTitleCase(string s)
    {
        return System.Globalization.CultureInfo.InvariantCulture.TextInfo.ToTitleCase(s.ToLower());
    }
}
