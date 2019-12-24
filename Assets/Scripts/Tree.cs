using UnityEngine;

public class Tree : Structure
{
    public int Wood { get; private set; } = 3;

    public int ChopWood(int axePower, int colonistWood, int colonistInvCapacity) {
        int gathered = axePower >= Wood ? Wood : axePower;
        gathered = Mathf.Min(colonistWood + gathered, colonistInvCapacity);

        Wood -= gathered;

        if (Wood <= 0)
            Destroy();

        return gathered;
    }
}
