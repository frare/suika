using frare;
using UnityEngine;

public class SettingsManager : Singleton<SettingsManager>
{
    protected override bool DontDestroyWhenLoad => true;

    public float MasterVolume { get; private set; } = 1f;
    public float MusicVolume { get; private set; } = 1f;
    public float SfxVolume { get; private set; } = 1f;
    public bool AnimatedBackground { get; private set; } = true;

    protected override void Awake()
    {
        base.Awake();

        LoadSettings();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }

    private void LoadSettings()
    {
        MasterVolume = PlayerPrefs.GetFloat("MasterVolume", 1f);
        MusicVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        SfxVolume = PlayerPrefs.GetFloat("SfxVolume", 1f);
        AnimatedBackground = PlayerPrefs.GetInt("AnimatedBackground", 1) == 1;
    }

    public void SetMasterVolume(float volume)
    {
        MasterVolume = volume;
        PlayerPrefs.SetFloat("MasterVolume", volume);
        PlayerPrefs.Save();
    }

    public void SetMusicVolume(float volume)
    {
        MusicVolume = volume;
        PlayerPrefs.SetFloat("MusicVolume", volume);
        PlayerPrefs.Save();
    }

    public void SetSfxVolume(float volume)
    {
        SfxVolume = volume;
        PlayerPrefs.SetFloat("SfxVolume", volume);
        PlayerPrefs.Save();
    }

    public void SetAnimatedBackground(bool isEnabled)
    {
        AnimatedBackground = isEnabled;
        PlayerPrefs.SetInt("AnimatedBackground", isEnabled ? 1 : 0);
        PlayerPrefs.Save();
    }
}