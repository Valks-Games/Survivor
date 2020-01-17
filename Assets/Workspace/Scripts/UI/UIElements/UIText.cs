using UnityEngine;
using UnityEngine.UI;

public class UIText : UIElement
{
    public override GameObject GameObject { get; }

    public UIText(string name, Transform parent) : base(name, parent)
    {
        GameObject = DefaultControls.CreateText(UIResources);

        SetActive(false);
        Colorize();
        GameObject.transform.SetParent(parent);

        Text text = GameObject.GetComponent<Text>();
        text.text = name;
        text.color = Color;
        text.fontSize = 16;
        text.alignment = TextAnchor.MiddleCenter;
    }
}