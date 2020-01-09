using UnityEngine;
using UnityEngine.UI;

public class OptionSlider : Option
{
    public Slider Instance => (Slider)Selectable;

    public OptionSlider(string name) : base(name)
    {

    }

    public override void Colorize()
    {
        base.Colorize();

        Transform fill = Selectable.transform.Find("Fill Area").Find("Fill");
        Transform background = Selectable.transform.Find("Background");

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