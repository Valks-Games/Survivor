using UnityEngine;

public class LightingController : MonoBehaviour
{
    public float Speed = 0.1f;

    public void Update()
    {
        transform.Rotate(new Vector3(1, 1, 0) * Time.deltaTime * Speed);
    }
}
