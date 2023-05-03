using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager gameManager;

    // LOADING SCREEN
    private static LoadingScreen loadingScreenPrefab;

    // REFERENCES
    private GatherInput gatherInput;
    private PlayerMovement playerMovement;
    private PlayerDirectionArrow playerDirectionArrow;
    private GameSession gameSession;
    private PlayerStatus playerStatus;
    private SpawnerManager spawnerManager;
    private SoundManager soundManager;
    private MapConfiner mapConfiner;
    private HUDManager hudManager;
    private PauseMenuUIManager pauseMenuUIManager;
    private PauseManager pauseManager;

    // RESOURCES
    private Dictionary<string, SkillData> skillDatum;
    private GameObject damagePopupTextPrefab;
    private GameObject healPopupTextPrefab;
    private Dictionary<Collectable.Type, Collectable> collectablePrefabs;

    // WINDOW UI
    private List<string> availableScreenModes;
    private List<Pair<int>> availableResolutions;
    private List<int> availableRefreshRates;

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
        loadingScreenPrefab = Resources.Load<LoadingScreen>("misc/Loading Screen");

        InitWindowUIs();
        WindowUIUtil.SetScreen();
    }

    ////////// REGISTER REFERENCES / REFERENCE GETTERS //////////
    public static GameManager GetGameManager()
    {
        return gameManager;
    }

    public static void RegisterGatherInput(GatherInput gi)
    {
        if (!gameManager)
        {
            return;
        }

        gameManager.gatherInput = gi;
    }

    public static GatherInput GatherInput()
    {
        return gameManager.gatherInput;
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

    public static void RegisterHUDManager(HUDManager hm)
    {
        if (!gameManager)
        {
            return;
        }

        gameManager.hudManager = hm;
    }

    public static HUDManager HUDManager()
    {
        return gameManager.hudManager;
    }

    public static void RegisterPauseMenuUIManager(PauseMenuUIManager pm)
    {
        if (!gameManager)
        {
            return;
        }

        gameManager.pauseMenuUIManager = pm;
    }

    public static PauseMenuUIManager PauseMenuUIManager()
    {
        return gameManager.pauseMenuUIManager;
    }

    public static void RegisterPauseManager(PauseManager pm)
    {
        if (!gameManager)
        {
            return;
        }

        gameManager.pauseManager = pm;
    }

    public static PauseManager PauseManager()
    {
        return gameManager.pauseManager;
    }

    ////////// ////////// ////////// ////////// ////////// //////////

    //////////////////////// LOAD RESOURCES ////////////////////////
    private void LoadSkillData()
    {
        skillDatum = new Dictionary<string, SkillData>();

        // TextAsset jsonFile = Resources.Load<TextAsset>("Data/skillData");
        // SkillData[] data = JsonHelper.FromJson<SkillData>(jsonFile.text);

        // foreach (SkillData skill in data)
        // {
        //     skill.sprite = Util.LoadSprite(skill.iconPath, skill.iconSubName);

        //     skillDatum.Add(skill.skillName, skill);
        // }

        SkillData[] datum = Resources.LoadAll<SkillData>("Data/skill datum");

        foreach (SkillData data in datum)
        {
            skillDatum.Add(data.SkillName, data);
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
            Collectable.Type.BOMB,
            Resources.Load<Collectable>("collectables/items/bomb")
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
            if (kvp.Value.Type == type)
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
        if (gameManager.skillDatum.TryGetValue(name, out SkillData data))
        {
            return data;
        }

        return null;
    }

    ////////// ////////// ////////// ////////// ////////// //////////

    /////////////////////////// WINDOW UI ///////////////////////////
    private void InitWindowUIs()
    {
        availableScreenModes = WindowUIUtil.GetAvailableScreenModes();
        availableResolutions = WindowUIUtil.GetAvailableResolutions();
        availableRefreshRates = WindowUIUtil.GetAvailableRefreshRates();
    }

    public static List<string> GetAvailableScreenModes()
    {
        return new List<string>(gameManager.availableScreenModes);
    }

    public static List<Pair<int>> GetAvailableResolutions()
    {
        return new List<Pair<int>>(gameManager.availableResolutions);
    }

    public static List<int> GetAvailableRefreshRates()
    {
        return new List<int>(gameManager.availableRefreshRates);
    }

    ////////// ////////// ////////// ////////// ////////// //////////


    /////////////////////////// LOAD SCENE ///////////////////////////
    public static void ReloadScene(bool async = false, float delay = 0)
    {
        LoadScene(SceneManager.GetActiveScene().name, async, delay);
    }

    public static void LoadScene(string name, bool async = false, float delay = 0)
    {
        if (async)
        {
            gameManager.StartCoroutine(LoadSceneAsync(name, delay));
        }
        else
        {
            gameManager.StartCoroutine(LoadSceneSync(name, delay));
        }
    }

    private static IEnumerator LoadSceneAsync(string sceneName, float delay = 0)
    {
        yield return new WaitForSeconds(delay);

        LoadingScreen loadingScreen = GameObject.Instantiate(
            loadingScreenPrefab,
            Vector3.zero,
            Quaternion.identity
        );

        Image loadingBar = loadingScreen.loadingBar;

        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);

        // TODO: delete this
        gameManager.soundManager.PlayMusic(gameManager.soundManager.audioRefs.musicMainMenu);

        while (!op.isDone)
        {
            float progress = Mathf.Clamp01(op.progress / .9f);

            loadingBar.fillAmount = progress;

            yield return null;
        }
    }

    private static IEnumerator LoadSceneSync(string sceneName, float delay = 0)
    {
        yield return new WaitForSeconds(delay);

        SceneManager.LoadScene(sceneName);
    }
    ////////// ////////// ////////// ////////// ////////// //////////
}
