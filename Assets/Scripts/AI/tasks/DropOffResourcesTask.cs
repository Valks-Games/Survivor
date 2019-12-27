using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropOffResourcesTask : AITask
{
    public DropOffResourcesTask(AIEntity entity) : base(entity, "Bases", "DropOffResources")
    {

    }

    public override IEnumerator RunTask()
    {
        yield return new WaitForSeconds(1);

        List<Material> keys = new List<Material>(Entity.Inventory.Keys);

        foreach (Material key in keys)
        {
            Entity.Base.DepositResource(key, Entity.Inventory[key]);
            Entity.Inventory[key] = 0;
        }

        if (!Entity.Base.CanUpgrade)
        {
            Dictionary<Material, int> reqResources = Entity.Base.ResourcesRequired;

            if (reqResources[Material.Wood] > reqResources[Material.Stone])
            {
                Entity.AssignTask(new GatherResourceTask(Entity, Material.Wood));
            }
            else
            {
                Entity.AssignTask(new GatherResourceTask(Entity, Material.Stone));
            }
        }
        else
        {
            Entity.Base.Upgrade();
            Entity.AssignTask(new DropOffResourcesTask(Entity));
        }
    }
}