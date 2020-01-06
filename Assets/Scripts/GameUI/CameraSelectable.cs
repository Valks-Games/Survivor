using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSelectable : MonoBehaviour
{
    public bool Initialized;
    public Renderer[] Renderers;

    public Bounds GetBounds()
    {
        Bounds bounds = new Bounds();

        for (int i = 0; i < Renderers.Length; i++)
        {
            if (bounds.center == Vector3.zero)
            {
                bounds = Renderers[0].bounds;
            } else 
            {
                bounds.Encapsulate(Renderers[i].bounds);
            }
        }
    

        return bounds;
    }

    private void OnEnable()
    {
        if (CameraSelectionManager.Selectables.Contains(this)) return;
        CameraSelectionManager.Selectables.Add(this);
    }

    private void OnDisable()
    {
        CameraSelectionManager.Selectables.Remove(this);
    }
    
}
