using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIElement
{
    public GameObject gameObject;
    public Transform transform => gameObject.transform;

    public UIElement(Transform parent)
    {
        gameObject = new GameObject("UI Element");
        gameObject.transform.SetParent(parent);
    }
}
