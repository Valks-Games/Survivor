using UnityEngine;
using UnityEngine.UI;

public class UIToggle : UISelectable
{
    public override GameObject GameObject { get; } = DefaultControls.CreateToggle(new DefaultControls.Resources());
    public Toggle Instance => (Toggle)GameObject.GetComponent<Selectable>();
    public string Path;

    public UIToggle(string name, Transform parent) : base(name, parent)
    {
        Text componentText = GameObject.transform.Find("Label").GetComponent<Text>();
        componentText.color = Color;
        componentText.text = name;
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