using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class GameSession : MonoBehaviour
{
    private float timer;

    [SerializeField]
    private TextMeshProUGUI timerText;

    private PlayerStatus playerStatus;
    private SoundManager soundManager;

    //////////////////// GAME STATE ////////////////////
    private bool tickTimer;
    private bool gameStarted;
    private bool gamePaused;

    /////////////////////////////////////////////////////

    //////////////////// LEVEL DETAIL ////////////////////

    [Header("Level")]
    [SerializeField]
    private LevelDetail levelDetail;
    private Modifier[] enemyModifiers;

    /////////////////////////////////////////////////////


    ////////////////// SPAWNING ENEMY //////////////////
    [Header("Enemy")]
    [SerializeField]
    private int maxEnemyPerWave = 30;

    [SerializeField]
    private int maxEnemyTotal = 100;

    [SerializeField]
    private float timeBetweenWaves = 15f;
    public event Action<Enemy, Modifier[]> onSpawnEnemy;
    public event Action onKillAllEnemies;
    public event Action<Modifier> onRemoveModifier;
    private HashSet<Enemy> enemyRefs;

    private bool canSpawnEnemy;

    private float enemySpawnTimer,
        spawnWaveTimer;
    private int currentEnemyListIndex,
        currentEnemyIndex;

    /////////////////////////////////////////////////////

    ////////////////// GAME EVENTS //////////////////
    public event Action onGameStart;
    public event Action onGameLost;

    /////////////////////////////////////////////////////

    ////////////////// OBJECT HOLDERS //////////////////
    [Header("Object Holders")]
    [Tooltip("All the spawned enemies will be the children of this game object")]
    public Transform enemyParent;

    [Tooltip("All the spawned damage popups will be the children of this game object")]
    public Transform damagePopupParent;

    [Tooltip("All the spawned skills will be the children of this game object")]
    public Transform skillParent;

    [Tooltip("All the spawned collectables will be the children of this game object")]
    public Transform collectableParent;

    /////////////////////////////////////////////////////

    private void Awake()
    {
        GameManager.RegisterGameSession(this);

        timer = enemySpawnTimer = 0f;
        tickTimer = false;

        gameStarted = false;
        gamePaused = false;

        spawnWaveTimer = timeBetweenWaves;
        enemyRefs = new HashSet<Enemy>();
        canSpawnEnemy = false;
        currentEnemyListIndex = currentEnemyIndex = 0;

        enemyModifiers = null;
        if (levelDetail.enemyModifiers.Length > 0)
        {
            enemyModifiers = levelDetail.enemyModifiers;

            for (int i = 0; i < enemyModifiers.Length; i++)
            {
                enemyModifiers[i].id = gameObject.GetInstanceID();
            }
        }
    }

    private void Start()
    {
        playerStatus = GameManager.PlayerStatus();
        soundManager = GameManager.SoundManager();
    }

    private void Update()
    {
        TickTimer();
        SpawnEnemy();
    }

    private void FixedUpdate()
    {
        // TODO: Delete this test
        if (timerText)
        {
            timerText.text = GetTimeString();
        }
    }

    private void OnDestroy() { }

    public float Timer => timer;

    private void TickTimer()
    {
        if (tickTimer)
        {
            timer += Time.deltaTime;
        }
    }

    public void StartGame()
    {
        if (gameStarted)
        {
            return;
        }

        soundManager.PlayMusic(soundManager.GetRandomActionClip());

        onGameStart?.Invoke();
        gameStarted = true;
    }

    public void HandleStartEventEnd()
    {
        canSpawnEnemy = true;
        tickTimer = true;
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

        soundManager.PlayClip(soundManager.audioRefs.sfxDefeat);
        soundManager.PlayMusic(soundManager.audioRefs.musicGameOver);

        onGameLost?.Invoke();
    }

    public string GetTimeString()
    {
        int seconds = Mathf.RoundToInt(timer);
        int minutes = seconds / 60;
        seconds %= 60;

        string timerStr = minutes.ToString() + ":";

        if (seconds < 10)
        {
            timerStr += "0";
        }

        timerStr += seconds.ToString();

        return timerStr;
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
            onSpawnEnemy?.Invoke(EnemyToSpawn(), enemyModifiers);
        }

        if (spawnWaveTimer <= 0 && enemyRefs.Count <= maxEnemyTotal - maxEnemyPerWave)
        {
            onSpawnEnemy?.Invoke(EnemyToSpawn(), enemyModifiers);

            spawnWaveTimer = timeBetweenWaves;
        }
    }

    private Enemy EnemyToSpawn()
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
                currentEnemyIndex = 0;
            }
        }

        Enemy[] enemies = levelDetail.levelEnemyDetails[currentEnemyListIndex].enemiesToSpawn;

        Enemy enemy = enemies[currentEnemyIndex];

        // Cycle through the enemies in the list
        currentEnemyIndex = ++currentEnemyIndex < enemies.Length ? currentEnemyIndex : 0;

        return enemy;
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

    public void KillAllEnemies()
    {
        onKillAllEnemies?.Invoke();
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

    public bool GamePaused => gamePaused;

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

        var enemy = levelDetail.levelEnemyDetails[0].enemiesToSpawn[0];

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
        Collectable.PickUpAllCollectables();
    }
}
