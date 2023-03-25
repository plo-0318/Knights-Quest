using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class GameSession : MonoBehaviour
{
    private float timer;
    private bool tickTimer;

    [SerializeField]
    private TextMeshProUGUI timerText;

    PlayerStatus playerStatus;

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
    public event Action<Enemy, Modifier[]> OnSpawnEnemy;
    public event Action OnKillAllEnemies;
    public event Action<Modifier> OnRemoveModifier;
    private HashSet<Enemy> enemyRefs;

    private bool canSpawnEnemy;

    private float enemySpawnTimer,
        spawnWaveTimer;
    private int currentEnemyListIndex,
        currentEnemyIndex;

    public Transform enemyParent;

    /////////////////////////////////////////////////////


    private void Awake()
    {
        GameManager.RegisterGameSession(this);
    }

    private void Start()
    {
        timer = enemySpawnTimer = 0f;
        tickTimer = true;

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

        playerStatus = GameManager.PlayerStatus();

        playerStatus.onPlayerDeath += HandleGameOver;
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

    private void OnDestroy()
    {
        playerStatus.onPlayerDeath -= HandleGameOver;
    }

    public float Timer => timer;

    private void TickTimer()
    {
        if (tickTimer)
        {
            timer += Time.deltaTime;
        }
    }

    private void InitSession() { }

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
            OnSpawnEnemy?.Invoke(EnemyToSpawn(), enemyModifiers);
        }

        if (spawnWaveTimer <= 0 && enemyRefs.Count <= maxEnemyTotal - maxEnemyPerWave)
        {
            OnSpawnEnemy?.Invoke(EnemyToSpawn(), enemyModifiers);

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
        OnKillAllEnemies?.Invoke();
    }

    public void RemoveModifierFromAllEnemies(Modifier mod)
    {
        OnRemoveModifier?.Invoke(mod);
    }

    private void HandleGameOver()
    {
        canSpawnEnemy = false;
    }

    // Get the closest enemy position to the point
    public Vector3 closestEnemyPosition(Vector3 pos)
    {
        return closestEnemyPositions(pos, 1).First();
    }

    // Get the n closest enemy positions to the point
    public IEnumerable<Vector3> closestEnemyPositions(Vector3 fromPos, int n)
    {
        if (enemyRefs.Count == 0 || n <= 0)
        {
            return null;
        }

        // Create a list of positions because enemy might get destroyed
        // during the calculation or before the value is consumed
        List<Vector3> positions = new List<Vector3>();

        foreach (Enemy enemy in enemyRefs)
        {
            Vector3 enemyPos = enemy.gameObject.transform.position;

            positions.Add(new Vector3(enemyPos.x, enemyPos.y, enemyPos.z));
        }

        int count = n <= positions.Count ? n : positions.Count;

        IEnumerable<Vector3> nClosestEnemyPos = positions
            .OrderBy(pos => (pos - fromPos).sqrMagnitude)
            .Take(n);

        return nClosestEnemyPos;
    }

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
}
