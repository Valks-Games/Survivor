using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public static bool FirstTime = true;

    public void Awake()
    {
        if (FirstTime)
        {
            FirstTime = false;
            SetResolution();
            SetQuality();
            SetFullscreen();
        }
    }

    private void SetResolution()
    {
        if (!PlayerPrefs.HasKey("options.resolution"))
            return;
        Resolution[] resolutions = Screen.resolutions;
        int index = PlayerPrefs.GetInt("options.resolution") - 1;
        Screen.SetResolution(resolutions[index].width, resolutions[index].height, Screen.fullScreen);
    }

    private void SetQuality()
    {
        if (!PlayerPrefs.HasKey("options.quality"))
            return;
        int index = PlayerPrefs.GetInt("options.quality");
        QualitySettings.SetQualityLevel(index);
    }

    private void SetFullscreen()
    {
        if (!PlayerPrefs.HasKey("options.fullscreen"))
            return;
        Screen.fullScreen = PlayerPrefsX.GetBool("options.fullscreen");
    }

    public void StartGame()
    {
        StartCoroutine(Utils.LoadAsynchronously("World Options"));
        Destroy(MenuMusicManager.MenuMusic);
    }

    public void LoadRoadmap()
    {
        StartCoroutine(Utils.LoadAsynchronously("Roadmap"));
    }

    public void LoadOptions()
    {
        StartCoroutine(Utils.LoadAsynchronously("Options"));
    }

    public void LoadCredits()
    {
        StartCoroutine(Utils.LoadAsynchronously("Credits"));
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}