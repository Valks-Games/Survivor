using UnityEngine;
using UnityEngine.UI;

public class UIHorizontalLayout : UIElement
{
    public HorizontalLayoutGroup layout;

    public UIHorizontalLayout(string name, Transform parent) : base(parent)
    {
        gameObject.name = name;
        gameObject.transform.position = parent.position;

        layout = gameObject.AddComponent<HorizontalLayoutGroup>();
        layout.childAlignment = TextAnchor.MiddleCenter;
        layout.childControlWidth = false;
        layout.childControlHeight = false;
        layout.childForceExpandWidth = false;
        layout.childForceExpandHeight = false;
    }
}
