using UnityEngine;

public class Structure : MonoBehaviour
{
    public int Workers { get; set; } = 0;

    public void Destroy() {
        Destroy(gameObject);
    }
}
