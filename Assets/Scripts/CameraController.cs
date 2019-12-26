using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Camera _cam;
    private WorldGenerator _worldGenerator;

    public float Speed = 100f;
    public float ScrollSpeed = 500f;
    public float ScrollLerp = 0.05f;
    public float TrackingLerp = 0.02f;
    public float DragSpeed = 500f;

    private Transform _trackingTarget = null;
    private Vector3 _dragOrigin;

    private float _zoom = 0f;
    private float _currentZoom = 0f;

    private float _lastTimeClicked;

    public void Start()
    {
        _cam = GetComponent<Camera>();
        _worldGenerator = GameObject.Find("World").GetComponent<WorldGenerator>();
        _lastTimeClicked = Time.time;
    }

    public void Update()
    {
        HandleCamera();   
    }

    private void HandleCamera() {
        float inputHorz = Input.GetAxis("Horizontal");
        float inputVert = Input.GetAxis("Vertical");
        float inputScroll = Input.GetAxis("Mouse ScrollWheel");

        HandleMoveKeys(inputHorz, inputVert);
        HandleTracking(inputHorz, inputVert);
        HandleZoom(inputScroll);
        HandleDrag();
        HandleClicking();
    }

    private void HandleMoveKeys(float inputHorz, float inputVert) {
        transform.position += new Vector3(inputHorz * Speed * Time.deltaTime, inputVert * Speed * Time.deltaTime, 0);
    }

    private void HandleZoom(float inputScroll) {
        _zoom -= inputScroll;
        _zoom = Mathf.Clamp(_zoom, -0.4f, 2f);
        _currentZoom = Mathf.Lerp(_currentZoom, _zoom, ScrollLerp);
        _cam.orthographicSize = 5 + _currentZoom * ScrollSpeed;
    }

    private void HandleDrag() {
        Cursor.lockState = CursorLockMode.None;

        if (Input.GetMouseButtonDown(0)) {
            _dragOrigin = Input.mousePosition;
            
            return;
        }

        if (!Input.GetMouseButton(0)) return;

        Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - _dragOrigin);
        Vector3 move = new Vector3(-pos.x * DragSpeed, -pos.y * DragSpeed, 0);

        transform.Translate(move, Space.World);

        Cursor.lockState = CursorLockMode.Confined;
    }

    private void HandleTracking(float inputHorz, float inputVert) {
        if (inputHorz != 0 || inputVert != 0)
            _trackingTarget = null;

        if (Input.GetMouseButtonDown(0)) {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);

            if (hit.collider != null)
            {
                _trackingTarget = hit.collider.transform;
                StartCoroutine(SlowDownLerp());
                _zoom = 0;
            }
        }

        if (_trackingTarget != null) {
            transform.position = Vector3.Lerp(transform.position, new Vector3(_trackingTarget.position.x, _trackingTarget.position.y, -10), TrackingLerp);
        }
    }

    private void HandleClicking() {
        if (Input.GetMouseButtonDown(0)) {
            if (Time.time - _lastTimeClicked < 0.2f) {
                //Double click logic here
            }

            _lastTimeClicked = Time.time;
        }
    }

    private IEnumerator SlowDownLerp() {
        ScrollLerp = 0.02f;
        TrackingLerp = 0.02f;
        yield return new WaitForSeconds(2.5f);
        ScrollLerp = 0.05f;
        TrackingLerp = 0.05f;
    }
}