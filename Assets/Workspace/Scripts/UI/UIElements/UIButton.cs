using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIButton : UISelectable
{
    public Button Instance => (Button)GameObject.GetComponent<Selectable>();

    public UIButton(GameObject gameObject) : base(gameObject)
    {

    }

    public void AddListener(Action action)
    {
        Instance.onClick.AddListener(delegate
        {
            action?.Invoke();
        });
    }
}