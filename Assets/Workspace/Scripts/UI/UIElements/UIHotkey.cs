using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIHotkey : UIElement
{
    private TMP_InputField instance;
    public static List<UIHotkey> UIHotkeys = new List<UIHotkey>();
    public bool Selected = true;

    public UIHotkey(GameObject gameObject) : base(gameObject)
    {
        instance = gameObject.transform.Find("InputField").GetComponent<TMP_InputField>();

        AddListenerOnSelect();
        AddListenerOnDeselect();

        UIHotkeys.Add(this);
    }

    public void UpdateText(string text)
    {
        instance.text = text;
    }

    public void ClearText()
    {
        instance.text = "";
    }

    private void AddListenerOnSelect()
    {
        instance.onSelect.AddListener(delegate
        {
            Debug.Log("Selected");
            Selected = true;
        });
    }

    private void AddListenerOnDeselect()
    {
        instance.onDeselect.AddListener(delegate
        {
            Debug.Log("Deselected");
            Selected = false;
        });
    }
}