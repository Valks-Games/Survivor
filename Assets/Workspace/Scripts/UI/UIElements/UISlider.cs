using System;
using UnityEngine;
using UnityEngine.UI;

public class UISlider : UISelectable
{
    public override GameObject GameObject { get; }
    public Slider Instance => (Slider)GameObject.GetComponent<Selectable>();

    public UISlider(string name, Transform parent) : base(name, parent)
    {
        GameObject prefab = Resources.Load<GameObject>("Prefabs/UI/Slider");
        GameObject = GameObject.Instantiate(prefab);

        SetActive(false);
        Colorize();
        GameObject.transform.SetParent(parent);

        SetActive(true);
    }

    public void AddListener(Action action)
    {
        Instance.onValueChanged.AddListener(delegate
        {
            action?.Invoke();
        });
    }

    public override void Colorize()
    {
        base.Colorize();

        Transform fill = GameObject.transform.Find("Fill Area").Find("Fill");
        Transform background = GameObject.transform.Find("Background");

        fill.GetComponent<Image>().color = Utils.ModifyColor(Color, -100);
        background.GetComponent<Image>().color = Utils.ModifyColor(Color, -200);
    }

    public void Save(string path, float value)
    {
        if (!PlayerPrefs.HasKey(path))
        {
            PlayerPrefs.SetFloat(path, value);
        }

        Instance.value = PlayerPrefs.GetFloat(path);
    }
}