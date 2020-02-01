using System.Threading.Tasks;
using UnityEngine;

public abstract class StructureTask<T> : GameTask<T> where T : FactionMember<T>
{
    public readonly string TargetStructure;

    public StructureTask(string target, string name) : base(name) =>
        TargetStructure = target;

    protected async Task MoveToStructure()
    {
        if (TargetStructure == "Bases")
            Target.TargetStructure = Target.Base.transform;
        else
            Target.TargetStructure = Target.ClosestStructure(TargetStructure);

        if (Target.TargetStructure == null)
            return;

        Target.TargetStructure.GetComponent<Structure>().Workers++;
        await Target.Move(Target.TargetStructure);
    }
}