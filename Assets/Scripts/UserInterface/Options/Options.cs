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
    #endregion

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

    public void Start()
    {
        #region Init:Post Processing
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
        #endregion

        #region Init:Menu Music
        goMenuMusic = GameObject.Find("Menu Music");
        if (goMenuMusic != null)
        {
            audioMenuMusic = goMenuMusic.GetComponent<AudioSource>();
        }
        else
        {
            Debug.Log("Menu Music has to be loaded from the 'Menu' scene first.");
        }
        #endregion
        
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

        buttonResetToDefaults.onClick.AddListener(delegate {
            ResetToDefaults();
        });

        buttonBackToMenu.onClick.AddListener(delegate {
            SceneManager.LoadScene("Menu");
        });

        InitializeOptionValues();
    }

    private void InitializeOptionValues()
    {
        // Bloom
        optionToggleBloom.Instance.isOn = ppBloom.enabled.value; 
        optionToggleBloom.Instance.onValueChanged.AddListener(delegate {
            ppBloom.enabled.value = !ppBloom.enabled.value;
            optionSliderBloom.Instance.interactable = !optionSliderBloom.Instance.interactable;
        });
        
        optionSliderBloom.Instance.value = ppBloom.intensity.value;
        optionSliderBloom.Instance.interactable = optionToggleBloom.Instance.isOn;
        optionSliderBloom.Instance.onValueChanged.AddListener(delegate {
            ppBloom.intensity.value = optionSliderBloom.Instance.value;
        });

        // Volume
        optionSliderVolumeMusic.Instance.value = audioMenuMusic.volume;
        optionSliderVolumeMusic.Instance.onValueChanged.AddListener(delegate {
            VolumeMusic = optionSliderVolumeMusic.Instance.value;
            audioMenuMusic.volume = VolumeMusic;
        });

        optionSliderVolumeSFX.Instance.value = VolumeSFX;
        optionSliderVolumeSFX.Instance.onValueChanged.AddListener(delegate {
            VolumeSFX = optionSliderVolumeSFX.Instance.value;
        });

        // Vignette
        optionToggleVignette.Instance.isOn = ppVignette.enabled.value;
        optionToggleVignette.Instance.onValueChanged.AddListener(delegate {
            ppVignette.enabled.value = !ppVignette.enabled.value;
            optionSliderVignette.Instance.interactable = !optionSliderVignette.Instance.interactable;
        });

        optionSliderVignette.Instance.value = ppVignette.intensity.value;
        optionSliderVignette.Instance.onValueChanged.AddListener(delegate {
            ppVignette.intensity.value = optionSliderVignette.Instance.value;
        });

        // VSync
        optionToggleVSync.Instance.isOn = QualitySettings.vSyncCount == 0 ? false : true;
        optionToggleVSync.Instance.onValueChanged.AddListener(delegate {
            QualitySettings.vSyncCount = QualitySettings.vSyncCount == 0 ? 1 : 0;
        });

        // Fullscreen
        optionToggleFullscreen.Instance.isOn = Screen.fullScreen;
        optionToggleFullscreen.Instance.onValueChanged.AddListener(delegate {
            Screen.fullScreen = !Screen.fullScreen;
        });

        // Camera
        optionSliderSensitivityPan.Instance.value = SensitivityPan;
        optionSliderSensitivityPan.Instance.onValueChanged.AddListener(delegate {
            SensitivityPan = optionSliderSensitivityPan.Instance.value;
        });

        optionSliderSensitivityZoom.Instance.value = SensitivityZoom;
        optionSliderSensitivityZoom.Instance.onValueChanged.AddListener(delegate {
            SensitivityZoom = optionSliderSensitivityZoom.Instance.value;
        });

        // Resolutions
        optionDropdownResolution.Instance.value = dropdownResolutionIndex;
        optionDropdownResolution.Instance.onValueChanged.AddListener(delegate {
            dropdownResolutionIndex = optionDropdownResolution.Instance.value;
            Screen.SetResolution(resolutions[optionDropdownResolution.Instance.value].width, resolutions[optionDropdownResolution.Instance.value].height, Screen.fullScreen);
        });

        // Quality
        optionDropdownQuality.Instance.value = QualitySettings.GetQualityLevel();
        optionDropdownQuality.Instance.onValueChanged.AddListener(delegate {
            QualitySettings.SetQualityLevel(optionDropdownQuality.Instance.value);
        });
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
}
