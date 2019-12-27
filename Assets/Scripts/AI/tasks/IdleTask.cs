using System.Collections;

public class IdleTask : AITask
{
    public IdleTask(AIEntity entity) : base(entity, "Bases", "Idle")
    {

    }

    public override IEnumerator RunTask()
    {
        yield break;
    }
}
