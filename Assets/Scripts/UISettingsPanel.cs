using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UISettingsPanel : MonoBehaviour
{
    [SerializeField] private GameObject panelRoot;
    [SerializeField] private Slider masterVolumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Toggle backgroundEffectToggle;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button closeButton;

    private BackgroundEffectController backgroundEffectController;

    private void Awake()
    {
        backgroundEffectController = FindAnyObjectByType<BackgroundEffectController>();

        masterVolumeSlider.onValueChanged.AddListener(OnMasterVolumeChanged);
        sfxVolumeSlider.onValueChanged.AddListener(OnSFXVolumeChanged);
        musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
        backgroundEffectToggle.onValueChanged.AddListener(OnBackgroundEffectToggled);
        mainMenuButton.onClick.AddListener(OnMainMenuClicked);
        closeButton.onClick.AddListener(OnCloseButtonClicked);

        Close();
    }

    private void Start()
    {
        LoadSettings();
    }

    private void OnDestroy()
    {
        masterVolumeSlider.onValueChanged.RemoveListener(OnMasterVolumeChanged);
        sfxVolumeSlider.onValueChanged.RemoveListener(OnSFXVolumeChanged);
        musicVolumeSlider.onValueChanged.RemoveListener(OnMusicVolumeChanged);
        backgroundEffectToggle.onValueChanged.RemoveListener(OnBackgroundEffectToggled);
        mainMenuButton.onClick.RemoveListener(OnMainMenuClicked);
        closeButton.onClick.RemoveListener(OnCloseButtonClicked);
    }

    public void Open()
    {
        panelRoot.SetActive(true);

        if (GameManager.Instance != null) GameManager.Instance.SetPaused(true);
    }

    public void Close()
    {
        panelRoot.SetActive(false);

        if (GameManager.Instance != null) GameManager.Instance.SetPaused(false);
    }

    private void LoadSettings()
    {
        masterVolumeSlider.value = SettingsManager.Instance.MasterVolume;
        sfxVolumeSlider.value = SettingsManager.Instance.SfxVolume;
        musicVolumeSlider.value = SettingsManager.Instance.MusicVolume;
        backgroundEffectToggle.isOn = SettingsManager.Instance.AnimatedBackground;
    }

    private void OnMasterVolumeChanged(float value)
    {
        AudioManager.Instance.SetMasterVolume(value);
        SettingsManager.Instance.SetMasterVolume(value);
    }

    private void OnSFXVolumeChanged(float value)
    {
        AudioManager.Instance.SetSfxVolume(value);
        SettingsManager.Instance.SetSfxVolume(value);
    }

    private void OnMusicVolumeChanged(float value)
    {
        AudioManager.Instance.SetMusicVolume(value);
        SettingsManager.Instance.SetMusicVolume(value);
    }

    private void OnBackgroundEffectToggled(bool isEnabled)
    {
        if (backgroundEffectController != null)
        {
            backgroundEffectController.SetEnabled(isEnabled);
            SettingsManager.Instance.SetAnimatedBackground(isEnabled);
        }
    }

    private void OnMainMenuClicked()
    {
        SceneManager.LoadScene("MenuScene");
    }

    private void OnCloseButtonClicked()
    {
        Close();
    }
}