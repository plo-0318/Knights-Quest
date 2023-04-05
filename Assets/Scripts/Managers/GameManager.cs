using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager gameManager;

    // REFERENCES
    private PlayerMovement playerMovement;
    private PlayerDirectionArrow playerDirectionArrow;
    private GameSession gameSession;
    private PlayerStatus playerStatus;
    private SpawnerManager spawnerManager;
    private SoundManager soundManager;
    private MapConfiner mapConfiner;

    // RESOURCES
    private Dictionary<string, SkillData> skillDatum;
    private GameObject damagePopupTextPrefab;
    private GameObject healPopupTextPrefab;
    private Dictionary<Collectable.Type, Collectable> collectablePrefabs;

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

        LoadSkillData();
        LoadPopupTexts();
        LoadCollectables();
    }

    ////////// REGISTER REFERENCES / REFERENCE GETTERS //////////
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

    public static void RegisterSoundManager(SoundManager sm)
    {
        if (!gameManager)
        {
            return;
        }

        gameManager.soundManager = sm;
    }

    public static SoundManager SoundManager()
    {
        return gameManager.soundManager;
    }

    public static void RegisterMapConfiner(MapConfiner mc)
    {
        if (!gameManager)
        {
            return;
        }

        gameManager.mapConfiner = mc;
    }

    public static MapConfiner MapConfiner()
    {
        return gameManager.mapConfiner;
    }

    ////////// ////////// ////////// ////////// ////////// //////////

    //////////////////////// LOAD RESOURCES ////////////////////////
    private void LoadSkillData()
    {
        skillDatum = new Dictionary<string, SkillData>();

        TextAsset jsonFile = Resources.Load<TextAsset>("Data/skillData");
        SkillData[] data = JsonHelper.FromJson<SkillData>(jsonFile.text);

        foreach (SkillData skill in data)
        {
            skill.sprite = Util.LoadSprite(skill.iconPath, skill.iconSubName);

            skillDatum.Add(skill.name, skill);
        }
    }

    private void LoadPopupTexts()
    {
        damagePopupTextPrefab = Resources.Load<GameObject>("Damage Popup Text");
        healPopupTextPrefab = Resources.Load<GameObject>("Heal Popup Text");
    }

    private void LoadCollectables()
    {
        collectablePrefabs = new Dictionary<Collectable.Type, Collectable>();

        collectablePrefabs.Add(
            Collectable.Type.GEM_GREEN,
            Resources.Load<Collectable>("collectables/gems/green exp gem")
        );
        collectablePrefabs.Add(
            Collectable.Type.GEM_BLUE,
            Resources.Load<Collectable>("collectables/gems/blue exp gem")
        );
        collectablePrefabs.Add(
            Collectable.Type.GEM_ORANGE,
            Resources.Load<Collectable>("collectables/gems/orange exp gem")
        );
        collectablePrefabs.Add(
            Collectable.Type.GEM_RED,
            Resources.Load<Collectable>("collectables/gems/red exp gem")
        );
        collectablePrefabs.Add(
            Collectable.Type.POTION,
            Resources.Load<Collectable>("collectables/items/potion")
        );
        collectablePrefabs.Add(
            Collectable.Type.SHIELD,
            Resources.Load<Collectable>("collectables/items/shield")
        );
        collectablePrefabs.Add(
            Collectable.Type.POUCH,
            Resources.Load<Collectable>("collectables/items/pouch")
        );
    }

    ////////// ////////// ////////// ////////// ////////// //////////

    //////////////////////// RESOURCES GETTERS ////////////////////////
    public static GameObject DamagePopupText => gameManager.damagePopupTextPrefab;
    public static GameObject HealPopupText => gameManager.healPopupTextPrefab;

    public static Collectable GetCollectable(Collectable.Type type)
    {
        if (gameManager.collectablePrefabs.TryGetValue(type, out var collectable))
        {
            return collectable;
        }

        return null;
    }

    public static ReadOnlyDictionary<string, SkillData> GetAllAttackingSkillData()
    {
        return GetAllTypeSkillData("ATTACK");
    }

    public static ReadOnlyDictionary<string, SkillData> GetAllUtilitySkillData()
    {
        return GetAllTypeSkillData("UTILITY");
    }

    private static ReadOnlyDictionary<string, SkillData> GetAllTypeSkillData(string type)
    {
        Dictionary<string, SkillData> typeSkillData = new Dictionary<string, SkillData>();

        foreach (var kvp in gameManager.skillDatum)
        {
            if (kvp.Value.type == type)
            {
                typeSkillData.Add(kvp.Key, kvp.Value);
            }
        }

        return new ReadOnlyDictionary<string, SkillData>(typeSkillData);
    }

    public static ReadOnlyDictionary<string, SkillData> GetAllSkillData()
    {
        return new ReadOnlyDictionary<string, SkillData>(gameManager.skillDatum);
    }

    public static SkillData GetSkillData(string name)
    {
        if (gameManager.skillDatum.TryGetValue(name, out SkillData skill))
        {
            return new SkillData(skill);
        }

        return null;
    }

    ////////// ////////// ////////// ////////// ////////// //////////

    public static void ReloadScene(float delay = 0)
    {
        gameManager.StartCoroutine(DelayReloadScene(delay));
    }

    private static IEnumerator DelayReloadScene(float time)
    {
        yield return new WaitForSeconds(time);

        Scene scene = SceneManager.GetActiveScene();

        gameManager.soundManager.PlayMusic(gameManager.soundManager.audioRefs.musicMainMenu);
        SceneManager.LoadScene(scene.name);
    }
}
