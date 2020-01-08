using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class UIButton : UIElement
{
    public Button button;
    public Image image;
    public RectTransform rectTransform;

    public UIButton(string content, Transform parent) : base(parent)
    {
        gameObject.name = content;
        rectTransform = gameObject.AddComponent<RectTransform>();
        image = gameObject.AddComponent<Image>();
        image.type = Image.Type.Sliced;
        button = gameObject.AddComponent<Button>();
        rectTransform.sizeDelta = new Vector2(120, 30);
        image.color = new Color32(50, 50, 50, 255);

        UIText text = new UIText("Test", transform);
    }
}