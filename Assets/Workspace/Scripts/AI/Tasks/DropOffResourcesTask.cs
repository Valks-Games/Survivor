using GameAPI.Async.Generic;
using GameAPI.Tasks;
using System.Collections.Generic;
using System.Threading.Tasks;

public class DropOffResourcesTask<T> : StructureTask<T> where T : ResourceGatherer<T>
{
    public DropOffResourcesTask() : base("Bases", "DropOffResources")
    {
    }

    protected override async Task Run()
    {
        await MoveToStructure();
        await new Delay(1f);

        Target.Base.Inventory.DepositAll(Target.Inventory.WithdrawAll());

        if (Target.Base.CanUpgrade)
            Target.Base.Upgrade();

        Dictionary<Material, int> reqResources = Target.Base.ResourcesRequired;

        if (reqResources[Material.Wood] > reqResources[Material.Stone])
            Target.Queue(new GatherResourceTask<T>(Material.Wood));
        else
            Target.Queue(new GatherResourceTask<T>(Material.Stone));

        Target.TargetStructure.GetComponent<Structure>().Workers--;
    }
}