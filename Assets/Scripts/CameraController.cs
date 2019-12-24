using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float Speed = 100f;

    private void Update()
    {
        float inputHorz = Input.GetAxis("Horizontal");
        float inputVert = Input.GetAxis("Vertical");

        transform.position += new Vector3(inputHorz * Speed * Time.deltaTime, inputVert * Speed * Time.deltaTime, 0);
    }
}
