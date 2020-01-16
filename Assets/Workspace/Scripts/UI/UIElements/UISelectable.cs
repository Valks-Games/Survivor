using UnityEngine;
using UnityEngine.UI;

public class UISelectable : UIElement
{
    public override GameObject GameObject { get; }

    public UISelectable(string name, Transform parent) : base(name, parent)
    {

    }

    public override void SetupDefaultUI()
    {
        base.SetupDefaultUI();
    }

    public override void Colorize()
    {
        base.Colorize();

        Selectable selectable = GameObject.GetComponent<Selectable>();
        ColorBlock colorBlock = selectable.colors;
        colorBlock.normalColor = Color;
        colorBlock.highlightedColor = Color;
        colorBlock.selectedColor = Color;
        colorBlock.pressedColor = Color;
        selectable.colors = colorBlock;
    }

    public override void SetActive(bool value)
    {
        base.SetActive(value);
    }
}