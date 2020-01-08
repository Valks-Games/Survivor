using UnityEngine;

public class Category
{
    public GameObject GameObject;
    public Transform Transform;

    public Category(string name, Transform parent) 
    {
        GameObject = new GameObject(name);
        GameObject.transform.SetParent(parent);

        Transform = GameObject.transform;
    }
}
