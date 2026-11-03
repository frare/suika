using UnityEngine;
using UnityEngine.Audio;
using System.Collections;
using System.Collections.Generic;
using frare;
using frare.CollectionsExtensions;

public class AudioManager : Singleton<AudioManager>
{
    protected override bool DontDestroyWhenLoad => true;

    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private AudioClip[] musicClips;
    [SerializeField] private AudioClip mergeClip;

    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    private float masterVolume = 1f;
    private float musicVolume = 1f;
    private float sfxVolume = 1f;
    private float mergeSfxPitch = 1f;
    private AudioClip currentMusicClip;
    private List<AudioClip> nextMusicClips;

    override protected void Awake()
    {
        base.Awake();

        SetNextMusicClips();
        NextMusic();
    }

    override protected void OnDestroy()
    {
        base.OnDestroy();
    }

    private void Update()
    {
        if (mergeSfxPitch > 1f) mergeSfxPitch = Mathf.Clamp(mergeSfxPitch - Time.deltaTime / 4f, 1f, 3f);
    }

    public void SetMasterVolume(float volume)
    {
        masterVolume = volume;
        audioMixer.SetFloat("MasterVolume", ConvertVolumeToDecibels(volume));
    }

    public void SetMusicVolume(float volume)
    {
        musicVolume = volume;
        audioMixer.SetFloat("MusicVolume", ConvertVolumeToDecibels(volume));
    }

    public void SetSfxVolume(float volume)
    {
        sfxVolume = volume;
        audioMixer.SetFloat("SfxVolume", ConvertVolumeToDecibels(volume));
    }

    public void PlaySfx(AudioClip clip, bool randomPitch = true)
    {
        if (randomPitch) sfxSource.pitch = Random.Range(0.8f, 1.2f);
        else sfxSource.pitch = 1f;

        sfxSource.PlayOneShot(clip);
    }

    public void PlayMergeSfx()
    {
        sfxSource.pitch = mergeSfxPitch + Random.Range(-0.2f, 0.2f);
        sfxSource.PlayOneShot(mergeClip);

        mergeSfxPitch = Mathf.Clamp(mergeSfxPitch, mergeSfxPitch + 0.5f, 3f);
    }

    [ContextMenu("Next Music")]
    public void NextMusic()
    {
        if (nextMusicClips.Count == 0) 
        {
            SetNextMusicClips();
            nextMusicClips.Remove(currentMusicClip);
        }

        currentMusicClip = nextMusicClips[0];
        musicSource.clip = currentMusicClip;
        musicSource.Play();
        StartCoroutine(WaitForMusicToEnd());
        
        nextMusicClips.RemoveAt(0);
    }

    private void SetNextMusicClips()
    {
        nextMusicClips = new List<AudioClip>(musicClips);
        nextMusicClips.Shuffle();
    }

    private IEnumerator WaitForMusicToEnd()
    {
        yield return new WaitUntil(() => !musicSource.isPlaying);
        NextMusic();
    }

    // when receiving a normalized volume value (0 to 1), convert it to decibels for the audio mixer
    private float ConvertVolumeToDecibels(float volume)
    {
        return volume > 0.0001f ? Mathf.Log10(volume) * 20f : -80f;
    }
}