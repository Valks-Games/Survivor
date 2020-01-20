using System;
using UnityEngine;
using UnityEngine.UI;

public class UIToggle : UISelectable
{
    public override GameObject GameObject { get; }
    public Toggle Instance => (Toggle)GameObject.GetComponent<Selectable>();

    public UIToggle(string name, Transform parent) : base(name, parent)
    {
        GameObject prefab = Resources.Load<GameObject>("Prefabs/UI/Toggle");
        GameObject = GameObject.Instantiate(prefab);

        SetActive(false);
        Colorize();
        GameObject.transform.SetParent(parent);

        Text componentText = GameObject.transform.Find("Label").GetComponent<Text>();
        componentText.color = Color;
        componentText.text = name;

        SetActive(true);
    }

    public void AddListener(Action action)
    {
        Instance.onValueChanged.AddListener(delegate
        {
            action?.Invoke();
        });
    }

    public void Save(string path, bool value)
    {
        if (!PlayerPrefs.HasKey(path))
        {
            PlayerPrefsX.SetBool(path, value);
        }

        Instance.isOn = PlayerPrefsX.GetBool(path);
    }
}