using System.Collections;
using UnityEngine;

public class GatherResourceTask : AITask
{

    private readonly Material _type;

    public GatherResourceTask(ColonistAI colonist, Material type) : base(colonist, type.GetResourceSource(), "GatherResource")
    {
        this._type = type;
    }

    public override IEnumerator RunTask()
    {
        yield return new WaitForSeconds(1);

        if (_colonist.GetClosestTarget() == null)
        {
            _colonist.AssignTask(new DropOffResourcesTask(_colonist));
        }
        else
        {
            Structure structureComponent = _colonist.GetClosestTarget().gameObject.GetComponent<Structure>();
            _colonist.inventory[_type] += structureComponent.GatherResource(_colonist.axePower, _colonist.inventory[_type], _colonist.inventoryCapacity);

            if (_colonist.inventory[_type] <= _colonist.inventoryCapacity - 1)
            {
                _colonist.StartCoroutine(RunTask());
            }
            else
            {
                _colonist.AssignTask(new DropOffResourcesTask(_colonist));
            }
        }
    }


}