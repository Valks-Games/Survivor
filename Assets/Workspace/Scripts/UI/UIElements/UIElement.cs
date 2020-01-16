using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class UIElement
{
    public static DefaultControls.Resources UIResources = new DefaultControls.Resources();
    public static List<UIElement> UIElements = new List<UIElement>();
    public abstract GameObject GameObject { get; }

    protected Color32 Color;

    public UIElement(string name, Transform parent)
    {
        this.Color = new Color32(255, 255, 255, 255); // Default Color
        this.SetActive(false);
        this.Colorize();
        this.GameObject.transform.SetParent(parent);
        this.SetupDefaultUI();

        UIElement.UIElements.Add(this);
    }

    public virtual void Colorize()
    {

    }

    public virtual void SetActive(bool value)
    {
        GameObject.SetActive(value);
    }

    public virtual void SetupDefaultUI()
    {
        UIResources.knob = Resources.Load<Sprite>("Sprites/UI/Knob");
        UIResources.checkmark = Resources.Load<Sprite>("Sprites/UI/Checkmark");
    }
}