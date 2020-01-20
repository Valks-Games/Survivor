using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils : MonoBehaviour
{
    /// <Summary>Sets the active state of the parent and the children</Summary>
    public static void SetChildrenActive(Transform parent, bool active)
    {
        parent.gameObject.SetActive(active);

        foreach (Transform child in parent)
        {
            child.gameObject.SetActive(active);
        }
    }

    public static Color32 ModifyColor(Color32 color, int offset)
    {
        return new Color32((byte)Mathf.Clamp(color.r + offset, 0, 255), (byte)Mathf.Clamp(color.g + offset, 0, 255), (byte)Mathf.Clamp(color.b + offset, 0, 255), color.a);
    }
}
