using UnityEngine;
using UnityEngine.UI;

public class UIToggle : UISelectable
{
    public override GameObject GameObject { get; } = DefaultControls.CreateToggle(UIResources);
    public Toggle Instance => (Toggle)GameObject.GetComponent<Selectable>();
    public string Path;

    public UIToggle(string name, Transform parent) : base(name, parent)
    {
        Text componentText = GameObject.transform.Find("Label").GetComponent<Text>();
        componentText.color = Color;
        componentText.text = name;
    }

    public override void SetupDefaultUI()
    {
        base.SetupDefaultUI();
        UIResources.standard = Resources.Load<Sprite>("Sprites/UI/Background");
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