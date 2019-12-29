using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Options : MonoBehaviour
{
    public static float VolumeSFX = 1.0f;
    public static float SensitivityPan = 3f;
    public static float SensitivityZoom = 10f;

    private static int _dropdownResolutionIndex = -1;

    private const float _defaultVignetteIntensity = 0.5f;
    private const float _defaultBloomIntensity = 30f;
    private const float _defaultVolumeMusic = 1f;
    private const float _defaultVolumeSFX = 1f;
    private const bool _defaultVignetteEnabled = true;
    private const bool _defaultBloomEnabled = true;
    private const bool _defaultVSyncEnabled = true;
    private const int _defaultVSyncValue = 1;
    private const float _defaultSensitivityPan = 3f;
    private const float _defaultSensitivityZoom = 10f;

    private GameObject _goPostProcessing;
    private GameObject _goMenuMusic;

    private PostProcessVolume _ppVolume;
    private Bloom _ppBloom;
    private Vignette _ppVignette;

    private AudioSource _audioMenuMusic;

    private Slider _sliderVignette;
    private Slider _sliderBloom;
    private Slider _sliderVolumeMusic;
    private Slider _sliderVolumeSFX;
    private Slider _sliderSensitivityPan;
    private Slider _sliderSensitivityZoom;

    private Toggle _toggleVignette;
    private Toggle _toggleBloom;
    private Toggle _toggleVSync;

    private Dropdown _dropdownResolutions;
    private Dropdown _dropdownQuality;

    public void Start()
    {
        _goPostProcessing = PostProcessingManager.PostProcessing;

        if (_goPostProcessing != null)
        {
            _ppVolume = _goPostProcessing.GetComponent<PostProcessVolume>();

            PostProcessProfile ppProfile = _ppVolume.profile;

            ppProfile.TryGetSettings(out _ppBloom);
            ppProfile.TryGetSettings(out _ppVignette);
        }
        else
        {
            Debug.Log("Post Processing has to be loaded from the 'Menu' scene first.");
        }

        _goMenuMusic = GameObject.Find("Menu Music");
        if (_goMenuMusic != null)
        {
            _audioMenuMusic = _goMenuMusic.GetComponent<AudioSource>();
        }
        else
        {
            Debug.Log("Menu Music has to be loaded from the 'Menu' scene first.");
        }

        // Bloom
        _sliderBloom = GameObject.Find("SliderBloom").GetComponent<Slider>();
        _toggleBloom = GameObject.Find("ToggleBloom").GetComponent<Toggle>();

        // Volume
        _sliderVolumeMusic = GameObject.Find("SliderVolumeMusic").GetComponent<Slider>();
        _sliderVolumeSFX = GameObject.Find("SliderVolumeSFX").GetComponent<Slider>();

        // Vignette
        _toggleVignette = GameObject.Find("ToggleVignette").GetComponent<Toggle>();
        _sliderVignette = GameObject.Find("SliderVignette").GetComponent<Slider>();

        // Resolutions
        _dropdownResolutions = GameObject.Find("DropdownResolutions").GetComponent<Dropdown>();
        InitializeResolutionsDropDown();

        // Quality
        _dropdownQuality = GameObject.Find("DropdownQuality").GetComponent<Dropdown>();
        InitializeQualityDropDown();

        // VSync
        _toggleVSync = GameObject.Find("ToggleVSync").GetComponent<Toggle>();

        // Camera
        _sliderSensitivityPan = GameObject.Find("SliderSensitivityPan").GetComponent<Slider>();
        _sliderSensitivityZoom = GameObject.Find("SliderSensitivityZoom").GetComponent<Slider>();

        InitializeValues();
    }

    private bool NotSetup()
    {
        return _goPostProcessing == null || _goMenuMusic == null;
    }

    private Resolution[] _resolutions;

    private void InitializeResolutionsDropDown()
    {
        _resolutions = Screen.resolutions;

        for (int i = 0; i < _resolutions.Length; i++)
        {
            _dropdownResolutions.options.Add(new Dropdown.OptionData(ResolutionToString(_resolutions[i])));
        }

        _dropdownResolutions.value = _dropdownResolutionIndex == -1 ? _resolutions.Length : _dropdownResolutionIndex;
    }

    private void InitializeQualityDropDown()
    {
        string[] names = QualitySettings.names;

        for (int i = 0; i < names.Length; i++)
        {
            _dropdownQuality.options.Add(new Dropdown.OptionData(names[i]));
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
        _toggleBloom.isOn = _ppBloom.enabled.value;
        _sliderBloom.interactable = _toggleBloom.isOn;
        _sliderBloom.value = _ppBloom.intensity.value;

        // Volume
        _sliderVolumeMusic.value = _audioMenuMusic.volume;
        _sliderVolumeSFX.value = VolumeSFX;

        // Vignette
        _toggleVignette.isOn = _ppVignette.enabled.value;
        _sliderVignette.value = _ppVignette.intensity.value;

        // VSync
        _toggleVSync.isOn = QualitySettings.vSyncCount == 0 ? false : true;

        // Camera
        _sliderSensitivityPan.value = SensitivityPan;
        _sliderSensitivityZoom.value = SensitivityZoom;

        // Resolutions
        _dropdownResolutions.value = _dropdownResolutionIndex;

        // Quality
        _dropdownQuality.value = QualitySettings.GetQualityLevel();
    }

    public void ChangeQuality()
    {
        QualitySettings.SetQualityLevel(_dropdownQuality.value);
    }

    public void ChangeResolution()
    {
        _dropdownResolutionIndex = _dropdownResolutions.value;
        Screen.SetResolution(_resolutions[_dropdownResolutions.value].width, _resolutions[_dropdownResolutions.value].height, Screen.fullScreen);
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

        _ppBloom.enabled.value = !_ppBloom.enabled.value;
        _sliderBloom.interactable = !_sliderBloom.interactable;
    }

    public void ToggleVignette()
    {
        if (NotSetup())
            return;

        _ppVignette.enabled.value = !_ppVignette.enabled.value;
        _sliderVignette.interactable = !_sliderVignette.interactable;
    }

    public void SliderVignetteUpdate()
    {
        if (NotSetup())
            return;

        _ppVignette.intensity.value = _sliderVignette.value;
    }

    public void SliderBloomUpdate()
    {
        if (NotSetup())
            return;

        _ppBloom.intensity.value = _sliderBloom.value;
    }

    public void SliderVolumeMusicUpdate()
    {
        if (NotSetup())
            return;

        _audioMenuMusic.volume = _sliderVolumeMusic.value;
    }

    public void SliderVolumeSFXUpdate()
    {
        if (NotSetup())
            return;

        VolumeSFX = _sliderVolumeSFX.value;
    }

    public void SliderSensitivityPanUpdate()
    {
        if (NotSetup())
            return;

        SensitivityPan = _sliderSensitivityPan.value;
    }

    public void SliderSensitivityZoomUpdate()
    {
        if (NotSetup())
            return;

        SensitivityZoom = _sliderSensitivityZoom.value;
    }

    public void ResetToDefaults()
    {
        if (NotSetup())
            return;

        // Bloom
        _ppBloom.enabled.value = _defaultBloomEnabled;
        _ppBloom.intensity.value = _defaultBloomIntensity;
        _toggleBloom.isOn = _defaultBloomEnabled;
        _sliderBloom.value = _defaultBloomIntensity;

        // Vignette
        _ppVignette.enabled.value = _defaultVignetteEnabled;
        _ppVignette.intensity.value = _defaultVignetteIntensity;
        _toggleVignette.isOn = _defaultVignetteEnabled;
        _sliderVignette.value = _defaultVignetteIntensity;

        // Volume (Music)
        _sliderVolumeMusic.value = _defaultVolumeMusic;

        // Volume (SFX)
        _sliderVolumeSFX.value = _defaultVolumeSFX;

        // VSync
        _toggleVSync.isOn = _defaultVSyncEnabled;
        QualitySettings.vSyncCount = _defaultVSyncValue;

        // Camera Pan
        _sliderSensitivityPan.value = _defaultSensitivityPan;

        // Camera Zoom
        _sliderSensitivityZoom.value = _defaultSensitivityZoom;
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
