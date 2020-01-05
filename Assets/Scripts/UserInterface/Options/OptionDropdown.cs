using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionDropdown : Option
{
    public Dropdown Instance => (Dropdown)selectable;

    public OptionDropdown(string name) : base(name)
    {

    }

    public override void Colorize()
    {
        base.Colorize();

        Transform template = selectable.transform.Find("Template");
        Transform scrollbar = template.Find("Scrollbar");
        Transform handle = scrollbar.Find("Sliding Area").Find("Handle");
        Transform item = template.Find("Viewport").Find("Content").Find("Item");
        Transform itemBackground = item.Find("Item Background");
        Transform itemCheckmark = item.Find("Item Checkmark");
        Transform itemLabel = item.Find("Item Label");

        scrollbar.GetComponent<Image>().color = Utils.ModifyColor(color, -250);
        handle.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        template.GetComponent<Image>().color = Utils.ModifyColor(color, -200);
        itemBackground.GetComponent<Image>().color = Utils.ModifyColor(color, -200);
        itemCheckmark.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        itemLabel.GetComponent<Text>().color = Utils.ModifyColor(color, 100);
    }
}
