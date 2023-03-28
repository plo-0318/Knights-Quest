using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager gameManager;

    private PlayerMovement playerMovement;
    private PlayerDirectionArrow playerDirectionArrow;
    private GameSession gameSession;
    private PlayerStatus playerStatus;
    private SpawnerManager spawnerManager;

    private Dictionary<string, SkillData> skillData;

    private GameObject _damagePopupText;

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

        skillData = new Dictionary<string, SkillData>();

        TextAsset jsonFile = Resources.Load<TextAsset>("Data/skillData");
        SkillData[] data = JsonHelper.FromJson<SkillData>(jsonFile.text);

        foreach (SkillData skill in data)
        {
            skillData.Add(skill.name, skill);
        }

        _damagePopupText = Resources.Load<GameObject>("Damage Popup Text");
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

    public static void RegisterPlayerStatus(PlayerStatus ps)
    {
        if (!gameManager)
        {
            return;
        }

        gameManager.playerStatus = ps;
    }

    public static PlayerStatus PlayerStatus()
    {
        return gameManager.playerStatus;
    }

    public static void RegisterSpawnerManager(SpawnerManager sm)
    {
        if (!gameManager)
        {
            return;
        }

        gameManager.spawnerManager = sm;
    }

    public static SpawnerManager SpawnerManager()
    {
        return gameManager.spawnerManager;
    }

    public static SkillData GetSkillData(string name)
    {
        if (gameManager.skillData.TryGetValue(name, out SkillData skill))
        {
            return new SkillData(skill);
        }

        return null;
    }

    public static GameObject damagePopupText => gameManager._damagePopupText;
}
