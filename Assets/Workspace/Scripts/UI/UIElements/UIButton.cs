using UnityEngine;
using UnityEngine.UI;

public class UIButton : UISelectable
{
    public override GameObject GameObject { get; }

    public UIButton(string name, Transform parent) : base(name, parent)
    {
        GameObject = DefaultControls.CreateButton(UIResources);
        GameObject.transform.Find("Text").GetComponent<Text>().text = name;

        SetActive(false);
        Colorize();
        GameObject.transform.SetParent(parent);
    }
}