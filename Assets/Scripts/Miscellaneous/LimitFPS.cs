using UnityEngine;

public class LimitFPS : MonoBehaviour
{
    public void Start()
    {
        Application.targetFrameRate = 60;
    }
}