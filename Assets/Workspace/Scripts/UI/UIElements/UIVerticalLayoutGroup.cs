using UnityEngine;
using UnityEngine.UI;

public class UIVerticalLayoutGroup : UIElement
{
    public override GameObject GameObject { get; }

    public UIVerticalLayoutGroup(string name, Transform parent, float spacing = 0) : base(name, parent)
    {
        GameObject = new GameObject(name);
        GameObject.transform.SetParent(parent);
        GameObject.transform.localPosition = Vector3.zero;

        VerticalLayoutGroup vertLayout = GameObject.AddComponent<VerticalLayoutGroup>();
        vertLayout.childAlignment = TextAnchor.MiddleCenter;
        vertLayout.childControlWidth = false;
        vertLayout.childControlHeight = false;
        vertLayout.childForceExpandWidth = false;
        vertLayout.childForceExpandHeight = false;
        vertLayout.spacing = spacing;
    }
}