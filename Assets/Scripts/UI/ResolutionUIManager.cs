using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class ResolutionUIManager : MonoBehaviour
{
    [Header("Screen Mode")]
    [SerializeField]
    private GameObject screenLeftButton;

    [SerializeField]
    private GameObject screenRightButton;

    [SerializeField]
    private TextMeshProUGUI screenModeText;

    [Header("Resolution")]
    [SerializeField]
    private GameObject resLeftButton;

    [SerializeField]
    private GameObject resRightButton;

    [SerializeField]
    private TextMeshProUGUI resolutionText;

    [Header("Refresh Rate")]
    [SerializeField]
    private GameObject refreshLeftButton;

    [SerializeField]
    private GameObject refreshRightButton;

    [SerializeField]
    private TextMeshProUGUI refreshRateText;

    [Header("Apply")]
    [SerializeField]
    private GameObject applyButton;

    private SoundManager soundManager;

    private List<string> availableScreenModes;
    private string currentScreenMode;
    private string selectedScreenMode;

    private List<Pair<int>> availableResolutions;
    private Pair<int> currentResolution;
    private Pair<int> selectedResolution;

    private List<int> availableRefreshRates;
    private int currentRefreshRate;
    private int selectedRefreshRate;

    private void Start()
    {
        soundManager = GameManager.SoundManager();

        InitButton(screenLeftButton, HandleScreenModePreviousButtonClick);
        InitButton(screenRightButton, HandleScreenModeNextButtonClick);

        InitButton(resLeftButton, HandleResPreviousButtonClick);
        InitButton(resRightButton, HandleResNextButtonClick);

        InitButton(refreshLeftButton, HandleRefreshPreviousButtonClick);
        InitButton(refreshRightButton, HandleRefreshNextButtonClick);

        InitButton(applyButton, HandleApplyButtonClick);
    }

    public void Init()
    {
        InitScreenModes();
        InitResolutions();
        InitRefreshRates();
    }

    //////////////////////// RESOLUTION HANDLERS ////////////////////////
    private void InitScreenModes()
    {
        availableScreenModes = GameManager.GetAvailableScreenModes();

        currentScreenMode = PlayerPrefsController.GetSCreenMode();
        selectedScreenMode = currentScreenMode;

        SetScreenModeText(currentScreenMode);
    }

    private void HandleScreenModeNextButtonClick()
    {
        selectedScreenMode = WindowUIUtil.GetNextInList<string>(
            availableScreenModes,
            selectedScreenMode
        );

        SetScreenModeText(selectedScreenMode);
    }

    private void HandleScreenModePreviousButtonClick()
    {
        selectedScreenMode = WindowUIUtil.GetNextInList<string>(
            availableScreenModes,
            selectedScreenMode,
            false
        );

        SetScreenModeText(selectedScreenMode);
    }

    private void SetScreenModeText(string screenMode)
    {
        screenModeText.text = screenMode.ToUpper();
    }

    ////////// ////////// ////////// ////////// ////////// //////////


    //////////////////////// RESOLUTION HANDLERS ////////////////////////
    private void InitResolutions()
    {
        availableResolutions = GameManager.GetAvailableResolutions();

        currentResolution = PlayerPrefsController.GetResolution();
        selectedResolution = new Pair<int>(currentResolution);

        SetResolutionText(currentResolution);
    }

    private void HandleResNextButtonClick()
    {
        selectedResolution = WindowUIUtil.GetNextInList<Pair<int>>(
            availableResolutions,
            selectedResolution
        );

        SetResolutionText(selectedResolution);
    }

    private void HandleResPreviousButtonClick()
    {
        selectedResolution = WindowUIUtil.GetNextInList<Pair<int>>(
            availableResolutions,
            selectedResolution,
            false
        );

        SetResolutionText(selectedResolution);
    }

    private void SetResolutionText(Pair<int> resolution)
    {
        resolutionText.text = resolution.first.ToString() + " x " + resolution.second.ToString();
    }

    ////////// ////////// ////////// ////////// ////////// //////////

    //////////////////////// RESOLUTION HANDLERS ////////////////////////
    private void InitRefreshRates()
    {
        availableRefreshRates = GameManager.GetAvailableRefreshRates();

        currentRefreshRate = PlayerPrefsController.GetRefreshRate();
        selectedRefreshRate = currentRefreshRate;

        SetRefreshRateText(selectedRefreshRate);
    }

    private void HandleRefreshNextButtonClick()
    {
        selectedRefreshRate = WindowUIUtil.GetNextInList<int>(
            availableRefreshRates,
            selectedRefreshRate
        );

        SetRefreshRateText(selectedRefreshRate);
    }

    private void HandleRefreshPreviousButtonClick()
    {
        selectedRefreshRate = WindowUIUtil.GetNextInList<int>(
            availableRefreshRates,
            selectedRefreshRate,
            false
        );

        SetRefreshRateText(selectedRefreshRate);
    }

    private void SetRefreshRateText(int refreshRate)
    {
        refreshRateText.text = refreshRate.ToString() + " Hz";
    }

    ////////// ////////// ////////// ////////// ////////// //////////

    ////////////////////////// APPLY EFFECTS //////////////////////////
    private void HandleApplyButtonClick()
    {
        // If selected new resolution, screen mode, or refresh rate
        if (currentResolution != selectedResolution || currentScreenMode != selectedScreenMode)
        {
            // Update player pref resolution
            PlayerPrefsController.SetResolution(
                selectedResolution.first,
                selectedResolution.second
            );

            // Update player pref resolution
            PlayerPrefsController.SetScreenMode(selectedScreenMode);

            // Update the current resolution
            currentResolution = new Pair<int>(selectedResolution);

            // Update the current screen mode
            currentScreenMode = selectedScreenMode;

            // Set the resolution
            Screen.SetResolution(
                selectedResolution.first,
                selectedResolution.second,
                WindowUIUtil.StringToScreenMode(selectedScreenMode)
            );
        }

        if (currentRefreshRate != selectedRefreshRate)
        {
            // Update player pref resolution
            PlayerPrefsController.SetRefreshRate(selectedRefreshRate);

            // Update the current referesh rate
            currentRefreshRate = selectedRefreshRate;

            // Set the frame rate
            Application.targetFrameRate = selectedRefreshRate;
        }
    }

    //////////////////////// RESOLUTION HANDLERS ////////////////////////


    ////////////////////// INITIALIZING BUTTONS //////////////////////
    private void InitButton(GameObject button, UnityAction onClick)
    {
        var btn = button.GetComponent<Button>();
        var btnEvents = button.GetComponent<ButtonEventHandler>();
        btn.onClick.AddListener(PlayClickSFX);
        btn.onClick.AddListener(onClick);
        btnEvents.onSelectAction.AddListener(
            delegate
            {
                OnSelectButton(button);
            }
        );
        btnEvents.onDeselectAction.AddListener(
            delegate
            {
                OnDeselectButton(button);
            }
        );
    }

    private void OnSelectButton(GameObject target)
    {
        soundManager.PlayClip(
            soundManager.audioRefs.sfxMouseHover,
            SoundManager.TimedSFX.MOUSE_HOVER
        );

        LeanTween
            .scale(target, new Vector3(1.3f, 1.3f, 1.3f), 0.3f)
            .setEaseOutExpo()
            .setIgnoreTimeScale(true);
    }

    //UI Animation based in button deselection
    private void OnDeselectButton(GameObject target)
    {
        LeanTween.scale(target, Vector3.one, 0.3f).setEaseOutExpo().setIgnoreTimeScale(true);
    }

    private void PlayClickSFX()
    {
        soundManager.PlayClip(soundManager.audioRefs.sfxMenuClick);
    }
    ////////// ////////// ////////// ////////// ////////// //////////
}
