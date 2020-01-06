using UnityEngine;
using System.Collections.Generic;

public class CameraSelectionManager : MonoBehaviour {
    
    //Disabled until I find the inputkey for left click (I can't think I know)
    //[Header("Input")]
    //public string InputKey = "Mouse 0";
    [Header("Selection Box")]
    public int SelectionBoxThickness = 1;

    public Color SelectionBoxColor = Color.cyan;
    [Header("Selected Circle")]
    public float SelectedCircleThickness = 0.03f;
    public float SelectedCircleRadius = 0.1f;
    public Color SelectedCircleColor = Color.cyan;
    

    [Header("Textures")]
    private Texture2D _Border;
    public Texture2D Border
    {
        get
        {
            if (_Border == null)
            {
                _Border = new Texture2D(1, 1);
                _Border.SetPixel(0, 0, Color.white);
                _Border.Apply();
            }

            return _Border;
        }
        set
        {
            _Border = value;
        }
    }

    private GameObject colliderEntity;

    bool selectionEnabled;
    Vector3 initialMousePosition;
    private GUIStyle style;

    public static List<CameraSelectable> Selectables = new List<CameraSelectable>(); 
    public static List<GameObject> Selected = new List<GameObject>();

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            selectionEnabled = true;
            initialMousePosition = Input.mousePosition;
        } else if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            selectionEnabled = false;
        }
        
        if (selectionEnabled)
        {
            List<GameObject> clone = new List<GameObject>(Selected);

            Bounds viewport = GetViewportBounds(Camera.main, initialMousePosition, Input.mousePosition);


            foreach(CameraSelectable selectable in Selectables)
            {

                if (viewport.Contains(Camera.main.WorldToViewportPoint(selectable.transform.position)))
                {
                    
                    Selected.Add(selectable.gameObject);
                    selectable.gameObject.DrawCircle(SelectedCircleColor, SelectedCircleRadius, SelectedCircleThickness);
                    selectable.Initialized = true;
                }
                else 
                {
                    if (selectable.GetComponent<LineRenderer>() != null) Destroy(selectable.GetComponent<LineRenderer>());
                    if (Selected.Contains(selectable.gameObject))
                    {
                        selectable.Initialized = false;
                        Selected.Remove(selectable.gameObject);
                    }
                }
            }


            


            
        }
        
    }

private void OnGUI()
    {
        if (selectionEnabled)
        {
            Rect selectionPane = GetSelectionRect(initialMousePosition, Input.mousePosition);
            DrawBorder(selectionPane, SelectionBoxThickness, SelectionBoxColor);

        }

    }

    private void DrawBorder(Rect rect, float thickness, Color color)
    {
        DrawRectOutline(new Rect(rect.xMin, rect.yMin, rect.width, thickness), color);
        DrawRectOutline(new Rect(rect.xMin, rect.yMin, thickness, rect.height), color);
        DrawRectOutline(new Rect(rect.xMax - thickness, rect.yMin, thickness, rect.height), color);
        DrawRectOutline(new Rect(rect.xMin, rect.yMax - thickness, rect.width, thickness), color);
    }

    private void DrawRectOutline(Rect rect, Color color)
    {
        GUI.color = color;
        GUI.DrawTexture(rect, Border);
        GUI.color = Color.white;
    }

    private Rect GetSelectionRect(Vector3 v1, Vector3 v2)
    {
        v1.y = Screen.height - v1.y;
        v2.y = Screen.height - v2.y;

        Vector3 minBounds = Vector3.Min(v1, v2);
        Vector3 maxBounds = Vector3.Max(v1, v2);

        return Rect.MinMaxRect(minBounds.x, minBounds.y, maxBounds.x, maxBounds.y);
    }


    private Bounds GetViewportBounds(Camera view, Vector3 firstPos, Vector3 secondPos)
    {
        firstPos = view.ScreenToViewportPoint(firstPos);
        secondPos = view.ScreenToViewportPoint(secondPos);
        Vector3 min = Vector3.Min(firstPos, secondPos);
        Vector3 max = Vector3.Max(firstPos, secondPos);
        min.z = view.nearClipPlane;
        max.z = view.farClipPlane;

        Bounds endResult = new Bounds();
        endResult.SetMinMax(min, max);
        
        return endResult;
    }



}