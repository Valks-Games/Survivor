using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DropOffResourcesTask : AITask {
    public DropOffResourcesTask(ColonistAI ai) : base(ai, "Bases", "DropOffResources")
    {

    }

    public override IEnumerator RunTask()
    {
        yield return new WaitForSeconds(1);

        List<Material> keys = new List<Material> (_ai.inventory.Keys);

        foreach (Material key in keys)
        {
            _ai.BaseScript.DepositResource(key, _ai.inventory[key]);
            _ai.inventory[key] = 0;
        }

        if (!_ai.BaseScript.CanUpgrade())
        {


            Dictionary<Material, int> reqResources = _ai.BaseScript.ResourcesRequired();

            if (reqResources[Material.WOOD] > reqResources[Material.STONE])
            {
                _ai.AssignTask(new GatherResourceTask(_ai, Material.WOOD));
            }
            else
            {
                _ai.AssignTask(new GatherResourceTask(_ai, Material.STONE));
            }
        }
        else
        {
            _ai.BaseScript.Upgrade();
            _ai.AssignTask(new DropOffResourcesTask(_ai));

        }
    }

}