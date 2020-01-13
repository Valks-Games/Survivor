using UnityEngine;
using UnityEngine.UI;

public class UIText : UIElement
{
    public override GameObject GameObject { get; } = DefaultControls.CreateText(new DefaultControls.Resources());

    public UIText(string name, Transform parent) : base(name, parent)
    {
        Text text = GameObject.GetComponent<Text>();
        text.text = name;
        text.color = Color;
        text.fontSize = 16;
        text.alignment = TextAnchor.MiddleCenter;
    }
}