using UnityEngine;
using UnityEngine.UI;

public class OptionToggle : Option
{
    public Toggle Instance => (Toggle)Selectable;
    public string Path;

    public OptionToggle(string name) : base(name)
    {
        DefaultControls.Resources uiResources = new DefaultControls.Resources();
        this.GameObject = DefaultControls.CreateToggle(uiResources);
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