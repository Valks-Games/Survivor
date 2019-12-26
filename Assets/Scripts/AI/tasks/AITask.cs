using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AITask
{

    private readonly string _name;
    protected ColonistAI _colonist;
    private readonly string _target;
    public AITask(ColonistAI colonist, string target, string name)
    {
        this._colonist = colonist;
        this._name = name;
        this._target = target;
    }


    public void Update(List<Transform> targets)
    {
        TaskLoop(targets);
    }

    public abstract IEnumerator RunTask();

    private void TaskLoop(List<Transform> targets)
    {
        if (_colonist.IsAtTarget())
        {
            return;
        }

        if (_colonist.InSearchMode())
        {
            _colonist.UpdateList(targets, _target);
            _colonist.SetClosestTarget(_colonist.GetClosestStructure(targets));
            if (_colonist.GetClosestTarget() != null)
            {
                _colonist.GetClosestTarget().GetComponent<Structure>().Workers += 1;
                _colonist.SetSearchMode(false);
            }
        }

        if (_colonist.GetClosestTarget() == null)
        {
            _colonist.SetSearchMode(true);
        }
        else
        {
            _colonist.WalkTowardsTarget(_colonist.GetClosestTarget());

            if (_colonist.AtTarget(_colonist.GetClosestTarget()))
            {
                _colonist.SetFoundTarget(true);

                _colonist.SetSearchMode(true);

                _colonist.StartCoroutine(RunTask());
            }
        }
    }

    public string GetTarget()
    {
        return _target;
    }
}