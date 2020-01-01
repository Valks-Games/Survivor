using System.Collections;
using UnityEngine;

public class GatherResourceTask : AITask
{
    private readonly Material type;

    public GatherResourceTask(AIEntity entity, Material type) : base(entity, type.GetResourceSource(), "GatherResource")
    {
        this.type = type;
    }

    public override IEnumerator RunTask()
    {
        yield return new WaitForSeconds(type.GetGatherTime());

        if (Entity.ClosestTarget == null)
        {
            Entity.AssignTask(new DropOffResourcesTask(Entity));
        }
        else
        {
            Structure structureComponent = Entity.ClosestTarget.gameObject.GetComponent<Structure>();
            Entity.Inventory[type] += structureComponent.GatherResource(Entity.AxePower, Entity.Inventory[type], Entity.MaxInventorySize);

            if (Entity.Inventory[type] <= Entity.MaxInventorySize - 1)
                Entity.StartCoroutine(RunTask());
            else
                Entity.AssignTask(new DropOffResourcesTask(Entity));
        }
    }
}