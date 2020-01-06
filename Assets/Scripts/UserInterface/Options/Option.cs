using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Option
{
    public Selectable selectable;
    public Color32 color;

    public Option(string name)
    {
        this.selectable = GameObject.Find(name).GetComponent<Selectable>();
        this.color = new Color32(255, 255, 255, 255); // Default Color

        this.selectable.gameObject.SetActive(false); // Prevents visual color flash change
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
