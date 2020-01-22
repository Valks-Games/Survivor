using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class UIElement
{
    public static List<UIElement> UIElements = new List<UIElement>();
    public GameObject GameObject;

    protected Color32 Color;

    public UIElement(GameObject gameObject)
    {
        if (gameObject == null)
        {
            Debug.Log("A UIElement has not been setup properly in the '" + SceneManager.GetActiveScene().name + "' scene.");
            return;
        }

        GameObject = gameObject;
        Color = new Color32(255, 255, 255, 255); // Default Color

        SetActive(false);
        Colorize();
        SetActive(true);

        UIElement.UIElements.Add(this);
    }

    public virtual void Colorize()
    {
    }

    public virtual void SetActive(bool value)
    {
        GameObject.SetActive(value);
    }
}