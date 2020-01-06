using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BottomPanel : MonoBehaviour
{
    public void Start()
    {
        GameObject screenCanvas = GameObject.Find("Screen Canvas");
        Transform panel = screenCanvas.transform.Find("Panel");

        UIHorizontalLayout elements = new UIHorizontalLayout("Elements", panel);
        UIVerticalLayout entityInfo = new UIVerticalLayout("Entity Information", elements.transform);

        UIText name = new UIText("Name", entityInfo.transform);
        UIText stone = new UIText("Stone", entityInfo.transform);
        UIText wood = new UIText("Wood", entityInfo.transform);
    }
}
