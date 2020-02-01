using GameAPI.Async.Generic;
using GameAPI.Items;
using GameAPI.Tasks;
using System.Threading.Tasks;

public class GatherResourceTask<T> : StructureTask<T> where T : ResourceGatherer<T>
{
    private readonly Material type;

    public GatherResourceTask(Material type) : base(type.AssociatedStructure, "GatherResource") =>
        this.type = type;

    protected override async Task Run()
    {
        await MoveToStructure();
        await new Delay(type.GatherTime);

        if (Target.TargetStructure == null)
        {
            Target.Queue(new DropOffResourcesTask<T>());
            return;
        }

        StructureResource structure = Target.TargetStructure.gameObject.GetComponent<Structure>() as StructureResource;

        int amount = structure.GatherResource(Target.AxePower, Target.Inventory[type]?.Amount ?? 0, Target.Inventory.MaxSize);
        Target.Inventory.Deposit(new Item(type, amount));

        if (Target.Inventory[type].Amount < Target.Inventory.MaxSize)
            Target.Queue(new GatherResourceTask<T>(type));
        else
            Target.Queue(new DropOffResourcesTask<T>());

        structure.Workers--;
    }
}