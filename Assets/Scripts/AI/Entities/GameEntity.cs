using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using WorldAPI.Entities;
using System;

public abstract class GameEntity<T> : Entity<T> where T: GameEntity<T>
{
    public Transform TargetStructure;

    public static Dictionary<string, List<Transform>> StructureList = new Dictionary<string, List<Transform>>();

    protected GameObject _world;

    public new void Start()
    {
        base.Start();

        foreach (Material resource in Material.Values)
            Inventory.Items.Add(resource, 0);

        _world = GameObject.Find("World");

        HaltDrag = 2.0f;

        if (StructureList.Count == 0)
        {
            StructureList.Add("Trees", RetrieveList("Trees"));
            StructureList.Add("Rocks", RetrieveList("Rocks"));
            StructureList.Add("Bases", RetrieveList("Bases"));
        }
    }

    public Transform ClosestStructure(string type)
    {
        StructureList[type] = StructureList[type]
            .Where(t => t != null)
            .ToList();

        return StructureList[type]
            .OrderBy(t => (t.position - transform.position).sqrMagnitude)
            .Where(t => t.gameObject.gameObject.GetComponent<Structure>().Workers == 0)
            .FirstOrDefault();
    }

    private List<Transform> RetrieveList(string type)
    {
        List<Transform> list = new List<Transform>();

        foreach (Transform child in _world.transform.Find(type))
            list.Add(child);

        return list;
    }
}