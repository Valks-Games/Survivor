using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WorldAPI.Items;

public class DropOffResourcesTask<T> : StructureTask<T> where T : ResourceGatherer<T>
{
    public DropOffResourcesTask() : base("Bases", "DropOffResources")
    {
    }

    protected override IEnumerator Run()
    {
        yield return TaskLoop();
        yield return new WaitForSeconds(1);

        List<Item> keys = new List<Item>(Target.Inventory.Items.Keys);

        foreach (Material key in keys)
        {
            Target.Base.DepositResource(key, Target.Inventory.Items[key]);
            Target.Inventory.Items[key] = 0;
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