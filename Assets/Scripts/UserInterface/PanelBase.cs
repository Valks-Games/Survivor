using UnityEngine;

public class PanelBase : MonoBehaviour
{
    // This class is currently useless at the moment, forgot why I created it in the first place.
    private RectTransform rt;
    private GameObject baseOne;

    void Start()
    {
        rt = GetComponent<RectTransform>();
        baseOne = GameObject.Find("Base 1");
    }
}
