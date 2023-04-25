using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIEventManager : MonoBehaviour
{
    [SerializeField]
    private UIGameEvent startEvent;

    [SerializeField]
    private UIGameEvent bossEvent;

    [SerializeField]
    private UIGameEvent gameOverEvent;

    private void Awake()
    {
        InitEvents();
    }

    //TODO: delte this
    private void Start()
    {
        // Invoke("TEST_StartEvent", 1);
        // Invoke("TEST_BossEvent", 1);
        // Invoke("TEST_GameOverEvent", 1);
    }

    private void InitEvents()
    {
        GameSession gameSession = GameManager.GameSession();

        // On game start --> Play start event
        // On start event end --> game session will handle start event handle
        gameSession.onGameStart += startEvent.PlayEvent;

        // On boss fight --> Play boss event
        // On boss event end --> game session will handle boss spawning
        gameSession.onBossFight += bossEvent.PlayEvent;

        // On game lost --> Play game over event
        gameSession.onGameLost += gameOverEvent.PlayEvent;
    }
}
