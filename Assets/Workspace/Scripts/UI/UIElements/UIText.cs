using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIText : UIElement
{
    public override GameObject GameObject { get; }

    public UIText(string name, Transform parent) : base(name, parent)
    {
        GameObject prefab = Resources.Load<GameObject>("Prefabs/UI/Text");
        GameObject = GameObject.Instantiate(prefab);

        SetActive(false);
        Colorize();
        GameObject.transform.SetParent(parent);

        TextMeshProUGUI text = GameObject.GetComponent<TextMeshProUGUI>();
        text.text = name;
        text.color = Color;
        text.fontSize = 16;
        text.alignment = TextAlignmentOptions.Center;

        SetActive(true);
    }
}