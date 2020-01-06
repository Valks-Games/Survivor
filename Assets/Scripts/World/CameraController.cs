using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Camera cam;
    private WorldGenerator worldGenerator;

    public float SpeedPan = 3f;
    public float SpeedScroll = 10f;
    public float DragSpeed = 500f;

    public float ScrollLerp = 0.05f;
    public float TrackingLerp = 0.02f;
    

    private Transform trackingTarget = null;
    private Vector3 dragOrigin;

    private float zoom = 0f;
    private float currentZoom = 0f;

    private float lastTimeClicked;

    public void Awake()
    {
        SpeedPan = Options.SensitivityPan;
        SpeedScroll = Options.SensitivityZoom;
    }

    public void Start()
    {
        cam = GetComponent<Camera>();
        worldGenerator = GameObject.Find("World").GetComponent<WorldGenerator>();
        lastTimeClicked = Time.time;
    }

    public void Update()
    {
        HandleCamera();
    }

    private void HandleCamera()
    {
        float inputHorz = Input.GetAxis("Horizontal");
        float inputVert = Input.GetAxis("Vertical");
        float inputScroll = Input.GetAxis("Mouse ScrollWheel");

        HandleMoveKeys(inputHorz, inputVert);
        HandleTracking(inputHorz, inputVert);
        HandleZoom(inputScroll);
        //HandleDrag(); Removed because of adding selection
        
        HandleClicking();
    }

    private void HandleMoveKeys(float inputHorz, float inputVert)
    {
        transform.position += new Vector3(inputHorz * SpeedPan * Time.deltaTime, 0, inputVert * SpeedPan * Time.deltaTime);
    }

    private void HandleZoom(float inputScroll)
    {
        zoom += inputScroll;
        zoom = Mathf.Clamp(zoom, 0f, 0.8f);
        currentZoom = Mathf.Lerp(currentZoom, zoom, ScrollLerp);

        Vector3 pos = transform.position;
        pos.y = 10 - currentZoom * SpeedScroll;
        transform.position = pos; 
    }

    private void HandleDrag()
    {
        Cursor.lockState = CursorLockMode.None;

        if (Input.GetMouseButtonDown(0))
        {
            dragOrigin = Input.mousePosition;

            return;
        }

        if (!Input.GetMouseButton(0)) return;

        Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - dragOrigin);
        Vector3 move = new Vector3(-pos.x * DragSpeed, -pos.y * DragSpeed, 0);

        transform.Translate(move, Space.World);

        Cursor.lockState = CursorLockMode.Confined;
    }

    private void HandleTracking(float inputHorz, float inputVert)
    {
        if (inputHorz != 0 || inputVert != 0)
            trackingTarget = null;

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);

            if (hit.collider != null)
            {

                trackingTarget = hit.collider.transform;
                StartCoroutine(SlowDownLerp());
                zoom = 0;
            }
        }

        if (trackingTarget != null)
            transform.position = Vector3.Lerp(transform.position, new Vector3(trackingTarget.position.x, trackingTarget.position.y, -10), TrackingLerp);
    }

    private void HandleClicking()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (Time.time - lastTimeClicked < 0.2f)
            {
                //Double click logic here
            }

            lastTimeClicked = Time.time;
        }


    }

    private IEnumerator SlowDownLerp(float value = 0.02f, float transition = 1f, int iterations = 64)
    {
        float orig = ScrollLerp;

        ScrollLerp = value;
        TrackingLerp = value;

        float increment = (value - orig) / iterations;
        float timeIncrement = transition / iterations;

        for (int i = 0; i < iterations; i++)
        {
            ScrollLerp += increment;
            TrackingLerp += increment;

            yield return new WaitForSeconds(timeIncrement);
        }
    }
}