using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Options : MonoBehaviour
{
    [Header("Vignette")]
    [SerializeField] public float DefaultVignetteIntensity = 0.25f;
    [SerializeField] public bool DefaultVignetteEnabled = true;

    [Header("Bloom")]
    [SerializeField] public float DefaultBloomIntensity = 30f;
    [SerializeField] public bool DefaultBloomEnabled = true;

    [Header("Volume")]
    [SerializeField] public float DefaultVolumeMusic = 1f;
    [SerializeField] public float DefaultVolumeSFX = 1f;

    [Header("VSync")]
    [SerializeField] public int DefaultVSyncValue = 1;
    [SerializeField] public bool DefaultVSyncEnabled = true;

    [Header("Camera")]
    [SerializeField] public float DefaultSensitivityPan = 3f;
    [SerializeField] public float DefaultSensitivityZoom = 10f;

    // Static
    [HideInInspector] public static float VolumeSFX = 1.0f;
    [HideInInspector] public static float SensitivityPan = 3f;
    [HideInInspector] public static float SensitivityZoom = 10f;

    // Private
    private static int dropdownResolutionIndex = -1;

    private GameObject goPostProcessing;
    private GameObject goMenuMusic;

    private PostProcessVolume ppVolume;
    private Bloom ppBloom;
    private Vignette ppVignette;

    private AudioSource audioMenuMusic;

    private Slider sliderVignette;
    private Slider sliderBloom;
    private Slider sliderVolumeMusic;
    private Slider sliderVolumeSFX;
    private Slider sliderSensitivityPan;
    private Slider sliderSensitivityZoom;

    private Toggle toggleVignette;
    private Toggle toggleBloom;
    private Toggle toggleVSync;

    private Dropdown dropdownResolutions;
    private Dropdown dropdownQuality;

    public void Start()
    {
        goPostProcessing = DontDestroy.Go;

        if (goPostProcessing != null)
        {
            ppVolume = goPostProcessing.GetComponent<PostProcessVolume>();

            PostProcessProfile ppProfile = ppVolume.profile;

            ppProfile.TryGetSettings(out ppBloom);
            ppProfile.TryGetSettings(out ppVignette);
        }
        else
        {
            Debug.Log("Post Processing has to be loaded from the 'Menu' scene first.");
        }

        goMenuMusic = GameObject.Find("Menu Music");
        if (goMenuMusic != null)
        {
            audioMenuMusic = goMenuMusic.GetComponent<AudioSource>();
        }
        else
        {
            Debug.Log("Menu Music has to be loaded from the 'Menu' scene first.");
        }

        // Bloom
        sliderBloom = GameObject.Find("SliderBloom").GetComponent<Slider>();
        toggleBloom = GameObject.Find("ToggleBloom").GetComponent<Toggle>();

        // Volume
        sliderVolumeMusic = GameObject.Find("SliderVolumeMusic").GetComponent<Slider>();
        sliderVolumeSFX = GameObject.Find("SliderVolumeSFX").GetComponent<Slider>();

        // Vignette
        toggleVignette = GameObject.Find("ToggleVignette").GetComponent<Toggle>();
        sliderVignette = GameObject.Find("SliderVignette").GetComponent<Slider>();

        // Resolutions
        dropdownResolutions = GameObject.Find("DropdownResolutions").GetComponent<Dropdown>();
        InitializeResolutionsDropDown();

        // Quality
        dropdownQuality = GameObject.Find("DropdownQuality").GetComponent<Dropdown>();
        InitializeQualityDropDown();

        // VSync
        toggleVSync = GameObject.Find("ToggleVSync").GetComponent<Toggle>();

        // Camera
        sliderSensitivityPan = GameObject.Find("SliderSensitivityPan").GetComponent<Slider>();
        sliderSensitivityZoom = GameObject.Find("SliderSensitivityZoom").GetComponent<Slider>();

        InitializeValues();
    }

    private bool NotSetup()
    {
        return goPostProcessing == null || goMenuMusic == null;
    }

    private Resolution[] resolutions;

    private void InitializeResolutionsDropDown()
    {
        resolutions = Screen.resolutions;

        for (int i = 0; i < resolutions.Length; i++)
        {
            dropdownResolutions.options.Add(new Dropdown.OptionData(ResolutionToString(resolutions[i])));
        }

        dropdownResolutions.value = dropdownResolutionIndex == -1 ? resolutions.Length : dropdownResolutionIndex;
    }

    private void InitializeQualityDropDown()
    {
        string[] names = QualitySettings.names;

        for (int i = 0; i < names.Length; i++)
        {
            dropdownQuality.options.Add(new Dropdown.OptionData(names[i]));
        }
    }

    private string ResolutionToString(Resolution res)
    {
        return res.width + " x " + res.height;
    }

    private void InitializeValues()
    {
        if (NotSetup())
            return;

        // Bloom
        toggleBloom.isOn = ppBloom.enabled.value;
        sliderBloom.interactable = toggleBloom.isOn;
        sliderBloom.value = ppBloom.intensity.value;

        // Volume
        sliderVolumeMusic.value = audioMenuMusic.volume;
        sliderVolumeSFX.value = VolumeSFX;

        // Vignette
        toggleVignette.isOn = ppVignette.enabled.value;
        sliderVignette.value = ppVignette.intensity.value;

        // VSync
        toggleVSync.isOn = QualitySettings.vSyncCount == 0 ? false : true;

        // Camera
        sliderSensitivityPan.value = SensitivityPan;
        sliderSensitivityZoom.value = SensitivityZoom;

        // Resolutions
        dropdownResolutions.value = dropdownResolutionIndex;

        // Quality
        dropdownQuality.value = QualitySettings.GetQualityLevel();
    }

    public void ChangeQuality()
    {
        QualitySettings.SetQualityLevel(dropdownQuality.value);
    }

    public void ChangeResolution()
    {
        dropdownResolutionIndex = dropdownResolutions.value;
        Screen.SetResolution(resolutions[dropdownResolutions.value].width, resolutions[dropdownResolutions.value].height, Screen.fullScreen);
    }

    public void ToggleFullScreen()
    {
        Screen.fullScreen = !Screen.fullScreen;
    }

    public void ToggleVSync()
    {
        QualitySettings.vSyncCount = QualitySettings.vSyncCount == 0 ? 1 : 0;
    }

    public void ToggleBloom()
    {
        if (NotSetup())
            return;

        ppBloom.enabled.value = !ppBloom.enabled.value;
        sliderBloom.interactable = !sliderBloom.interactable;
    }

    public void ToggleVignette()
    {
        if (NotSetup())
            return;

        ppVignette.enabled.value = !ppVignette.enabled.value;
        sliderVignette.interactable = !sliderVignette.interactable;
    }

    public void SliderVignetteUpdate()
    {
        if (NotSetup())
            return;

        ppVignette.intensity.value = sliderVignette.value;
    }

    public void SliderBloomUpdate()
    {
        if (NotSetup())
            return;

        ppBloom.intensity.value = sliderBloom.value;
    }

    public void SliderVolumeMusicUpdate()
    {
        if (NotSetup())
            return;

        audioMenuMusic.volume = sliderVolumeMusic.value;
    }

    public void SliderVolumeSFXUpdate()
    {
        if (NotSetup())
            return;

        VolumeSFX = sliderVolumeSFX.value;
    }

    public void SliderSensitivityPanUpdate()
    {
        if (NotSetup())
            return;

        SensitivityPan = sliderSensitivityPan.value;
    }

    public void SliderSensitivityZoomUpdate()
    {
        if (NotSetup())
            return;

        SensitivityZoom = sliderSensitivityZoom.value;
    }

    public void ResetToDefaults()
    {
        if (NotSetup())
            return;

        // Bloom
        ppBloom.enabled.value = DefaultBloomEnabled;
        ppBloom.intensity.value = DefaultBloomIntensity;
        toggleBloom.isOn = DefaultBloomEnabled;
        sliderBloom.value = DefaultBloomIntensity;

        // Vignette
        ppVignette.enabled.value = DefaultVignetteEnabled;
        ppVignette.intensity.value = DefaultVignetteIntensity;
        toggleVignette.isOn = DefaultVignetteEnabled;
        sliderVignette.value = DefaultVignetteIntensity;

        // Volume (Music)
        sliderVolumeMusic.value = DefaultVolumeMusic;

        // Volume (SFX)
        sliderVolumeSFX.value = DefaultVolumeSFX;

        // VSync
        toggleVSync.isOn = DefaultVSyncEnabled;
        QualitySettings.vSyncCount = DefaultVSyncValue;

        // Camera Pan
        sliderSensitivityPan.value = DefaultSensitivityPan;

        // Camera Zoom
        sliderSensitivityZoom.value = DefaultSensitivityZoom;
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("Menu");
        }
    }
}
