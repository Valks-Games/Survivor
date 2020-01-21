using UnityEngine;

public class OptionsScene : MonoBehaviour
{
    public void Start()
    {
        new Options().Initialize(GameObject.Find("Panel").transform);
    }
}
