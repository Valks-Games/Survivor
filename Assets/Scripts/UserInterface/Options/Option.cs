using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Option
{
    public Selectable selectable;
    public Color32 color;

    public Option(Selectable selectable, Color32 color)
    {
        this.selectable = selectable;
        this.color = color;

        this.selectable.gameObject.SetActive(false);
        this.Colorize();
        this.selectable.gameObject.SetActive(true);
    }

    public virtual void Colorize()
    {
        ColorBlock colorBlock = selectable.colors;
        colorBlock.normalColor = color;
        colorBlock.highlightedColor = color;
        colorBlock.selectedColor = color;
        colorBlock.pressedColor = color;
        selectable.colors = colorBlock;
    }
}
