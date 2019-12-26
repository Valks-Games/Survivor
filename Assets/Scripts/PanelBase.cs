using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelBase : MonoBehaviour
{
    private RectTransform _rt;
    private GameObject _baseOne;

    void Start()
    {
        _rt = GetComponent<RectTransform>();
        _baseOne = GameObject.Find("Base 1");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
