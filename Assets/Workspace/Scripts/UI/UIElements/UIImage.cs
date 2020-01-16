using UnityEngine;
using UnityEngine.UI;

public class UIImage : UIElement
{
    public override GameObject GameObject { get; } = DefaultControls.CreateImage(UIResources);

    public UIImage(string name, Transform parent) : base(name, parent)
    {

    }
}