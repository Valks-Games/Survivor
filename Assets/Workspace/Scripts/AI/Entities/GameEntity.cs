using GameAPI.Entities;
using System.Linq;
using UnityEngine;

public abstract class GameEntity<T> : Entity<T> where T : GameEntity<T>
{
    public Transform TargetStructure;

    protected GameObject world;

    public new void Start()
    {
        base.Start();

        world = GameObject.Find("World");

        HaltDrag = 2.0f;
    }

    public Transform ClosestStructure(string type)
    {
        World.StructureList[type] = 
        (
            from t in World.StructureList[type]
            where t != null
            select t
        ).ToList();

        return
        (
            from t in World.StructureList[type]
            where t.gameObject.gameObject.GetComponent<Structure>().Workers == 0
            orderby (t.position - transform.position).sqrMagnitude
            select t
        ).FirstOrDefault();
    }
}