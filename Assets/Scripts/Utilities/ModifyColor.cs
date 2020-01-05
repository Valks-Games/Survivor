using UnityEngine;

public class Utils
{
    public static Color32 ModifyColor(Color32 color, int offset)
    {
        return new Color32((byte)Mathf.Clamp(color.r + offset, 0, 255), (byte)Mathf.Clamp(color.g + offset, 0, 255), (byte)Mathf.Clamp(color.b + offset, 0, 255), color.a);
    }
}
