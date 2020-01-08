using UnityEngine;
using UnityEngine.UI;

public class OptionToggle : Option
{
    public Toggle Instance => (Toggle)Selectable;
    public string Path;

    public OptionToggle(string name, string path, bool value = true) : base(name)
    {
        Save(path, value);
        Instance.isOn = PlayerPrefsX.GetBool(path);
    }

    public void Save(string path, bool value)
    {
        if (!PlayerPrefs.HasKey(path))
        {
            PlayerPrefsX.SetBool(path, value);
        }
    }
}