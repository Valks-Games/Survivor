using UnityEngine;

public class EnableOnStart : MonoBehaviour
{
    public GameObject[] activate;

    public void Awake()
    {
        foreach (GameObject go in activate)
        {
            go.SetActive(true);
        }
    }
}