using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIVictoryEvent : UIGameEvent
{
    [SerializeField]
    private Button mainMenuButton;

    private SoundManager soundManager;

    protected override void Awake()
    {
        base.Awake();

        popupTime = 0.8f;

        mainMenuButton.onClick.AddListener(HandleMainMenu);

        mainMenuButton.gameObject.SetActive(false);
    }

    private void Start()
    {
        soundManager = GameManager.SoundManager();

        mainMenuButton.onClick.AddListener(PlayMenuClickSFX);
    }

    protected override void StartEvent()
    {
        mainMenuButton.gameObject.SetActive(true);
    }

    private void PlayMenuClickSFX()
    {
        soundManager.PlayClip(soundManager.audioRefs.sfxMenuClick);
    }

    //TODO: Update this code
    private void HandleMainMenu()
    {
        EndEvent();
        GameManager.ReloadScene(true, 0.5f);
    }
}
