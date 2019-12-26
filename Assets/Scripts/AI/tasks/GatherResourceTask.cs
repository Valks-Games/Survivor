using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GatherResourceTask : AITask {

    private Material _type;

    public GatherResourceTask(ColonistAI ai, Material type) : base(ai, type.GetResourceSource(), "GatherResource")
    {
        this._type = type;
    }

    public override IEnumerator RunTask()
    {
        yield return new WaitForSeconds(1);

        if (_ai.GetClosestTarget() == null)
        {
            _ai.AssignTask(new DropOffResourcesTask(_ai));
        } else 
        {
            Structure structureComponent = _ai.GetClosestTarget().gameObject.GetComponent<Structure>();
            _ai.Inventory[_type] += structureComponent.GatherResource(_ai.axePower, _ai.Inventory[_type], _ai.inventoryCapacity);
            
            if (_ai.Inventory[_type] <= _ai.inventoryCapacity - 1)
            {
                _ai.StartCoroutine(RunTask());
            }
            else
            {
                _ai.AssignTask(new DropOffResourcesTask(_ai));
            }
        }
    }

    
}