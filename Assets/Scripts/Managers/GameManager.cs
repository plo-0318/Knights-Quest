using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager gameManager;

    private PlayerMovement playerMovement;
    private PlayerDirectionArrow playerDirectionArrow;
    private GameSession gameSession;
    private PlayerStat playerStat;

    private void Awake()
    {
        if (!gameManager)
        {
            gameManager = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static GameManager GetGameManager()
    {
        return gameManager;
    }

    public static void RegisterPlayerMovement(PlayerMovement pm)
    {
        if (!gameManager)
        {
            return;
        }

        gameManager.playerMovement = pm;
    }

    public static PlayerMovement PlayerMovement()
    {
        return gameManager.playerMovement;
    }

    public static void RegisterPlayerDirectionArrow(PlayerDirectionArrow pda)
    {
        if (!gameManager)
        {
            return;
        }

        gameManager.playerDirectionArrow = pda;
    }

    public static PlayerDirectionArrow PlayerDirectionArrow()
    {
        return gameManager.playerDirectionArrow;
    }

    public static void RegisterGameSession(GameSession gs)
    {
        if (!gameManager)
        {
            return;
        }

        gameManager.gameSession = gs;
    }

    public static GameSession GameSession()
    {
        return gameManager.gameSession;
    }

    public static void RegisterPlayerStat(PlayerStat ps)
    {
        if (!gameManager)
        {
            return;
        }

        gameManager.playerStat = ps;
    }

    public static PlayerStat PlayerStat()
    {
        return gameManager.playerStat;
    }
}
