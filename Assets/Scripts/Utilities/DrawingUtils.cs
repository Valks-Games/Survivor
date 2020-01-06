using UnityEngine;

public static class DrawingUtils {
    public static void DrawCircle(this GameObject container, Color color, float radius, float lineWidth)
    {
        if (container == null) return;
        if (container.GetComponent<LineRenderer>() != null) return; //Eventually find a better way of doing this so you can have multiple, but for now this works
        var segments = 360;
        var line = container.AddComponent<LineRenderer>();
        line.material = new UnityEngine.Material(Shader.Find("Standard"));;
        line.material.color = color;
        line.useWorldSpace = false;
        line.startWidth = lineWidth;
        line.endWidth = lineWidth;
        line.positionCount = segments + 1;

        var pointCount = segments + 1; 
        var points = new Vector3[pointCount];

        for (int i = 0; i < pointCount; i++)
        {
            var rad = Mathf.Deg2Rad * (i * 360f / segments);
            points[i] = new Vector3(Mathf.Sin(rad) * radius, 0.125f, Mathf.Cos(rad) * radius);
        }

        line.SetPositions(points);
    }
}