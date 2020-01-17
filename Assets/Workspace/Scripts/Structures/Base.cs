using System.Collections.Generic;
using UnityEngine;

public class Base : Structure
{
    public static int ID = 0;

    public int CostMultiplier = 10;

    public int Tier = 1;

    public Dictionary<Material, int> Resources = new Dictionary<Material, int>();

    public override void Awake()
    {
        base.Awake();
        ID++;
        SetName("Base " + ID);
    }

    public override void Start()
    {
        foreach (Material resource in Material.Values)
            Resources.Add(resource, 0);

        base.Start();
    }

    public void DepositResource(Material type, int amount)
    {
        Resources[type] += amount;
    }

    public void Upgrade()
    {
        if (!CanUpgrade)
            return;

        Resources[Material.Wood] -= Tier * CostMultiplier;
        Resources[Material.Stone] -= Tier * CostMultiplier;

        Tier++;
        Debug.Log("Upgraded base to tier " + Tier);
    }

    public bool CanUpgrade
    {
        get
        {
            Dictionary<Material, int> reqResources = ResourcesRequired;
            // Debug.Log(reqResources.ToString());

            // TODO: See if there's any faster way to do this
            List<Material> keys = new List<Material>(reqResources.Keys);

            foreach (Material key in keys)
                if (reqResources[key] != 0)
                    return false;

            return true;
        }
    }

    public Dictionary<Material, int> ResourcesRequired
    {
        get
        {
            Dictionary<Material, int> reqResources = new Dictionary<Material, int>();

            int reqWood = (Tier * CostMultiplier) - Resources[Material.Wood];
            int reqStone = (Tier * CostMultiplier) - Resources[Material.Stone];

            reqResources.Add(Material.Wood, reqWood);
            reqResources.Add(Material.Stone, reqStone);

            return reqResources;
        }
    }

    public override string ToString()
    {
        return "ID: " + ID + ", Tier: " + Tier;
    }
}