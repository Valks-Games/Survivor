using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIButton : UISelectable
{
    public override GameObject GameObject { get; }
    public Button Instance => (Button)GameObject.GetComponent<Selectable>();

    public UIButton(string name, Transform parent) : base(name, parent)
    {
        GameObject prefab = Resources.Load<GameObject>("Prefabs/UI/Button");
        GameObject = GameObject.Instantiate(prefab);
        GameObject.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = name;

        SetActive(false);
        Colorize();
        GameObject.transform.SetParent(parent);

        SetActive(true);
    }

    public void AddListener(Action action)
    {
        Instance.onClick.AddListener(delegate
        {
            action?.Invoke();
        });
    }
}