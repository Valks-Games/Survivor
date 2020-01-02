using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimitFPS : MonoBehaviour
{
    public void Start()
    {
        Application.targetFrameRate = 60;
    }
}
