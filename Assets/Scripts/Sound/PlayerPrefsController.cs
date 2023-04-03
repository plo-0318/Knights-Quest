using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefsController : MonoBehaviour
{
    private const string MUSIC_VOLUME_KEY = "music volume";
    private const string SFX_VOULME_KEY = "sfx volume";

    private const float DEFAULT_VOLUME = 0.5f;

    private const float MIN_VOLUME = 0;
    private const float MAX_VOLUME = 1f;

    public static void SetMusicVolume(float volume)
    {
        if (volume >= MIN_VOLUME && volume <= MAX_VOLUME)
        {
            PlayerPrefs.SetFloat(MUSIC_VOLUME_KEY, volume);
        }
    }

    public static void SetSFXVolume(float volume)
    {
        if (volume >= MIN_VOLUME && volume <= MAX_VOLUME)
        {
            PlayerPrefs.SetFloat(SFX_VOULME_KEY, volume);
        }
    }

    public static float GetMusicVolume()
    {
        return PlayerPrefs.HasKey(MUSIC_VOLUME_KEY)
            ? PlayerPrefs.GetFloat(MUSIC_VOLUME_KEY)
            : DEFAULT_VOLUME;
    }

    public static float GetSFXVolume()
    {
        return PlayerPrefs.HasKey(SFX_VOULME_KEY)
            ? PlayerPrefs.GetFloat(SFX_VOULME_KEY)
            : DEFAULT_VOLUME;
    }
}
