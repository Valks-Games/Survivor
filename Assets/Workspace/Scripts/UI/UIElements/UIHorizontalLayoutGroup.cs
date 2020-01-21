using UnityEngine;
using UnityEngine.UI;

public class UIHorizontalLayoutGroup : UIElement
{
    public override GameObject GameObject { get; }

    public UIHorizontalLayoutGroup(string name, Transform parent, float spacing = 0) : base(name, parent)
    {
        GameObject = new GameObject(name);
        GameObject.transform.SetParent(parent);
        GameObject.transform.localPosition = Vector3.zero;

        HorizontalLayoutGroup horzLayout = GameObject.AddComponent<HorizontalLayoutGroup>();
        horzLayout.childAlignment = TextAnchor.MiddleCenter;
        horzLayout.childControlWidth = false;
        horzLayout.childControlHeight = false;
        horzLayout.childForceExpandWidth = false;
        horzLayout.childForceExpandHeight = false;
        horzLayout.spacing = spacing;
    }
}