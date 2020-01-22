using System;
using TMPro;
using UnityEngine;

public class UIInputField : UIElement
{
    public TMP_InputField Instance => GameObject.GetComponent<TMP_InputField>();

    public UIInputField(GameObject gameObject) : base(gameObject)
    {

    }

    public void UpdatePlaceholder(string newText)
    {
        GameObject.transform.Find("Text Area").Find("Placeholder").GetComponent<TextMeshProUGUI>().text = newText;
    }

    public void AddListener(Action action)
    {
        Instance.onValueChanged.AddListener(delegate
        {
            action?.Invoke();
        });
    }
}