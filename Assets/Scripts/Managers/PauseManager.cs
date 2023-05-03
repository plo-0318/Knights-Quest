using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseManager : MonoBehaviour
{
    private bool pauseMenuOpen;
    private GameSession gameSession;
    private SoundManager soundManager;

    private void Awake()
    {
        GameManager.RegisterPauseManager(this);

        pauseMenuOpen = false;
    }

    private void Start()
    {
        gameSession = GameManager.GameSession();
        soundManager = GameManager.SoundManager();

        GameManager.GatherInput().onPausePressed += TogglePauseMenu;
    }

    private void TogglePauseMenu()
    {
        // If pause menu is not open, open it
        if (!pauseMenuOpen)
        {
            gameSession.PauseGame();
            soundManager.PlayClip(soundManager.audioRefs.sfxMenuOpen);
            GameManager.PauseMenuUIManager().ShowPauseMenu();
        }
        // Close the pause menu
        else
        {
            GameManager.PauseMenuUIManager().HidePauseMenu(gameSession.ResumeGame);
        }

        pauseMenuOpen = !pauseMenuOpen;
    }

    public void HidePauseMenu()
    {
        if (!pauseMenuOpen)
        {
            return;
        }

        GameManager.PauseMenuUIManager().HidePauseMenu(gameSession.ResumeGame);
        pauseMenuOpen = false;
    }
}
