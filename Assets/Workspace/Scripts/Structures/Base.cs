using GameAPI.Items;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Base : Structure
{
    public static int ID = 0;

    public int CostMultiplier = 10;

    public int Tier = 1;

    public override void Awake()
    {
        base.Awake();
        ID++;
        SetName("Base " + ID);
    }

    public void Upgrade()
    {
        if (!CanUpgrade)
            return;

        Item wood = Inventory.Withdraw(Material.Wood);
        Item stone = Inventory.Withdraw(Material.Stone);

        wood.Amount -= Tier * CostMultiplier;
        stone.Amount -= Tier * CostMultiplier;

        if (wood.Amount > 0)
            Inventory.Deposit(wood);

        if (stone.Amount > 0)
            Inventory.Deposit(stone);

        Tier++;
        Debug.Log("Upgraded base to tier " + Tier);
    }

    public bool CanUpgrade =>
        ResourcesRequired.All(v => v.Value == 0);

    public Dictionary<Material, int> ResourcesRequired
    {
        get
        {
            Dictionary<Material, int> reqResources = new Dictionary<Material, int>();

            int reqWood = (Tier * CostMultiplier) - Inventory[Material.Wood]?.Amount ?? 0;
            int reqStone = (Tier * CostMultiplier) - Inventory[Material.Stone]?.Amount ?? 0;

            reqResources.Add(Material.Wood, reqWood);
            reqResources.Add(Material.Stone, reqStone);

            return reqResources;
        }
    }

    public override string ToString() =>
        $"ID: {ID}, Tier: {Tier}";
}