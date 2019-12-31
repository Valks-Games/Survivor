using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIListener : MonoBehaviour
{
    public static float CurrentGameSpeed = 1f;

    private bool _baseUICreated;
    private GameObject _canvas;
    private GameObject _menu;
    private bool _menuActive;

    public void Start()
    {
        _canvas = GameObject.Find("World Canvas");
        _menu = GameObject.Find("Menu");
        _menu.SetActive(_menuActive);
    }

    public void Update()
    {
        HandleUIPopUps();
        HandleMenu();
        HandleChangeGameSpeed();
    }

    private void HandleChangeGameSpeed()
    {
        if (Input.GetKeyDown(KeyCode.Minus))
        {
            CurrentGameSpeed -= 0.1f;
        }

        if (Input.GetKeyDown(KeyCode.Equals))
        {
            CurrentGameSpeed += 0.1f;
        }

        CurrentGameSpeed = Mathf.Max(0.25f, CurrentGameSpeed);
        CurrentGameSpeed = Mathf.Min(3.00f, CurrentGameSpeed);
    }

    private void HandleMenu()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _menuActive = !_menuActive;
            _menu.SetActive(_menuActive);

            if (Time.timeScale == 0)
            {
                Time.timeScale = CurrentGameSpeed;
            }
            else
            {
                Time.timeScale = 0;
            }
        }
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    private void HandleUIPopUps()
    {
        if (_baseUICreated)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);

            if (hit.collider != null && hit.collider.CompareTag("Base") && !_baseUICreated)
            {
                GameObject panel = Instantiate(Resources.Load("Prefabs/PanelBaseInfo") as GameObject, _canvas.transform);

                Transform stone = panel.transform.Find("Stone");
                Transform wood = panel.transform.Find("Wood");

                stone.GetComponent<TextMeshPro>().text = "Stone: " + 1;
                wood.GetComponent<TextMeshPro>().text = "Wood: " + 1;

                _baseUICreated = true;
            }
        }
    }
}
