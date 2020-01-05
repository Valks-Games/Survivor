using System.Collections;
using UnityEngine;

public class GatherResourceTask<T> : StructureTask<T> where T: ResourceGatherer<T>
{
    private readonly Material type;

    public GatherResourceTask(Material type) : base(type.AssociatedStructure, "GatherResource")
    {
        this.type = type;
    }

    protected override IEnumerator Run()
    {
        yield return TaskLoop();
        yield return new WaitForSeconds(type.GatherTime);

        if (Target.TargetStructure == null)
        {
            Target.AssignTask(new DropOffResourcesTask<T>());
            yield break;
        }
        
        Structure structureComponent = Target.TargetStructure.gameObject.GetComponent<Structure>();
        Target.Inventory.Items[type] += ((StructureResource) structureComponent).GatherResource(Target.AxePower, Target.Inventory.Items[type], Target.Inventory.MaxSize);

        if (Target.Inventory.Items[type] < Target.Inventory.MaxSize)
            Target.QueueTask(new GatherResourceTask<T>(type));
        else
            Target.QueueTask(new DropOffResourcesTask<T>());

        structureComponent.Workers--;
    }
}