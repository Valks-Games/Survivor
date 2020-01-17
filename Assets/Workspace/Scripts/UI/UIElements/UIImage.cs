using UnityEngine;
using UnityEngine.UI;

public class UIImage : UIElement
{
    public override GameObject GameObject { get; }

    public UIImage(string name, Transform parent) : base(name, parent)
    {
        GameObject = DefaultControls.CreateImage(UIResources);

        SetActive(false);
        Colorize();
        GameObject.transform.SetParent(parent);
    }
}