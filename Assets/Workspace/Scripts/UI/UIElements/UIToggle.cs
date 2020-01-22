using System;
using UnityEngine;
using UnityEngine.UI;

public class UIToggle : UISelectable
{
    public Toggle Instance => (Toggle)GameObject.GetComponent<Selectable>();

    public UIToggle(GameObject gameObject) : base(gameObject)
    {

    }

    public override void Colorize()
    {
        base.Colorize();

        Text componentText = GameObject.transform.Find("Label").GetComponent<Text>();
        componentText.color = Color;
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