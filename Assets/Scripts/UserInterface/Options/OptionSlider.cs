using UnityEngine;
using UnityEngine.UI;

public class OptionSlider : Option
{
    public Slider Instance => (Slider)selectable;

    public OptionSlider(string name) : base(name)
    {
        
    }

    public override void Colorize()
    {
        base.Colorize();

        Transform fill = selectable.transform.Find("Fill Area").Find("Fill");
        Transform background = selectable.transform.Find("Background");

        fill.GetComponent<Image>().color = Utils.ModifyColor(color, -100);
        background.GetComponent<Image>().color = Utils.ModifyColor(color, -200);
    }
}
