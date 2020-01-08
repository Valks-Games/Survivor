using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Option
{
    public static List<Option> Options = new List<Option>();
    public Selectable Selectable;
    public Color32 Color;

    public Option(string name)
    {
        this.Selectable = GameObject.Find(name).GetComponent<Selectable>();
        this.Color = new Color32(255, 255, 255, 255); // Default Color
        this.SetActive(false);
        this.Colorize();

        Option.Options.Add(this);
    }

    public virtual void Colorize()
    {
        ColorBlock colorBlock = Selectable.colors;
        colorBlock.normalColor = Color;
        colorBlock.highlightedColor = Color;
        colorBlock.selectedColor = Color;
        colorBlock.pressedColor = Color;
        Selectable.colors = colorBlock;
    }

    public virtual void SetActive(bool value)
    {
        this.Selectable.gameObject.SetActive(value);
    }
}