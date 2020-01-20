using UnityEngine;

public class UIImage : UIElement
{
    public override GameObject GameObject { get; }

    public UIImage(string name, Transform parent) : base(name, parent)
    {
        GameObject prefab = Resources.Load<GameObject>("Prefabs/UI/Image");
        GameObject = GameObject.Instantiate(prefab);

        SetActive(false);
        Colorize();
        GameObject.transform.SetParent(parent);

        SetActive(true);
    }
}