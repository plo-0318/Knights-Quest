using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VolumenUIManager : MonoBehaviour
{
    [Header("Sliders")]
    [SerializeField]
    private Slider musicVolumeSlider;

    [SerializeField]
    private Slider sfxVolumeSlider;

    [Header("Texts")]
    [SerializeField]
    private TextMeshProUGUI musicVolumeText;

    [SerializeField]
    private TextMeshProUGUI sfxVolumeText;

    private SoundManager soundManager;

    private void Awake()
    {
        InitSliders();
        SetMusicVolumeText();
        SetSFXVolumeText();
    }

    private void Start()
    {
        soundManager = GameManager.SoundManager();
    }

    private void InitSliders()
    {
        musicVolumeSlider.value = PlayerPrefsController.GetMusicVolume();
        sfxVolumeSlider.value = PlayerPrefsController.GetSFXVolume();

        musicVolumeSlider.onValueChanged.AddListener(HandleMusicVolumeChange);
        sfxVolumeSlider.onValueChanged.AddListener(HandleSFXVolumeChange);
    }

    private void SetMusicVolumeText()
    {
        musicVolumeText.text = Mathf.RoundToInt(musicVolumeSlider.value * 100).ToString();
    }

    private void SetSFXVolumeText()
    {
        sfxVolumeText.text = Mathf.RoundToInt(sfxVolumeSlider.value * 100).ToString();
    }

    private void HandleMusicVolumeChange(float value)
    {
        SetMusicVolumeText();
        PlayerPrefsController.SetMusicVolume(value);
        soundManager.SetMusicVolume(value);
    }

    private void HandleSFXVolumeChange(float value)
    {
        SetSFXVolumeText();
        PlayerPrefsController.SetSFXVolume(value);
        soundManager.SetSFXVolume(value);
    }
}
