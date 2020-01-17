using UnityEngine;
using UnityEngine.UI;

public class UIDropdown : UISelectable
{
    public override GameObject GameObject { get; }
    public Dropdown Instance => (Dropdown)GameObject.GetComponent<Selectable>();

    public UIDropdown(string name, Transform parent) : base(name, parent)
    {
        GameObject = DefaultControls.CreateDropdown(UIResources);
        GameObject.GetComponent<Dropdown>().options.Clear();
        GameObject.transform.Find("Template").GetComponent<ScrollRect>().scrollSensitivity = 45;

        SetActive(false);
        Colorize();
        GameObject.transform.SetParent(parent);
    }

    public override void Colorize()
    {
        base.Colorize();

        Transform template = GameObject.transform.Find("Template");
        Transform scrollbar = template.Find("Scrollbar");
        Transform handle = scrollbar.Find("Sliding Area").Find("Handle");
        Transform item = template.Find("Viewport").Find("Content").Find("Item");
        Transform itemBackground = item.Find("Item Background");
        Transform itemCheckmark = item.Find("Item Checkmark");
        Transform itemLabel = item.Find("Item Label");

        scrollbar.GetComponent<Image>().color = Utils.ModifyColor(Color, -250);
        handle.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        template.GetComponent<Image>().color = Utils.ModifyColor(Color, -200);
        itemBackground.GetComponent<Image>().color = Utils.ModifyColor(Color, -200);
        itemCheckmark.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        itemLabel.GetComponent<Text>().color = Utils.ModifyColor(Color, 100);
    }

    public void Save(string path, int value)
    {
        if (!PlayerPrefs.HasKey(path))
        {
            PlayerPrefs.SetInt(path, value);
        }

        Instance.value = PlayerPrefs.GetInt(path);
    }
}