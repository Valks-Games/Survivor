using System.Collections;
using UnityEngine;

public abstract class StructureTask<T> : GameTask<T> where T: FactionMember<T>
{
    public readonly string TargetStructure;

    public StructureTask(string target, string name) : base(name)
    {
        TargetStructure = target;
    }

    protected IEnumerator TaskLoop()
    {
        if (TargetStructure == "Bases")
            Target.TargetStructure = Target.Base.transform;
        else
            Target.TargetStructure = Target.ClosestStructure(TargetStructure);

        if (Target.TargetStructure == null)
        {
            Debug.LogWarning("[FactionMemberTask.cs:26] Exited out of Task due to invalid (null) target structure. THIS PROBABLY WILL CAUSE AN ERROR IN COLONIST BEHAVIOUR.");
            yield break;
        }

        Target.TargetStructure.GetComponent<Structure>().Workers++;
        Target.WalkTowards(Target.TargetStructure);

        while (!Target.IsAt(Target.TargetStructure))
            yield return new WaitForSeconds(0.1f);
    }
}