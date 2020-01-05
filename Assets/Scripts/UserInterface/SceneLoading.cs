using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoading : MonoBehaviour
{
    public GameObject Background;
    public GameObject ProgressBarFill;

    public Sprite[] backgrounds;

    private Image progressBarFillImage;

    public void Start()
    {
        progressBarFillImage = ProgressBarFill.GetComponent<Image>();
        SetRandomBackground();
        StartCoroutine(LoadScene("Main"));
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
