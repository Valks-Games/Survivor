using System.Linq;
using UnityEngine;
using WorldAPI.Entities;

public abstract class GameEntity<T> : Entity<T> where T : GameEntity<T>
{
    public Transform TargetStructure;

    protected GameObject world;

    public new void Start()
    {
        base.Start();

        foreach (Material resource in Material.Values)
            Inventory.Items.Add(resource, 0);

        world = GameObject.Find("World");

        HaltDrag = 2.0f;
    }

    public Transform ClosestStructure(string type)
    {
        WorldGenerator.StructureList[type] = WorldGenerator.StructureList[type]
            .Where(t => t != null)
            .ToList();

        return WorldGenerator.StructureList[type]
            .OrderBy(t => (t.position - transform.position).sqrMagnitude)
            .Where(t => t.gameObject.gameObject.GetComponent<Structure>().Workers == 0)
            .FirstOrDefault();
    }
}