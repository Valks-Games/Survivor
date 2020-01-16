using UnityEngine;
using UnityEngine.UI;

public class UIButton : UISelectable
{
    public override GameObject GameObject { get; } = DefaultControls.CreateButton(UIResources);

    public UIButton(string name, Transform parent) : base(name, parent)
    {
        GameObject.transform.Find("Text").GetComponent<Text>().text = name;
    }
}