using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUIManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField]
    private Image backdrop;

    [Header("Buttons")]
    [SerializeField]
    private GameObject playButton;

    [SerializeField]
    private GameObject optionsButton;

    [SerializeField]
    private GameObject creditsButton;

    [SerializeField]
    private GameObject exitButton;

    private CanvasGroup canvasGroup;

    private void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();

        UIUtil.InitButton(playButton, HandlePlay, 1.1f);
        UIUtil.InitButton(optionsButton, Hide, 1.1f);
        UIUtil.InitButton(creditsButton, HandleCredits, 1.1f);
        UIUtil.InitButton(exitButton, HandleExit, 1.1f);
    }

    private void DisableMenuButtons()
    {
        canvasGroup.interactable = false;
    }

    private void EnableMenuButtons()
    {
        canvasGroup.interactable = true;
    }

    private void FadeInBackDrop()
    {
        AnimationUtil.Fade(backdrop, 0.25f, 0, 150f / 255f);
    }

    private void FadeOutBackDrop()
    {
        AnimationUtil.Fade(backdrop, 0.25f, 150f / 255f, 0, true, EnableMenuButtons);
    }

    public void Hide()
    {
        DisableMenuButtons();
        FadeInBackDrop();

        FindObjectOfType<MainMenuOptionsUIManager>().Show();
    }

    public void Show()
    {
        FadeOutBackDrop();
    }

    private void HandlePlay()
    {
        GameManager.LoadScene("Level 1", true);
    }

    private void HandleCredits()
    {
        GameManager.LoadScene("Credits", true);
    }

    private void HandleExit()
    {
        Application.Quit();
    }
}
