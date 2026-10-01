using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Mixer")]
    [SerializeField] private AudioMixer audioMixer;

    [Header("UI Toggles")]
    [SerializeField] private Toggle musicToggle;
    [SerializeField] private Toggle sfxToggle;

    
    private const string MUSIC_PARAM = "MusicVol";
    private const string SFX_PARAM = "SFXVol";

    
    private const float NORMAL_VOL = 0f;
    private const float MUTE_VOL = -80f;

    private void Start()
    {
        
        bool musicOn = PlayerPrefs.GetInt("MusicEnabled", 1) == 1;
        bool sfxOn = PlayerPrefs.GetInt("SFXEnabled", 1) == 1;

        if (musicToggle != null) musicToggle.isOn = musicOn;
        if (sfxToggle != null) sfxToggle.isOn = sfxOn;

        SetMusicState(musicOn);
        SetSFXState(sfxOn);
    }

    public void ToggleMusic(bool enabled)
    {
        SetMusicState(enabled);
        PlayerPrefs.SetInt("MusicEnabled", enabled ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void ToggleSFX(bool enabled)
    {
        SetSFXState(enabled);
        PlayerPrefs.SetInt("SFXEnabled", enabled ? 1 : 0);
        PlayerPrefs.Save();
    }

    private void SetMusicState(bool enabled)
    {
        float volume = enabled ? NORMAL_VOL : MUTE_VOL;
        audioMixer.SetFloat(MUSIC_PARAM, volume);
    }

    private void SetSFXState(bool enabled)
    {
        float volume = enabled ? NORMAL_VOL : MUTE_VOL;
        audioMixer.SetFloat(SFX_PARAM, volume);
    }
}