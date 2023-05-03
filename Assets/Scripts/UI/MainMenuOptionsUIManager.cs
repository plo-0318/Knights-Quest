using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class MainMenuOptionsUIManager : MonoBehaviour
{
    [Header("Screens")]
    [SerializeField]
    private GameObject optionsScreen;

    [SerializeField]
    private GameObject resolutionScreen;

    [SerializeField]
    private GameObject volumeScreen;

    [SerializeField]
    private GameObject gameScreen;

    [Header("Option Buttons")]
    [SerializeField]
    private GameObject resolutionButton;

    [SerializeField]
    private GameObject volumeButton;

    [SerializeField]
    private GameObject gameButton;

    [Header("Back Buttons")]
    [SerializeField]
    private GameObject optionsBackButton;

    [SerializeField]
    private GameObject resolutionBackButton;

    [SerializeField]
    private GameObject volumeBackButton;

    [SerializeField]
    private GameObject gameBackButton;

    private void Start()
    {
        UIUtil.InitButton(resolutionButton, NavigateToResolution, 1.1f);
        UIUtil.InitButton(volumeButton, NavigateToVolume, 1.1f);
        UIUtil.InitButton(gameButton, NavigateToGame, 1.1f);

        UIUtil.InitButton(optionsBackButton, Close, 1.1f);
        UIUtil.InitButton(resolutionBackButton, NavigateToOptions, 1.1f);
        UIUtil.InitButton(volumeBackButton, NavigateToOptions, 1.1f);
        UIUtil.InitButton(gameBackButton, NavigateToOptions, 1.1f);
    }

    public void Show()
    {
        CloseAllSubMenus();
        NavigateToOptions();
    }

    public void Close()
    {
        CloseAllSubMenus();

        FindObjectOfType<MainMenuUIManager>().Show();
    }

    private void CloseAllSubMenus()
    {
        EventSystem.current.SetSelectedGameObject(null);
        optionsScreen.SetActive(false);
        resolutionScreen.SetActive(false);
        volumeScreen.SetActive(false);
        gameScreen.SetActive(false);
    }

    private void NavigateToOptions()
    {
        CloseAllSubMenus();

        optionsScreen.SetActive(true);
    }

    private void NavigateToResolution()
    {
        CloseAllSubMenus();

        resolutionScreen.GetComponent<ResolutionUIManager>().Init();
        resolutionScreen.SetActive(true);
    }

    private void NavigateToVolume()
    {
        CloseAllSubMenus();

        volumeScreen.SetActive(true);
    }

    private void NavigateToGame()
    {
        CloseAllSubMenus();

        gameScreen.SetActive(true);
    }
}
