using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIText : UIElement
{
    public TMP_Text text;

    public UIText(string content, Transform parent) : base(parent)
    {
        gameObject.name = content;
        text = gameObject.AddComponent<TextMeshProUGUI>();
        text.text = content;
        text.fontSize = 20;
        text.alignment = TextAlignmentOptions.Center;
    }
}
