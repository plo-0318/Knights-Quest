using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class GameSession : MonoBehaviour
{
    private float timer;

    private SpawnerManager spawnerManager;
    private PlayerStatus playerStatus;
    private SoundManager soundManager;
    private HUDManager hudManager;
    private float bossBorderFadeOutTime = 3f;

    //////////////////// GAME STATE ////////////////////
    private bool tickTimer;
    private bool gameStarted;
    private bool gamePaused;
    private bool gameOver;
    private bool lastBossFight;

    /////////////////////////////////////////////////////

    //////////////////// LEVEL DETAIL ////////////////////

    [Header("Level")]
    [SerializeField]
    private LevelDetail levelDetail;
    private Modifier[] enemyModifiers;

    /////////////////////////////////////////////////////


    ////////////////// SPAWNING ENEMY //////////////////
    private int maxEnemyPerWave = 5;
    private int maxEnemyTotal = 10;
    private float timeBetweenWaves = 15f;
    public event Action<EnemySpawnUtil.EnemyToSpawn, Modifier[]> onSpawnEnemy;
    public event Action<bool> onKillAllEnemies;
    public event Action<Modifier> onRemoveModifier;
    private HashSet<Enemy> enemyRefs;

    private bool canSpawnEnemy;

    private float enemySpawnTimer,
        spawnWaveTimer;
    private int currentEnemyListIndex;

    /////////////////////////////////////////////////////

    ////////////////// GAME EVENTS //////////////////
    public event Action<Action> onGameStart;
    public event Action onGameLost;
    public event Action onGameWon;
    public event Action<Action> onBossFight;
    private TimedEvents timedEvents;

    /////////////////////////////////////////////////////

    ////////////////// OBJECT HOLDERS //////////////////
    [NonSerialized]
    public Transform enemyParent;

    [NonSerialized]
    public Transform damagePopupParent;

    [NonSerialized]
    public Transform skillParent;

    [NonSerialized]
    public Transform collectableParent;

    /////////////////////////////////////////////////////

    private void Awake()
    {
        GameManager.RegisterGameSession(this);

        timer = enemySpawnTimer = 0f;
        tickTimer = false;

        gameStarted = false;
        gamePaused = false;

        lastBossFight = false;

        spawnWaveTimer = timeBetweenWaves;
        enemyRefs = new HashSet<Enemy>();
        canSpawnEnemy = false;
        currentEnemyListIndex = 0;

        enemyModifiers = null;
        if (levelDetail.enemyModifiers.Length > 0)
        {
            enemyModifiers = levelDetail.enemyModifiers;

            for (int i = 0; i < enemyModifiers.Length; i++)
            {
                enemyModifiers[i].name = gameObject.name;
            }
        }

        CreateObjectHolders();
        InitTimedEvents();

        EnemySpawnUtil.Init(levelDetail);
    }

    private void Start()
    {
        spawnerManager = GameManager.SpawnerManager();
        playerStatus = GameManager.PlayerStatus();
        soundManager = GameManager.SoundManager();
        hudManager = GameManager.HUDManager();

        spawnerManager.InstantiateSpawners(maxEnemyPerWave);

        // TODO: delete
        // BossBorder.Spawn();
    }

    private void Update()
    {
        TickTimer();
        ExecTimedEvents();
        SpawnEnemy();
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    ////////////////////////// HELPERS //////////////////////////
    public float CurrentTime => timer;

    private void TickTimer()
    {
        if (tickTimer)
        {
            timer += Time.deltaTime;
        }
    }

    public void CreateObjectHolders()
    {
        GameObject objectHolder = new GameObject("Object Holder");
        GameObject skillHolder = new GameObject("Skills");
        GameObject damageTextHolder = new GameObject("Damage Texts");
        GameObject enemyHolder = new GameObject("Enemies");
        GameObject collectableHolder = new GameObject("Collectables");

        skillHolder.transform.parent = objectHolder.transform;
        damageTextHolder.transform.parent = objectHolder.transform;
        enemyHolder.transform.parent = objectHolder.transform;
        collectableHolder.transform.parent = objectHolder.transform;

        skillParent = skillHolder.transform;
        damagePopupParent = damageTextHolder.transform;
        enemyParent = enemyHolder.transform;
        collectableParent = collectableHolder.transform;
    }

    ////////////////////////// ////// //////////////////////////

    ////////////////////////// EVENTS //////////////////////////
    public void StartGame()
    {
        if (gameStarted)
        {
            return;
        }

        soundManager.PlayMusic(soundManager.GetRandomActionClip());

        onGameStart?.Invoke(OnStartEventEnd);
        gameStarted = true;
    }

    private void OnStartEventEnd()
    {
        canSpawnEnemy = true;
        tickTimer = true;

        hudManager.ShowHUD();

        GameManager.PlayerStatus().AssignSkill(new SkillSword());
    }

    public void PauseGame()
    {
        if (gamePaused)
        {
            return;
        }

        gamePaused = true;
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        if (!gamePaused)
        {
            return;
        }

        Time.timeScale = 1f;
        gamePaused = false;
    }

    public void HandleGameLost()
    {
        canSpawnEnemy = false;
        tickTimer = false;
        gameOver = true;

        soundManager.PlayClip(soundManager.audioRefs.sfxDefeat);
        soundManager.PlayMusic(soundManager.audioRefs.musicGameOver);

        onGameLost?.Invoke();
    }

    public void HandleGameWon()
    {
        canSpawnEnemy = false;
        tickTimer = false;
        gameOver = true;

        StartCoroutine(DelayGameWonEvent(2.5f));
    }

    public void HandleBossFight()
    {
        tickTimer = false;
        canSpawnEnemy = false;
        KillAllEnemies(false);

        // TODO: display boss ui?

        BossBorder.Spawn();

        onBossFight?.Invoke(OnBossEventEnd);
    }

    private IEnumerator DelayGameWonEvent(float delay)
    {
        yield return new WaitForSeconds(delay);

        soundManager.PlayClip(soundManager.audioRefs.sfxVictory);

        onGameWon?.Invoke();

        yield return new WaitForSeconds(1f);

        soundManager.PlayMusic(soundManager.audioRefs.musicGameOver);
    }

    public void OnBossEventEnd()
    {
        soundManager.PlayMusic(soundManager.audioRefs.musicBoss);
        SpawnBoss();
    }

    private IEnumerator ResumeGameAfterBossDeath(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (lastBossFight)
        {
            HandleGameWon();
        }
        else
        {
            soundManager.PlayMusic(soundManager.GetRandomActionClip());

            tickTimer = true;
            canSpawnEnemy = true;
        }
    }

    public void HandleBossFightEnd()
    {
        BossBorder.FadeOutAndRemove();
        StartCoroutine(ResumeGameAfterBossDeath(bossBorderFadeOutTime));
    }

    private Action GenerateEnemySpawnAction(int maxEnemyPerWave, int maxEnemyTotal, int numSpawners)
    {
        return () =>
        {
            this.maxEnemyPerWave = maxEnemyPerWave;
            this.maxEnemyTotal = maxEnemyTotal;
            spawnerManager.InstantiateSpawners(numSpawners);
        };
    }

    public void InitTimedEvents()
    {
        timedEvents = new TimedEvents();

        timedEvents.AddTimedEvent(45, GenerateEnemySpawnAction(10, 20, 8));
        timedEvents.AddTimedEvent(75, GenerateEnemySpawnAction(13, 25, 8));
        timedEvents.AddTimedEvent(61, GameManager.SpawnerManager().SpawnEliteEnemy);
        timedEvents.AddTimedEvent(100, GenerateEnemySpawnAction(15, 30, 9));
        timedEvents.AddTimedEvent(120, HandleBossFight);
        timedEvents.AddTimedEvent(121, GameManager.SpawnerManager().SpawnEliteEnemy);
        timedEvents.AddTimedEvent(145, GenerateEnemySpawnAction(20, 30, 10));
        timedEvents.AddTimedEvent(180, GenerateEnemySpawnAction(22, 35, 10));
        timedEvents.AddTimedEvent(181, GameManager.SpawnerManager().SpawnEliteEnemy);
        timedEvents.AddTimedEvent(240, HandleBossFight);
        timedEvents.AddTimedEvent(241, GameManager.SpawnerManager().SpawnEliteEnemy);
        timedEvents.AddTimedEvent(242, GenerateEnemySpawnAction(25, 35, 11));
        timedEvents.AddTimedEvent(301, GameManager.SpawnerManager().SpawnEliteEnemy);
        timedEvents.AddTimedEvent(
            360,
            () =>
            {
                HandleBossFight();
                lastBossFight = true;
            }
        );
    }

    private void ExecTimedEvents()
    {
        if (timedEvents.Empty())
        {
            return;
        }

        bool reachedTime = timer >= timedEvents.NextEventTime();

        if (reachedTime)
        {
            timedEvents.NextEvent()();
            timedEvents.Shift();
        }
    }

    ////////////////////////// //////// //////////////////////////

    ////////////////////////// ENEMIES //////////////////////////
    private void SpawnBoss()
    {
        // Enemy bossToSpawn = EnemySpawnUtil.NextBossEnemyToSpawn();

        // Instantiate(bossToSpawn, BossBorder.BossBorderPos(), Quaternion.identity, enemyParent);

        EnemySpawnUtil.SpawnBossAt(BossBorder.GetBossBorderPos(), 3f);
    }

    private void SpawnEnemy()
    {
        if (!canSpawnEnemy)
        {
            return;
        }

        enemySpawnTimer += Time.deltaTime;
        spawnWaveTimer -= Time.deltaTime;

        if (enemyRefs.Count < maxEnemyPerWave)
        {
            onSpawnEnemy?.Invoke(_EnemyToSpawn, enemyModifiers);
        }

        if (spawnWaveTimer <= 0 && enemyRefs.Count <= maxEnemyTotal - maxEnemyPerWave)
        {
            onSpawnEnemy?.Invoke(_EnemyToSpawn, enemyModifiers);

            spawnWaveTimer = timeBetweenWaves;
        }
    }

    private Enemy _EnemyToSpawn()
    {
        // Get the spawn duration for each enemy list
        float duration = levelDetail.levelEnemyDetails[currentEnemyListIndex].spawnDuration;

        // If the current list of enemies have spawned for its maximum duration
        // Reset enemy spawn timer
        // If there are more enemy lists, advance to the next list, else don't advance
        if (enemySpawnTimer >= duration)
        {
            enemySpawnTimer = 0;

            if (currentEnemyListIndex + 1 < levelDetail.levelEnemyDetails.Length)
            {
                currentEnemyListIndex++;
            }
        }

        return EnemySpawnUtil.NextEnemyToSpawn(currentEnemyListIndex);
    }

    public int EnemyCount => enemyRefs.Count;

    public void OnEnemySpawn(Enemy enemy)
    {
        enemyRefs.Add(enemy);
    }

    public void OnEnemyDestroy(Enemy enemy)
    {
        enemyRefs.Remove(enemy);
    }

    public void KillAllEnemies(bool deathByPlayer)
    {
        onKillAllEnemies?.Invoke(deathByPlayer);
    }

    public void RemoveModifierFromAllEnemies(Modifier mod)
    {
        onRemoveModifier?.Invoke(mod);
    }

    // Get the closest enemy position to the point
    public List<Vector3> closestEnemyPosition(Vector3 pos)
    {
        return closestEnemyPositions(pos, 1);
    }

    // Get the n closest enemy positions to the point
    public List<Vector3> closestEnemyPositions(Vector3 fromPos, int n)
    {
        if (enemyRefs.Count == 0 || n <= 0)
        {
            return new List<Vector3>();
        }

        // Create a list of positions because enemy might get destroyed
        // during the calculation or before the value is consumed
        List<Vector3> positions = new List<Vector3>();

        foreach (Enemy enemy in enemyRefs)
        {
            Vector3 enemyPos = enemy.gameObject.transform.position;

            positions.Add(new Vector3(enemyPos.x, enemyPos.y, enemyPos.z));
        }

        // Check if n is less than the amount of enemies
        int count = n <= positions.Count ? n : positions.Count;

        IEnumerable<Vector3> nClosestEnemyPos = positions
            .OrderBy(pos => (pos - fromPos).sqrMagnitude)
            .Take(n);

        return nClosestEnemyPos.ToList<Vector3>();
    }

    // When using this method, should always if enemy reference is null
    public List<Enemy> closestEnemy(Vector3 pos)
    {
        return closestEnemies(pos, 1);
    }

    // When using this method, should always if enemy reference is null
    public List<Enemy> closestEnemies(Vector3 fromPos, int n)
    {
        if (enemyRefs.Count == 0 || n <= 0)
        {
            return new List<Enemy>();
        }

        // Create a list that contains all the enemies
        List<Enemy> enemies = enemyRefs.ToList();

        // Check if n is less than the amount of enemies
        int count = n <= enemies.Count ? n : enemies.Count;

        IEnumerable<Enemy> nClosestEnemies = enemies
            .OrderBy(enemy => (enemy.transform.position - fromPos).sqrMagnitude)
            .Take(n);

        return nClosestEnemies.ToList<Enemy>();
    }

    ////////////////////////// //////// //////////////////////////

    //////////////////////// STATE GETTERS ////////////////////////
    public bool IsGamePaused => gamePaused;
    public bool TimerTicking => tickTimer;
    public bool IsGameOver => gameOver;

    ////////////////////////// //////// //////////////////////////


    // TODO: delete these test functions
    public void TEST_ToggleSpawn()
    {
        canSpawnEnemy = !canSpawnEnemy;
    }

    public void TEST_HurtPlayer()
    {
        float damage = 10f;

        playerStatus.Hurt(damage, Vector2.down);
    }

    public void TEST_SpawnRandomEnemiesAroundPlayer()
    {
        Vector3 playerPos = GameManager.PlayerMovement().gameObject.transform.position;

        float minX = -10f,
            minY = -10f,
            maxX = 10f,
            maxY = 10f;

        var enemy = EnemySpawnUtil.NextEnemyToSpawn(0);

        for (int i = 0; i < 10; i++)
        {
            Vector2 spawnPos = new Vector2(
                UnityEngine.Random.Range(minX, maxX) + playerPos.x,
                UnityEngine.Random.Range(minY, maxY) + playerPos.y
            );

            Debug.Log(spawnPos);

            var e = Instantiate(enemy, spawnPos, Quaternion.identity);
        }
    }

    public List<Enemy> TEST_GetNClosestEnemies(Vector3 fromPos, int n)
    {
        if (enemyRefs.Count == 0 || n <= 0)
        {
            return new List<Enemy>();
        }

        List<Enemy> enemies = new List<Enemy>();

        foreach (Enemy enemy in enemyRefs)
        {
            enemies.Add(enemy);
        }

        int count = n <= enemies.Count ? n : enemies.Count;

        IEnumerable<Enemy> nClosestEnemyPos = enemies
            .OrderBy(enemy => (enemy.transform.position - fromPos).sqrMagnitude)
            .Take(n);

        return nClosestEnemyPos.ToList<Enemy>();
    }

    private IEnumerator TEST_BlinkEnemy(Enemy enemy)
    {
        float blinkDuration = 5f;
        float currentTime = 0;
        float oscillationSpeed = 6f;

        var sprite = enemy.GetComponent<SpriteRenderer>();
        Color originalColor = new Color(
            sprite.color.r,
            sprite.color.g,
            sprite.color.b,
            sprite.color.a
        );

        while (currentTime < blinkDuration)
        {
            var a = Mathf.PingPong(Time.time * oscillationSpeed, 1f);

            sprite.color = new Color(originalColor.r, originalColor.g, originalColor.b, a);

            currentTime += Time.deltaTime;
            yield return null;
        }

        sprite.color = originalColor;
    }

    public void TEST_BlinkNClosestEnemies(int n)
    {
        var enemies = TEST_GetNClosestEnemies(GameManager.PlayerMovement().transform.position, n);

        foreach (var enemy in enemies)
        {
            StartCoroutine(TEST_BlinkEnemy(enemy));
        }
    }

    public void TEST_DisableAllEnemyMovement()
    {
        foreach (var enemy in enemyRefs)
        {
            if (enemy.TryGetComponent<WalkingEnemy>(out var we))
            {
                we.TEST_DisableMovement();
            }
        }
    }

    public void TEST_PickupAllCollectables()
    {
        Collectable.PickUpAllGems();
    }
}
