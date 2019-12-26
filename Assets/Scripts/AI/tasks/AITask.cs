using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public abstract class AITask {

    private string _name;
    protected ColonistAI _ai;
    private string _target;
    public AITask(ColonistAI ai, string target, string name)
    {
        this._ai = ai;
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
        if (_ai.IsAtTarget())
        {
            return;
        }

        if (_ai.InSearchMode())
        {
            _ai.UpdateList(targets, _target);
            _ai.SetClosestTarget(_ai.GetClosestStructure(targets)); 
            if (_ai.GetClosestTarget() != null) {
                _ai.GetClosestTarget().GetComponent<Structure>().Workers += 1;
                 _ai.SetSearchMode(false);
            }
        }

        if (_ai.GetClosestTarget() == null)
        {
            _ai.SetSearchMode(true);
        }
        else {
            _ai.WalkTowardsTarget(_ai.GetClosestTarget());

            if (_ai.AtTarget(_ai.GetClosestTarget()))
            {
                _ai.SetFoundTarget(true);

                _ai.SetSearchMode(true);
                
                _ai.StartCoroutine(RunTask());
            }
        }
    }

    public string GetTarget()
    {
        return _target;
    }

    

    



}