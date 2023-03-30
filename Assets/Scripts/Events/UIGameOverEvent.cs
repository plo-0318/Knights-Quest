using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGameOverEvent : UIGameEvent
{
    [SerializeField]
    private Button playAgainButton,
        mainMenuButton;

    protected override void Awake()
    {
        base.Awake();

        fadeInSpeed = 1f;

        playAgainButton.onClick.AddListener(HandlePlayAgain);
        mainMenuButton.onClick.AddListener(HandleMainMenu);

        playAgainButton.gameObject.SetActive(false);
        mainMenuButton.gameObject.SetActive(false);
    }

    protected override void StartEvent()
    {
        playAgainButton.gameObject.SetActive(true);
        mainMenuButton.gameObject.SetActive(true);
    }

    private void HandlePlayAgain()
    {
        EndEvent();
        GameManager.ReloadScene();
    }

    //TODO: Update this code
    private void HandleMainMenu()
    {
        EndEvent();
        GameManager.ReloadScene();
    }
}
