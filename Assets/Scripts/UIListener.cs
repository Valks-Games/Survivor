using TMPro;
using UnityEngine;

public class UIListener : MonoBehaviour
{
    private bool _baseUICreated;
    private GameObject _canvas;

    public void Start()
    {
        _canvas = GameObject.Find("World Canvas");
    }

    public void Update()
    {
        HandleUIPopUps();
    }

    private void HandleUIPopUps()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);

            if (hit.collider != null)
            {
                if (hit.collider.tag.Equals("Base"))
                {
                    if (!_baseUICreated)
                    {
                        GameObject panel = Resources.Load("Prefabs/PanelBaseInfo") as GameObject;
                        GameObject panelGO = Instantiate(panel, _canvas.transform);

                        Debug.Log(panelGO.transform.childCount);

                        Transform stone = panelGO.transform.Find("Stone");
                        Transform wood = panelGO.transform.Find("Wood");

                        stone.GetComponent<TextMeshPro>().text = "Stone: " + 1;
                        wood.GetComponent<TextMeshPro>().text = "Wood: " + 1;

                        _baseUICreated = true;
                    }
                }
            }
        }
    }
}
