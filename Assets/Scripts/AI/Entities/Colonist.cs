using UnityEngine;

public class Colonist : AIEntity
{
    public void Start()
    {
        Init(
            health: 10 + Random.Range(0, 5),
            damage: 1 + Random.Range(0, 3)
            );

        AssignTask(new GatherResourceTask(this, Material.Stone));
    }

    public new void Update()
    {
        base.Update();
    }
}
