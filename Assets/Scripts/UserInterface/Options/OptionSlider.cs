using UnityEngine;
using UnityEngine.UI;

public class OptionSlider : Option
{
    public Slider Instance => (Slider)selectable;

    public OptionSlider(Selectable selectable, Color32 color) : base(selectable, color)
    {

    }

    public override void Colorize()
    {
        base.Colorize();

        Transform fill = selectable.transform.Find("Fill Area").Find("Fill");
        Transform background = selectable.transform.Find("Background");

        Color32 fillColor = color;
        Color32 backgroundColor = color;

        int reductionFill = 100;
        int reductionBackground = 200;

        fillColor = new Color32((byte)Mathf.Max(0, fillColor.r - reductionFill), (byte)Mathf.Max(0, fillColor.g - reductionFill), (byte)Mathf.Max(0, fillColor.b - reductionFill), fillColor.a);
        backgroundColor = new Color32((byte)Mathf.Max(0, backgroundColor.r - reductionBackground), (byte)Mathf.Max(0, backgroundColor.g - reductionBackground), (byte)Mathf.Max(0, backgroundColor.b - reductionBackground), backgroundColor.a);

        fill.GetComponent<Image>().color = fillColor;
        background.GetComponent<Image>().color = backgroundColor;
    }
}
