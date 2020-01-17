using UnityEngine;
using UnityEngine.UI;

public class UIInputField : UISelectable
{
    public override GameObject GameObject { get; }

    public UIInputField(string name, Transform parent) : base(name, parent)
    {
        GameObject = DefaultControls.CreateInputField(UIResources);

        SetActive(false);
        Colorize();
        GameObject.transform.SetParent(parent);
    }
}