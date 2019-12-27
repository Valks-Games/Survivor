using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropOffResourcesTask : AITask
{
    public DropOffResourcesTask(ColonistAI colonist) : base(colonist, "Bases", "DropOffResources")
    {

    }

    public override IEnumerator RunTask()
    {
        yield return new WaitForSeconds(1);

        List<Material> keys = new List<Material> (_colonist.Inventory.Keys);

        foreach (Material key in keys)
        {
            _colonist.BaseScript.DepositResource(key, _colonist.Inventory[key]);
            _colonist.Inventory[key] = 0;
        }

        if (!_colonist.BaseScript.CanUpgrade())
        {


            Dictionary<Material, int> reqResources = _colonist.BaseScript.ResourcesRequired();

            if (reqResources[Material.Wood] > reqResources[Material.Stone])
            {
                _colonist.AssignTask(new GatherResourceTask(_colonist, Material.Wood));
            }
            else
            {
                _colonist.AssignTask(new GatherResourceTask(_colonist, Material.Stone));
            }
        }
        else
        {
            _colonist.BaseScript.Upgrade();
            _colonist.AssignTask(new DropOffResourcesTask(_colonist));
        }
    }
}