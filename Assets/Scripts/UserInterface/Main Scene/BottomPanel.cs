using UnityEngine;

public class BottomPanel : MonoBehaviour
{
    public void Start()
    {
        GameObject screenCanvas = GameObject.Find("Screen Canvas");
        Transform panel = screenCanvas.transform.Find("Panel");
        //
        UIHorizontalLayout elements = new UIHorizontalLayout("Elements", panel);
        UIVerticalLayout entityInfo = new UIVerticalLayout("Entity Information", elements.transform);

        UIText name = new UIText("Name", entityInfo.transform);
        UIButton button = new UIButton("Button", entityInfo.transform);
    }
}