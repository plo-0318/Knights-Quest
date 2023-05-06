using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EnemySpawnUtil
{
    private static LevelDetail _levelDetail;
    private static List<List<Enemy>> enemyLists;
    private static List<int> enemyIndexes;
    public delegate Enemy EnemyToSpawn();

    private static int eliteEnemyIndex;
    private static int bossIndex;
    private static Enemy currentBoss;

    private static Indicator bossIndicator;

    static EnemySpawnUtil()
    {
        bossIndicator = Resources.Load<Indicator>("Misc/boss indicator");
    }

    private static void InitIndices()
    {
        eliteEnemyIndex = 0;
        bossIndex = 0;
        currentBoss = null;
    }

    public static void Init(LevelDetail levelDetail)
    {
        InitIndices();

        enemyLists = new List<List<Enemy>>();
        enemyIndexes = new List<int>();

        _levelDetail = levelDetail;

        foreach (LevelEnemyDetail led in _levelDetail.levelEnemyDetails)
        {
            List<Enemy> enemiesToSpawn = new List<Enemy>();

            // Adding all the enemies into the enemies to spawn list
            // Add the enemy n number of times
            // n: the propotion of the enemy
            foreach (var levelEnemy in led.levelEnemies)
            {
                int count = levelEnemy.proportion;
                Enemy enemy = levelEnemy.enemy;

                for (int i = 0; i < count; i++)
                {
                    enemiesToSpawn.Add(enemy);
                }
            }

            enemyIndexes.Add(0);
            enemyLists.Add(enemiesToSpawn);
        }
    }

    public static void ApplyGlobalModifiers(Enemy enemy)
    {
        GameSession gameSession = GameManager.GameSession();

        EnemyModifier[] enemyModifiers = gameSession.enemyModifiers;

        if (enemyModifiers != null && enemyModifiers.Length > 0)
        {
            enemy.Init(enemyModifiers);
        }
    }

    public static Enemy NextEnemyToSpawn(int enemyListIndex)
    {
        // Get the current enemy list
        List<Enemy> enemiesToSpawn = enemyLists[enemyListIndex];

        // Get the current enemy index
        int enemyIndex = enemyIndexes[enemyListIndex];

        // Get the current enemy, and increment the enemy index
        Enemy enemyToSpawn = enemiesToSpawn[enemyIndex++];

        // If index is out of bound, reset it to 0
        enemyIndex = enemyIndex >= enemiesToSpawn.Count ? 0 : enemyIndex;

        // Save the index
        enemyIndexes[enemyListIndex] = enemyIndex;

        return enemyToSpawn;
    }

    public static Enemy NextEliteEnemyToSpawn()
    {
        if (eliteEnemyIndex >= _levelDetail.eliteEnemyEvents.Length)
        {
            return null;
        }

        // TODO: delete this log
        Debug.Log("spawning elite [" + eliteEnemyIndex + "]");

        Enemy eliteEnemy = _levelDetail.eliteEnemyEvents[eliteEnemyIndex++].enemy;

        return eliteEnemy;
    }

    public static void DecrementEliteEnemyIndex()
    {
        eliteEnemyIndex--;
    }

    public static Enemy NextBossEnemyToSpawn()
    {
        if (bossIndex >= _levelDetail.bosses.Length)
        {
            return null;
        }

        Enemy boss = _levelDetail.bosses[bossIndex++];

        return boss;
    }

    public static void SpawnBossAt(Vector3 spawnPos, float blinkDuration)
    {
        // Spawn a boss indicator
        Indicator indicator = GameObject.Instantiate(bossIndicator, spawnPos, Quaternion.identity);

        // Start the boss indicator, after indicator finishes, spawn the boss
        indicator.StartBlink(
            blinkDuration,
            () =>
            {
                currentBoss = GameObject.Instantiate(
                    NextBossEnemyToSpawn(),
                    spawnPos,
                    Quaternion.identity,
                    GameManager.GameSession().enemyParent
                );

                // If there is a global enemy modifier, apply it
                ApplyGlobalModifiers(currentBoss);
            }
        );
    }

    public static Enemy CurrentBoss => currentBoss;
}
