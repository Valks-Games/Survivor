using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropOffResourcesTask<T> : StructureTask<T> where T : ResourceGatherer<T>
{
    public DropOffResourcesTask() : base("Bases", "DropOffResources")
    {

    }

    protected override IEnumerator Run()
    {
        yield return TaskLoop();
        yield return new WaitForSeconds(1);

        List<Material> keys = new List<Material>(Target.Inventory.Keys);

        foreach (Material key in keys)
        {
            Target.Base.DepositResource(key, Target.Inventory[key]);
            Target.Inventory[key] = 0;
        }

        if (Target.Base.CanUpgrade)
        {
            Target.Base.Upgrade();
            Target.QueueTask(new DropOffResourcesTask<T>());
            yield break;
        }
        
        Dictionary<Material, int> reqResources = Target.Base.ResourcesRequired;

        if (reqResources[Material.Wood] > reqResources[Material.Stone])
            Target.QueueTask(new GatherResourceTask<T>(Material.Wood));
        else
            Target.QueueTask(new GatherResourceTask<T>(Material.Stone));

        Target.TargetStructure.GetComponent<Structure>().Workers--;
    }
}