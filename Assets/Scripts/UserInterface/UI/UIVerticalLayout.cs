using UnityEngine;
using UnityEngine.UI;

public class UIVerticalLayout : UIElement
{
    public VerticalLayoutGroup layout;

    public UIVerticalLayout(string name, Transform parent) : base(parent)
    {
        gameObject.name = name;
        gameObject.transform.position = parent.position;

        layout = gameObject.AddComponent<VerticalLayoutGroup>();
        layout.childAlignment = TextAnchor.MiddleCenter;
    }
}
