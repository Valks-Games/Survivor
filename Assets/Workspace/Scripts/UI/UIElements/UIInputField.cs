using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class UIInputField : UIElement
{
    public override GameObject GameObject { get; }
    public TMP_InputField Instance => GameObject.GetComponent<TMP_InputField>();

    public UIInputField(string name, Transform parent) : base(name, parent)
    {
        GameObject prefab = Resources.Load<GameObject>("Prefabs/UI/InputField");
        GameObject = GameObject.Instantiate(prefab);

        SetActive(false);
        Colorize();
        GameObject.transform.SetParent(parent);

        SetActive(true);
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