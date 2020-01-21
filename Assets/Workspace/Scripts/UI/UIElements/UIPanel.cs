using UnityEngine;
using UnityEngine.UI;

public class UIPanel : UIElement
{
    public override GameObject GameObject { get; }

    public UIPanel(string name, Transform parent) : base(name, parent)
    {
        GameObject prefab = Resources.Load<GameObject>("Prefabs/UI/Panel");
        GameObject = GameObject.Instantiate(prefab);

        SetActive(false);
        Colorize();
        GameObject.transform.SetParent(parent);

        SetActive(true);
    }
}