using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGameOverEvent : UIGameEvent
{
    [SerializeField]
    private Button playAgainButton,
        mainMenuButton;

    private SoundManager soundManager;

    protected override void Awake()
    {
        base.Awake();

        popupTime = 0.8f;

        playAgainButton.onClick.AddListener(HandlePlayAgain);
        mainMenuButton.onClick.AddListener(HandleMainMenu);

        playAgainButton.gameObject.SetActive(false);
        mainMenuButton.gameObject.SetActive(false);
    }

    private void Start()
    {
        soundManager = GameManager.SoundManager();

        playAgainButton.onClick.AddListener(PlayMenuClickSFX);
        mainMenuButton.onClick.AddListener(PlayMenuClickSFX);
    }

    protected override void StartEvent()
    {
        playAgainButton.gameObject.SetActive(true);
        mainMenuButton.gameObject.SetActive(true);
    }

    private void HandlePlayAgain()
    {
        EndEvent();
        GameManager.ReloadScene(0.5f);
    }

    private void PlayMenuClickSFX()
    {
        soundManager.PlayClip(soundManager.audioRefs.sfxMenuClick);
    }

    //TODO: Update this code
    private void HandleMainMenu()
    {
        EndEvent();
        GameManager.ReloadScene(0.5f);
    }
}
