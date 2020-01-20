using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIListener : MonoBehaviour
{
    public static float CurrentGameSpeed = 1f;

    public GameObject SectionMenu;

    private UIButton buttonBackToMenu;
    private UIButton buttonOptions;

    private bool baseUICreated;
    private GameObject canvas;
    private bool menuActive;

    public void Start()
    {
        canvas = GameObject.Find("World Canvas");
        SectionMenu.SetActive(menuActive);

        buttonBackToMenu = new UIButton("Back to Menu", SectionMenu.transform);
        buttonBackToMenu.Instance.onClick.AddListener(delegate
        {
            Time.timeScale = CurrentGameSpeed;
            SceneManager.LoadScene("Menu");
        });

        buttonOptions = new UIButton("Options", SectionMenu.transform);
        buttonOptions.Instance.onClick.AddListener(delegate
        {
            Debug.Log("TODO: Create options menu here.");
        });
    }

    public void Update()
    {
        //HandleUIPopUps();

        HandleMenu();
        HandleChangeGameSpeed();

        if (Input.GetKeyDown(KeyCode.R))
        {
            Time.timeScale = CurrentGameSpeed;
            SceneManager.LoadScene("Main");
        }
    }

    private void HandleChangeGameSpeed()
    {
        if (Input.GetKeyDown(KeyCode.Minus))
        {
            CurrentGameSpeed -= 0.1f;
            Time.timeScale = CurrentGameSpeed;
        }

        if (Input.GetKeyDown(KeyCode.Equals))
        {
            CurrentGameSpeed += 0.1f;
            Time.timeScale = CurrentGameSpeed;
        }

        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            CurrentGameSpeed = 1f;
            Time.timeScale = CurrentGameSpeed;
        }

        CurrentGameSpeed = Mathf.Max(0.5f, CurrentGameSpeed);
        CurrentGameSpeed = Mathf.Min(3f, CurrentGameSpeed);
    }

    private void HandleMenu()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            menuActive = !menuActive;
            Utils.SetChildrenActive(SectionMenu.transform, menuActive);

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

    private void HandleUIPopUps()
    {
        if (baseUICreated)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);

            if (hit.collider != null && hit.collider.CompareTag("Base") && !baseUICreated)
            {
                GameObject panel = Instantiate(Resources.Load("Prefabs/PanelBaseInfo") as GameObject, canvas.transform);

                Transform stone = panel.transform.Find("Stone");
                Transform wood = panel.transform.Find("Wood");

                stone.GetComponent<TextMeshPro>().text = "Stone: " + 1;
                wood.GetComponent<TextMeshPro>().text = "Wood: " + 1;

                baseUICreated = true;
            }
        }
    }
}