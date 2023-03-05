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
    private int maxEnemyCount = 50;
    public event Action<Enemy> SpawnEnemy;
    public event Action OnKillAllEnemies;
    private int enemyCount;
    private bool canSpawnEnemy;

    private float enemySpawnTimer;
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
        enemyCount = 0;
        canSpawnEnemy = false;
        currentEnemyListIndex = currentEnemyIndex = 0;

        Invoke("TEST_StartSpawn", 3f);
    }

    private void Update()
    {
        timer += Time.deltaTime;
        enemySpawnTimer += Time.deltaTime;

        if (canSpawnEnemy && enemyCount < maxEnemyCount)
        {
            SpawnEnemy?.Invoke(EnemyToSpawn());
        }
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
