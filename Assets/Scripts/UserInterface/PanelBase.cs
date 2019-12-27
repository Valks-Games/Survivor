using UnityEngine;

public class PanelBase : MonoBehaviour
{
    // This class is currently useless at the moment, forgot why I created it in the first place.
    private RectTransform _rt;
    private GameObject _baseOne;

    void Start()
    {
        _rt = GetComponent<RectTransform>();
        _baseOne = GameObject.Find("Base 1");
    }
}
