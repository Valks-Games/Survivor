using UnityEngine;
using UnityEngine.UI;

public class UIButton : UISelectable
{
    public override GameObject GameObject { get; } = DefaultControls.CreateButton(new DefaultControls.Resources());

    public UIButton(string name, Transform parent) : base(name, parent)
    {

    }
}