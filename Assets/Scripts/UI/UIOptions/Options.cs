using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Options : MonoBehaviour
{
    #region Inspector:Default Values

    // Inspector Options
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

    #endregion Inspector:Default Values

    [HideInInspector] public static float VolumeSFX = 1.0f;
    [HideInInspector] public static float VolumeMusic = 1.0f;
    [HideInInspector] public static float SensitivityPan = 3f;
    [HideInInspector] public static float SensitivityZoom = 10f;

    private GameObject goPostProcessing;
    private GameObject goMenuMusic;

    private PostProcessVolume ppVolume;
    private Bloom ppBloom;
    private Vignette ppVignette;

    private AudioSource audioMenuMusic;

    private Resolution[] resolutions;
    private static int dropdownResolutionIndex = -1;

    private OptionSlider optionSliderBloom;
    private OptionToggle optionToggleBloom;
    private OptionSlider optionSliderVolumeMusic;
    private OptionSlider optionSliderVolumeSFX;
    private OptionToggle optionToggleVignette;
    private OptionSlider optionSliderVignette;
    private OptionDropdown optionDropdownResolution;
    private OptionDropdown optionDropdownQuality;
    private OptionToggle optionToggleVSync;
    private OptionToggle optionToggleFullscreen;
    private OptionSlider optionSliderSensitivityPan;
    private OptionSlider optionSliderSensitivityZoom;

    private Button buttonResetToDefaults;
    private Button buttonBackToMenu;

    public void Awake()
    {
        SceneManager.sceneUnloaded += ClearStaticOptions;
    }

    public void Start()
    {
        #region Setup

        goPostProcessing = DontDestroy.DontDestroyObjects[0];

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

        optionSliderBloom = new OptionSlider("SliderBloom"); // Bloom
        optionToggleBloom = new OptionToggle("ToggleBloom");

        optionSliderVolumeMusic = new OptionSlider("SliderVolumeMusic"); // Volume
        optionSliderVolumeSFX = new OptionSlider("SliderVolumeSFX");

        optionToggleVignette = new OptionToggle("ToggleVignette"); // Vignette
        optionSliderVignette = new OptionSlider("SliderVignette");

        optionDropdownResolution = new OptionDropdown("DropdownResolutions"); // Resolutions
        InitializeResolutionsDropDown();

        optionDropdownQuality = new OptionDropdown("DropdownQuality"); // Quality
        InitializeQualityDropDown();

        optionToggleFullscreen = new OptionToggle("ToggleFullscreen"); // Fullscreen
        optionToggleVSync = new OptionToggle("ToggleVSync"); // VSync

        optionSliderSensitivityPan = new OptionSlider("SliderSensitivityPan"); // Camera
        optionSliderSensitivityZoom = new OptionSlider("SliderSensitivityZoom");

        if (goPostProcessing == null || goMenuMusic == null)
            return;

        buttonResetToDefaults = GameObject.Find("ButtonReset").GetComponent<Button>();
        buttonBackToMenu = GameObject.Find("ButtonMenu").GetComponent<Button>();

        buttonResetToDefaults.onClick.AddListener(delegate
        {
            ResetToDefaults();
        });

        buttonBackToMenu.onClick.AddListener(delegate
        {
            SceneManager.LoadScene("Menu");
        });

        #endregion Setup

        InitializePlayerPrefs();
        InitializeUIValues();
        InitializeOptionValues();
    }

    private void InitializePlayerPrefs()
    {
        optionToggleBloom.Save("options.bloom.enabled", DefaultBloomEnabled);
        optionSliderBloom.Save("options.bloom.intensity", DefaultBloomIntensity);
        optionSliderVolumeMusic.Save("options.volume.music", DefaultVolumeMusic);
        optionSliderVolumeSFX.Save("options.volume.sfx", DefaultVolumeSFX);
        optionToggleVignette.Save("options.vignette.enabled", DefaultVignetteEnabled);
        optionSliderVignette.Save("options.vignette.intensity", DefaultVignetteIntensity);
        optionDropdownResolution.Save("options.resolution", resolutions.Length);
        optionDropdownQuality.Save("options.quality", QualitySettings.GetQualityLevel());
        optionToggleFullscreen.Save("options.fullscreen", Screen.fullScreen);
        optionToggleVSync.Save("options.vsync", QualitySettings.vSyncCount == 0 ? true : false);
        optionSliderSensitivityPan.Save("options.sensitivity.pan", DefaultSensitivityPan);
        optionSliderSensitivityZoom.Save("options.sensitivity.zoom", DefaultSensitivityZoom);
    }

    private void InitializeUIValues()
    {
        ppBloom.enabled.value = PlayerPrefsX.GetBool("options.bloom.enabled");
        ppBloom.intensity.value = PlayerPrefs.GetFloat("options.bloom.intensity");
        ppVignette.enabled.value = PlayerPrefsX.GetBool("options.vignette.enabled");
        ppVignette.intensity.value = PlayerPrefs.GetFloat("options.vignette.intensity");
        optionToggleFullscreen.Instance.isOn = PlayerPrefsX.GetBool("options.fullscreen");
        optionToggleVSync.Instance.isOn = PlayerPrefsX.GetBool("options.vsync");
    }

    private void InitializeOptionValues()
    {
        // Bloom
        optionToggleBloom.Instance.isOn = PlayerPrefsX.GetBool("options.bloom.enabled");
        optionToggleBloom.Instance.onValueChanged.AddListener(delegate
        {
            ppBloom.enabled.value = !ppBloom.enabled.value;
            optionSliderBloom.Instance.interactable = !optionSliderBloom.Instance.interactable;
            PlayerPrefsX.SetBool("options.bloom.enabled", ppBloom.enabled.value);
        });

        optionSliderBloom.Instance.value = PlayerPrefs.GetFloat("options.bloom.intensity");
        optionSliderBloom.Instance.interactable = optionToggleBloom.Instance.isOn;
        optionSliderBloom.Instance.onValueChanged.AddListener(delegate
        {
            ppBloom.intensity.value = optionSliderBloom.Instance.value;
            PlayerPrefs.SetFloat("options.bloom.intensity", ppBloom.intensity.value);
        });

        // Volume
        optionSliderVolumeMusic.Instance.value = PlayerPrefs.GetFloat("options.volume.music");
        optionSliderVolumeMusic.Instance.onValueChanged.AddListener(delegate
        {
            VolumeMusic = optionSliderVolumeMusic.Instance.value;
            audioMenuMusic.volume = VolumeMusic;
            PlayerPrefs.SetFloat("options.volume.music", VolumeMusic);
        });

        optionSliderVolumeSFX.Instance.value = PlayerPrefs.GetFloat("options.volume.sfx");
        optionSliderVolumeSFX.Instance.onValueChanged.AddListener(delegate
        {
            VolumeSFX = optionSliderVolumeSFX.Instance.value;
            PlayerPrefs.SetFloat("options.volume.sfx", VolumeSFX);
        });

        // Vignette
        optionToggleVignette.Instance.isOn = PlayerPrefsX.GetBool("options.vignette.enabled");
        optionToggleVignette.Instance.onValueChanged.AddListener(delegate
        {
            ppVignette.enabled.value = !ppVignette.enabled.value;
            optionSliderVignette.Instance.interactable = !optionSliderVignette.Instance.interactable;
            PlayerPrefsX.SetBool("options.vignette.enabled", ppVignette.enabled.value);
        });

        optionSliderVignette.Instance.value = PlayerPrefs.GetFloat("options.vignette.intensity");
        optionSliderVignette.Instance.interactable = optionToggleVignette.Instance.isOn;
        optionSliderVignette.Instance.onValueChanged.AddListener(delegate
        {
            ppVignette.intensity.value = optionSliderVignette.Instance.value;
            PlayerPrefs.SetFloat("options.vignette.intensity", ppVignette.intensity.value);
        });

        // VSync
        optionToggleVSync.Instance.isOn = PlayerPrefsX.GetBool("options.vsync");
        optionToggleVSync.Instance.onValueChanged.AddListener(delegate
        {
            QualitySettings.vSyncCount = QualitySettings.vSyncCount == 0 ? 1 : 0;
            PlayerPrefsX.SetBool("options.vsync", QualitySettings.vSyncCount == 1 ? true : false);
        });

        // Fullscreen
        optionToggleFullscreen.Instance.isOn = PlayerPrefsX.GetBool("options.fullscreen");
        optionToggleFullscreen.Instance.onValueChanged.AddListener(delegate
        {
            Screen.fullScreen = !Screen.fullScreen;
            PlayerPrefsX.SetBool("options.fullscreen", Screen.fullScreen);
        });

        // Camera
        optionSliderSensitivityPan.Instance.value = PlayerPrefs.GetFloat("options.sensitivity.pan");
        optionSliderSensitivityPan.Instance.onValueChanged.AddListener(delegate
        {
            SensitivityPan = optionSliderSensitivityPan.Instance.value;
            PlayerPrefs.SetFloat("options.sensitivity.pan", SensitivityPan);
        });

        optionSliderSensitivityZoom.Instance.value = PlayerPrefs.GetFloat("options.sensitivity.zoom");
        optionSliderSensitivityZoom.Instance.onValueChanged.AddListener(delegate
        {
            SensitivityZoom = optionSliderSensitivityZoom.Instance.value;
            PlayerPrefs.SetFloat("options.sensitivity.zoom", SensitivityZoom);
        });

        // Resolutions
        optionDropdownResolution.Instance.value = PlayerPrefs.GetInt("options.resolution");
        optionDropdownResolution.Instance.onValueChanged.AddListener(delegate
        {
            dropdownResolutionIndex = optionDropdownResolution.Instance.value;
            Screen.SetResolution(resolutions[optionDropdownResolution.Instance.value].width, resolutions[optionDropdownResolution.Instance.value].height, Screen.fullScreen);
            PlayerPrefs.SetInt("options.resolution", dropdownResolutionIndex);
        });

        // Quality
        optionDropdownQuality.Instance.value = PlayerPrefs.GetInt("options.quality");
        optionDropdownQuality.Instance.onValueChanged.AddListener(delegate
        {
            QualitySettings.SetQualityLevel(optionDropdownQuality.Instance.value);
            PlayerPrefs.SetInt("options.quality", optionDropdownQuality.Instance.value);
        });

        foreach (Option option in Option.Options)
        {
            option.SetActive(true);
        }
    }

    private void ResetToDefaults()
    {
        ppBloom.enabled.value = DefaultBloomEnabled; // Bloom
        ppBloom.intensity.value = DefaultBloomIntensity;
        optionToggleBloom.Instance.isOn = DefaultBloomEnabled;
        optionSliderBloom.Instance.value = DefaultBloomIntensity;

        ppVignette.enabled.value = DefaultVignetteEnabled; // Vignette
        ppVignette.intensity.value = DefaultVignetteIntensity;
        optionToggleVignette.Instance.isOn = DefaultVignetteEnabled;
        optionSliderVignette.Instance.value = DefaultVignetteIntensity;

        optionSliderVolumeMusic.Instance.value = DefaultVolumeMusic; // Volume (Music)
        optionSliderVolumeSFX.Instance.value = DefaultVolumeSFX; // Volume (SFX)

        optionToggleVSync.Instance.isOn = DefaultVSyncEnabled; // VSync
        QualitySettings.vSyncCount = DefaultVSyncValue;

        optionSliderSensitivityPan.Instance.value = DefaultSensitivityPan; // Camera Pan
        optionSliderSensitivityZoom.Instance.value = DefaultSensitivityZoom; // Camera Zoom
    }

    private void InitializeResolutionsDropDown()
    {
        resolutions = Screen.resolutions;

        for (int i = 0; i < resolutions.Length; i++)
        {
            Resolution res = resolutions[i];
            int refreshRate = res.refreshRate;

            optionDropdownResolution.Instance.options.Add(new Dropdown.OptionData(ResolutionToString(res)));
        }

        optionDropdownResolution.Instance.value = dropdownResolutionIndex == -1 ? resolutions.Length : dropdownResolutionIndex;
    }

    private void InitializeQualityDropDown()
    {
        string[] names = QualitySettings.names;

        for (int i = 0; i < names.Length; i++)
        {
            optionDropdownQuality.Instance.options.Add(new Dropdown.OptionData(names[i]));
        }
    }

    private string ResolutionToString(Resolution res)
    {
        return res.width + " x " + res.height + " (" + res.refreshRate + " Hz)";
    }

    private void ClearStaticOptions<Scene>(Scene scene)
    {
        Option.Options.Clear();
    }
}