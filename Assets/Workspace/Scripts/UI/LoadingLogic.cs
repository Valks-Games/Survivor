using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class LoadingLogic : MonoBehaviour
{
    public GameObject Background;
    public GameObject ProgressBarFill;
    public GameObject Title;
    public GameObject LoadingTips;

    public Sprite[] backgrounds;
    private string[] titles;
    private string[] tips;

    private Image progressBarFillImage;
    private TextMeshProUGUI titleText;
    private TextMeshProUGUI textTips;

    public void Start()
    {
        progressBarFillImage = ProgressBarFill.GetComponent<Image>();
        SetRandomBackground();
        StartCoroutine(LoadScene("Main"));

        titles = new string[] { "Survivor", "Unknown", "The Expanse" };
        titleText = Title.GetComponent<TextMeshProUGUI>();
        titleText.text = titles[Random.Range(0, titles.Length)];

        tips = new string[] {
            "Click on a colonist to select it and give it commands.",
            "It may be unwise to alert others of your existance."
        };
        textTips = LoadingTips.GetComponent<TextMeshProUGUI>();
        textTips.text = tips[Random.Range(0, tips.Length)];
    }

    private void SetRandomBackground()
    {
        Background.GetComponent<Image>().sprite = backgrounds[Random.Range(0, backgrounds.Length)];
    }

    private IEnumerator LoadScene(string scene)
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(scene);
        asyncOperation.allowSceneActivation = false;

        while (!asyncOperation.isDone)
        {
            progressBarFillImage.fillAmount = asyncOperation.progress;

            if (asyncOperation.progress >= 0.9f)
            {
                asyncOperation.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}