using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameSession : MonoBehaviour
{
    private float timer;

    [SerializeField]
    private TextMeshProUGUI timerText;

    /////////////// Level Detail ///////////////

    [Header("Level")]
    [SerializeField]
    private LevelDetail levelDetail;
    private Modifier[] enemyModifiers;

    ////////////// ////////////// //////////////


    ////////////// Spawning Enemy //////////////
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

    ////////////// ////////////// //////////////



    private void Awake()
    {
        GameManager.RegisterGameSession(this);
    }

    private void Start()
    {
        timer = enemySpawnTimer = 0f;
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

        // TODO: delete this test function
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
        float duration = levelDetail.levelEnemyDetails[currentEnemyListIndex].spawnDuration;

        if (enemySpawnTimer >= duration)
        {
            enemySpawnTimer = 0;

            if (currentEnemyIndex + 1 < levelDetail.levelEnemyDetails.Length)
            {
                currentEnemyListIndex++;
                currentEnemyIndex = 0;
            }
        }

        Enemy[] enemies = levelDetail.levelEnemyDetails[currentEnemyListIndex].enemiesToSpawn;

        Enemy enemy = enemies[currentEnemyIndex];

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

    // TODO: delete this test function

    public void TEST_AddDaggerSkill()
    {
        GameManager.PlayerStat().AssignSkill(new SkillDagger());
    }

    public void TEST_StartSpawn()
    {
        canSpawnEnemy = true;
    }
}
