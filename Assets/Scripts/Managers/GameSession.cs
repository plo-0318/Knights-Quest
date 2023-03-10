using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameSession : MonoBehaviour
{
    private float timer;

    [Header("Level")]
    [SerializeField]
    private LevelDetail levelDetail;

    [SerializeField]
    private TextMeshProUGUI timerText;

    ////////////// Spawning Enemy //////////////
    [Header("Enemy")]
    [SerializeField]
    private int maxEnemyPerWave = 30;

    [SerializeField]
    private int maxEnemyTotal = 100;

    [SerializeField]
    private float timeBetweenWaves = 15f;
    public event Action<Enemy> OnSpawnEnemy;
    public event Action OnKillAllEnemies;
    private int enemyCount;
    private bool canSpawnEnemy;

    private float enemySpawnTimer,
        spawnWaveTimer;
    private int currentEnemyListIndex,
        currentEnemyIndex;

    public Transform enemyParent;

    ////////////// ////////////// //////////////


    private void Awake()
    {
        GameManager.RegisterGameSession(this);
    }

    private void Start()
    {
        timer = enemySpawnTimer = 0f;
        spawnWaveTimer = timeBetweenWaves;
        enemyCount = 0;
        canSpawnEnemy = false;
        currentEnemyListIndex = currentEnemyIndex = 0;

        Invoke("TEST_StartSpawn", 3f);
    }

    private void Update()
    {
        timer += Time.deltaTime;

        SpawnEnemy();
    }

    private void FixedUpdate()
    {
        if (timerText)
        {
            timerText.text = GetTimeString();
        }
    }

    public float GetTime()
    {
        return timer;
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

        if (enemyCount < maxEnemyPerWave)
        {
            OnSpawnEnemy?.Invoke(EnemyToSpawn());
        }

        if (spawnWaveTimer <= 0 && enemyCount <= maxEnemyTotal - maxEnemyPerWave)
        {
            OnSpawnEnemy?.Invoke(EnemyToSpawn());

            spawnWaveTimer = timeBetweenWaves;
        }
    }

    private Enemy EnemyToSpawn()
    {
        float duration = levelDetail.levelEnemyDetails[currentEnemyListIndex].spawnDuration;

        if (enemySpawnTimer >= duration)
        {
            enemySpawnTimer = 0;

            if (currentEnemyIndex + 1 < levelDetail.levelEnemyDetails.Count)
            {
                currentEnemyListIndex++;
                currentEnemyIndex = 0;
            }
        }

        List<Enemy> enemies = levelDetail.levelEnemyDetails[currentEnemyListIndex].enemiesToSpawn;

        Enemy enemy = enemies[currentEnemyIndex];

        currentEnemyIndex = ++currentEnemyIndex < enemies.Count ? currentEnemyIndex : 0;

        return enemy;
    }

    public int EnemyCount => enemyCount;

    public void OnEnemySpawn()
    {
        enemyCount++;
    }

    public void OnEnemyDestroy()
    {
        enemyCount--;
    }

    public void KillAllEnemies()
    {
        OnKillAllEnemies?.Invoke();
    }

    public void TEST_AddDaggerSkill()
    {
        GameManager.PlayerStat().AssignSkill(new SkillDagger());
    }

    public void TEST_StartSpawn()
    {
        canSpawnEnemy = true;
    }
}
