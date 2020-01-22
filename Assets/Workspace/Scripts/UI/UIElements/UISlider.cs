using System;
using UnityEngine;
using UnityEngine.UI;

public class UISlider : UISelectable
{
    public Slider Instance => (Slider)GameObject.GetComponent<Selectable>();

    public UISlider(GameObject gameObject) : base(gameObject)
    {

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