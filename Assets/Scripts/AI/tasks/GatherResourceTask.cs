using System.Collections;
using UnityEngine;

public class GatherResourceTask : AITask
{
    private readonly Material _type;

    public GatherResourceTask(AIEntity entity, Material type) : base(entity, type.GetResourceSource(), "GatherResource")
    {
        _type = type;
    }

    public override IEnumerator RunTask()
    {
        yield return new WaitForSeconds(_type.GetGatherTime());

        if (Entity.ClosestTarget == null)
        {
            Entity.AssignTask(new DropOffResourcesTask(Entity));
        }
        else
        {
            Structure structureComponent = Entity.ClosestTarget.gameObject.GetComponent<Structure>();
            Entity.Inventory[_type] += structureComponent.GatherResource(Entity.AxePower, Entity.Inventory[_type], Entity.MaxInventorySize);

            if (Entity.Inventory[_type] <= Entity.MaxInventorySize - 1)
                Entity.StartCoroutine(RunTask());
            else
                Entity.AssignTask(new DropOffResourcesTask(Entity));
        }
    }
}