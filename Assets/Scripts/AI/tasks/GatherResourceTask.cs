using System.Collections;
using UnityEngine;

public class GatherResourceTask<T> : StructureTask<T> where T: ResourceGatherer<T>
{
    private readonly Material _type;

    public GatherResourceTask(Material type) : base(type.AssociatedStructure, "GatherResource")
    {
        _type = type;
    }

    protected override IEnumerator Run()
    {
        yield return TaskLoop();
        yield return new WaitForSeconds(_type.GatherTime);

        if (Target.TargetStructure == null)
        {
            Target.AssignTask(new DropOffResourcesTask<T>());
            yield break;
        }
        
        Structure structureComponent = Target.TargetStructure.gameObject.GetComponent<Structure>();
        Target.Inventory[_type] += structureComponent.GatherResource(Target.AxePower, Target.Inventory[_type], Target.MaxInventorySize);

        if (Target.Inventory[_type] < Target.MaxInventorySize)
            Target.QueueTask(new GatherResourceTask<T>(_type));
        else
            Target.QueueTask(new DropOffResourcesTask<T>());

        structureComponent.Workers--;
    }
}