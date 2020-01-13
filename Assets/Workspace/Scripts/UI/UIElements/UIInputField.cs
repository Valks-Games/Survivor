using UnityEngine;
using UnityEngine.UI;

public class UIInputField : UISelectable
{
    public override GameObject GameObject { get; } = DefaultControls.CreateInputField(new DefaultControls.Resources());

    public UIInputField(string name, Transform parent) : base(name, parent)
    {

    }
}