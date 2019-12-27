using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AITask
{
    public readonly string Name;
    public AIEntity Entity;
    public readonly string Target;
    public AITask(AIEntity entity, string target, string name)
    {
        Entity = entity;
        Name = name;
        Target = target;
    }

    public void Update(List<Transform> targets)
    {
        TaskLoop(targets);
    }

    public abstract IEnumerator RunTask();

    private void TaskLoop(List<Transform> targets)
    {
        if (Entity.IsAtTarget)
        {
            return;
        }

        if (Entity.Searching)
        {
            Entity.UpdateList(targets, Target);
            Entity.ClosestTarget = Entity.GetClosestStructure(targets);

            if (Entity.ClosestTarget != null)
            {
                Entity.ClosestTarget.GetComponent<Structure>().Workers += 1;
                Entity.Searching = false;
            }
        }

        if (Entity.ClosestTarget == null)
        {
            Entity.Searching = true;
            return;
        }

        Entity.WalkTowardsTarget(Entity.ClosestTarget);

        if (Entity.AtTarget(Entity.ClosestTarget))
        {
            Entity.IsAtTarget = true;
            Entity.Searching = true;

            Entity.StartCoroutine(RunTask());
        }
    }
}