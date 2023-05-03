using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pair<T>
{
    public T first,
        second;

    public Pair(T first, T second)
    {
        this.first = first;
        this.second = second;
    }

    public Pair(Pair<T> other)
    {
        this.first = other.first;
        this.second = other.second;
    }

    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        Pair<T> other = (Pair<T>)obj;
        return EqualityComparer<T>.Default.Equals(first, other.first)
            && EqualityComparer<T>.Default.Equals(second, other.second);
    }

    public override int GetHashCode()
    {
        int hash = 17;
        hash = hash * 23 + (first == null ? 0 : first.GetHashCode());
        hash = hash * 23 + (second == null ? 0 : second.GetHashCode());
        return hash;
    }
}

public class PlayerPrefsController : MonoBehaviour
{
    private const string MUSIC_VOLUME_KEY = "music volume";
    private const string SFX_VOULME_KEY = "sfx volume";
    private const string RESOLUTION_W_KEY = "resolution width";
    private const string RESOLUTION_H_KEY = "resolution height";
    private const string SCREEN_MODE_KEY = "screen mode";
    private const string REFRESH_RATE_KEY = "refresh rate";

    private const float DEFAULT_VOLUME = 0.5f;

    private const float MIN_VOLUME = 0;
    private const float MAX_VOLUME = 1f;

    private const string DEFAULT_SCREEN_MODE = "windowed";

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

    public static void SetResolution(int width, int height)
    {
        PlayerPrefs.SetInt(RESOLUTION_W_KEY, width);
        PlayerPrefs.SetInt(RESOLUTION_H_KEY, height);
    }

    public static Pair<int> GetResolution()
    {
        if (PlayerPrefs.HasKey(RESOLUTION_H_KEY) && PlayerPrefs.HasKey(RESOLUTION_W_KEY))
        {
            return new Pair<int>(
                PlayerPrefs.GetInt(RESOLUTION_W_KEY),
                PlayerPrefs.GetInt(RESOLUTION_H_KEY)
            );
        }

        return new Pair<int>(Screen.currentResolution.width, Screen.currentResolution.height);
    }

    public static void SetScreenMode(string screenMode)
    {
        if (screenMode != "fullscreen" && screenMode != "borderless" && screenMode != "windowed")
        {
            return;
        }

        PlayerPrefs.SetString(SCREEN_MODE_KEY, screenMode);
    }

    public static string GetSCreenMode()
    {
        return PlayerPrefs.HasKey(SCREEN_MODE_KEY)
            ? PlayerPrefs.GetString(SCREEN_MODE_KEY)
            : DEFAULT_SCREEN_MODE;
    }

    public static void SetRefreshRate(int refreshRate)
    {
        PlayerPrefs.SetInt(REFRESH_RATE_KEY, refreshRate);
    }

    public static int GetRefreshRate()
    {
        if (PlayerPrefs.HasKey(REFRESH_RATE_KEY))
        {
            return PlayerPrefs.GetInt(REFRESH_RATE_KEY);
        }

        var availableRefreshRates = GameManager.GetAvailableRefreshRates();

        return availableRefreshRates.Count > 0
            ? availableRefreshRates[availableRefreshRates.Count - 1]
            : 60;
    }
}
