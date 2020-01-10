using UnityEngine;
using UnityEngine.UI;

public class UIImage : UIElement
{
    public override GameObject GameObject { get; } = DefaultControls.CreateImage(new DefaultControls.Resources());

    public UIImage(string name, Transform parent) : base(name, parent)
    {

    }
}