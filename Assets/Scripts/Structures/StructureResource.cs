using UnityEngine;

public class StructureResource : Structure {
    public int GatherResource(int toolPower, int colonistResource, int colonistInvCapacity)
    {
        int gathered = Mathf.Min(colonistResource + toolPower, colonistInvCapacity, Resource);
        Resource -= gathered;

       if (Resource < 1)
       {
            Destroy(transform.gameObject);
            Destroy();
       }

        return gathered;
    }
}