using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneLoading : MonoBehaviour
{
    public Sprite[] backgrounds;
    private Sprite randomBackground;
    [SerializeField]
    private Image _progressBar;
    // Start is called before the first frame update
    void Start()
    {
        randomBackground = backgrounds[Random.Range(0, backgrounds.Length)];
        GetComponent<Image>().sprite = randomBackground;
        StartCoroutine(LoadAsyncOperation());
    }

    IEnumerator LoadAsyncOperation()
    {
        yield return new WaitForSeconds(5);
        AsyncOperation gameLevel = SceneManager.LoadSceneAsync(3);
        while (gameLevel.progress < 1)
        {
            _progressBar.fillAmount = gameLevel.progress;

            yield return new WaitForEndOfFrame();
        }

    }
}
