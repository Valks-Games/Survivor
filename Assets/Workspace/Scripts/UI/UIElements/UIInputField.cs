using UnityEngine;
using UnityEngine.UI;

public class UIInputField : UISelectable
{
    public override GameObject GameObject { get; }

    public UIInputField(string name, Transform parent) : base(name, parent)
    {
        GameObject prefab = Resources.Load<GameObject>("Prefabs/UI/InputField");
        GameObject = GameObject.Instantiate(prefab);

        SetActive(false);
        Colorize();
        GameObject.transform.SetParent(parent);
    }
}