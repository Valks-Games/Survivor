using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public void Awake()
    {
        SetResolution();
        SetQuality();
    }

    private void SetResolution()
    {
        Resolution[] resolutions = Screen.resolutions;
        int index = PlayerPrefs.GetInt("options.resolution");
        Screen.SetResolution(resolutions[index].width, resolutions[index].height, Screen.fullScreen);
    }

    private void SetQuality()
    {
        int index = PlayerPrefs.GetInt("options.quality");
        QualitySettings.SetQualityLevel(index);
    }

    public void StartGame()
    {
        LoadSceneAsync("Loading");
        Destroy(MenuMusicManager.MenuMusic);
    }

    public void LoadRoadmap()
    {
        LoadSceneAsync("Roadmap");
    }

    public void LoadOptions()
    {
        LoadSceneAsync("Options");
    }

    public void LoadCredits()
    {
        LoadSceneAsync("Credits");
    }

    public void QuitGame()
    {
#if UNITYEDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void LoadSceneAsync(string scene)
    {
        StartCoroutine(LoadAsynchronously(scene));
    }

    private IEnumerator LoadAsynchronously(string scene)
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Single);
        asyncOperation.allowSceneActivation = false;

        while (!asyncOperation.isDone)
        {
            if (asyncOperation.progress >= 0.9f)
            {
                asyncOperation.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}