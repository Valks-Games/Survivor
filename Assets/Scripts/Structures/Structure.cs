using UnityEngine;

public class Structure : MonoBehaviour
{
    public int Resource { get; private set; } = 3;
    public string Type { get; set; } = "None";
    public int Workers { get; set; } = 0;
    public int Team { get; set; } = 0;

    public int GatherResource(int toolPower, int colonistResource, int colonistInvCapacity)
    {
        int gathered = toolPower >= Resource ? Resource : toolPower;
        gathered = Mathf.Min(colonistResource + gathered, colonistInvCapacity);

        Resource -= gathered;

        if (Resource <= 0)
            Destroy();

        return gathered;
    }

    public void Destroy() {
        Destroy(gameObject);
    }
}
